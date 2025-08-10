using BookRadar.Common.Entities;

namespace BookRadar.DataAccess.Repositories.Historial
{
    public interface IHistorialRepository
    {
        Task AddRangeAsync(IEnumerable<HistorialBusqueda> items);
        Task<bool> ExistsSimilarRecentlyAsync(HistorialBusqueda item, TimeSpan window);
        Task<IEnumerable<string>> GetRecentTitlesByAuthorAsync(string author, DateTime limiteTiempo);
        Task<(List<HistorialBusqueda> Items, int TotalRows)> ObtenerHistorialAsync(int pageNumber, int pageSize);
    }
}