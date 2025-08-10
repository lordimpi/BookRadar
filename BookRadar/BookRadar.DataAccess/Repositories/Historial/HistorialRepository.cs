using BookRadar.Common.Entities;
using BookRadar.DataAccess.Data;
using Microsoft.EntityFrameworkCore;

namespace BookRadar.DataAccess.Repositories.Historial
{
    public class HistorialRepository : IHistorialRepository
    {
        private readonly AppDbContext _dbContext;

        public HistorialRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddRangeAsync(IEnumerable<HistorialBusqueda> items)
        {
            await _dbContext.HistorialBusquedas.AddRangeAsync(items);
        }

        public Task<bool> ExistsSimilarRecentlyAsync(HistorialBusqueda item, TimeSpan window)
        {
            var threshold = DateTime.UtcNow.Add(-window);
            return _dbContext.HistorialBusquedas.AnyAsync(x =>
                x.Autor == item.Autor &&
                x.Titulo == item.Titulo &&
                x.AnioPublicacion == item.AnioPublicacion &&
                x.Editorial == item.Editorial &&
                x.FechaConsulta >= threshold);
        }

        public async Task<IEnumerable<string>> GetRecentTitlesByAuthorAsync(string author, DateTime limiteTiempo)
        {
            return await _dbContext.HistorialBusquedas
                .Where(h => h.Autor == author && h.FechaConsulta >= limiteTiempo)
                .Select(h => h.Titulo)
                .ToListAsync();
        }

    }
}