using BookRadar.Common.DTOs;

namespace BookRadar.Bussiness.Service.OpenLibrary
{
    public interface IOpenLibraryService
    {
        Task<PagedResultDTO<LibroDTO>> SearchByAuthorAsync(string author, int page = 1, int pageSize = 10);
    }
}