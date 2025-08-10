using BookRadar.Bussiness.Service.Http;
using BookRadar.Common.Configurations;
using BookRadar.Common.DTOs;
using BookRadar.Common.Entities;
using BookRadar.Common.IOptionPattern;
using BookRadar.Common.Response;
using BookRadar.DataAccess.UnitOfWork;
using Mapster;

namespace BookRadar.Bussiness.Service.OpenLibrary
{
    public class OpenLibraryService : IOpenLibraryService
    {
        private readonly IHttpService _httpService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly string _baseUrl;

        public OpenLibraryService(IHttpService httpService, IGenericOptionsService<OpenLibraryOptions> optionsService, IUnitOfWork unitOfWork)
        {
            _httpService = httpService;
            _unitOfWork = unitOfWork;
            _baseUrl = optionsService.GetSnapshotOptions().OpenLibrary;
        }

        public async Task<PagedResultDTO<LibroDTO>> SearchByAuthorAsync(string author, int page = 1, int pageSize = 10)
        {
            if (string.IsNullOrWhiteSpace(author))
            {
                return new PagedResultDTO<LibroDTO>
                {
                    Items = [],
                    Page = page,
                    PageSize = pageSize,
                    TotalItems = 0
                };
            }

            var url = $"{_baseUrl}search.json?author={Uri.EscapeDataString(author)}&limit={pageSize}&page={page}&fields=title,first_publish_year,publisher,numFound";
            var response = await _httpService.GetAsync<OpenLibraryResponse>(url);

            var items = response?.Docs?
                .Select(d => new LibroDTO
                {
                    Titulo = d.Title ?? string.Empty,
                    AnioPublicacion = d.FirstPublishYear,
                    Editorial = d.Publisher?.FirstOrDefault() ?? string.Empty
                })
                .ToList() ?? [];

            if (items.Any())
            {
                var fecha = DateTime.UtcNow;
                var limiteTiempo = fecha.AddMinutes(-1);

                var recientes = await _unitOfWork.HistorialRepository
                    .GetRecentTitlesByAuthorAsync(author, limiteTiempo);

                var setRecientes = recientes.ToHashSet(StringComparer.OrdinalIgnoreCase);

                var nuevos = items
                    .Where(libro => !setRecientes.Contains(libro.Titulo))
                    .Select(libro => new HistorialBusqueda
                    {
                        Autor = author,
                        Titulo = libro.Titulo,
                        AnioPublicacion = libro.AnioPublicacion,
                        Editorial = libro.Editorial ?? string.Empty,
                        FechaConsulta = fecha
                    })
                    .ToList();

                if (nuevos.Count > 0)
                {
                    await _unitOfWork.BeginTransactionAsync();
                    try
                    {
                        await _unitOfWork.HistorialRepository.AddRangeAsync(nuevos);
                        await _unitOfWork.SaveChangesAsync();
                        await _unitOfWork.CommitAsync();
                    }
                    catch
                    {
                        await _unitOfWork.RollbackAsync();
                        throw;
                    }
                }
            }

            return new PagedResultDTO<LibroDTO>
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalItems = response?.NumFound ?? 0
            };
        }

        public async Task<PagedResultDTO<HistorialBusquedaDTO>> ObtenerHistorialAsync(
        int page, int pageSize)
        {
            var (Items, TotalRows) = await _unitOfWork.HistorialRepository.ObtenerHistorialAsync(page, pageSize);
            var dto = Items.Adapt<List<HistorialBusquedaDTO>>();
            return new PagedResultDTO<HistorialBusquedaDTO>
            {
                Items = dto,
                PageSize = Items.Count,
                TotalItems = TotalRows
            };
        }
    }
}