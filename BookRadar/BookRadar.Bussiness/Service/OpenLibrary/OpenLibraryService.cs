using BookRadar.Bussiness.Service.Http;
using BookRadar.Common.Configurations;
using BookRadar.Common.DTOs;
using BookRadar.Common.IOptionPattern;
using BookRadar.Common.Response;

namespace BookRadar.Bussiness.Service.OpenLibrary
{
    public class OpenLibraryService : IOpenLibraryService
    {
        private readonly IHttpService _httpService;
        private readonly string _baseUrl;

        public OpenLibraryService(IHttpService httpService, IGenericOptionsService<OpenLibraryOptions> optionsService)
        {
            _httpService = httpService;
            _baseUrl = optionsService.GetSnapshotOptions().OpenLibrary;
        }

        public async Task<PagedResultDTO<LibroDTO>> SearchByAuthorAsync(string author, int page = 1, int pageSize = 10)
        {
            var url = $"{_baseUrl}search.json?author={Uri.EscapeDataString(author)}&limit={pageSize}&page={page}&fields=title,first_publish_year,publisher,numFound";

            var response = await _httpService.GetAsync<OpenLibraryResponse>(url);

            var items = response?.Docs?
                .Select(d => new LibroDTO
                {
                    Titulo = d.Title ?? string.Empty,
                    AnioPublicacion = d.FirstPublishYear,
                    Editorial = d.Publisher?.FirstOrDefault()
                })
                .ToList() ?? [];

            return new PagedResultDTO<LibroDTO>
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalItems = response?.NumFound ?? 0
            };
        }

    }
}