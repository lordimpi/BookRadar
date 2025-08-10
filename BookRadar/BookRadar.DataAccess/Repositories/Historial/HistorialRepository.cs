using BookRadar.Common.DTOs;
using BookRadar.Common.Entities;
using BookRadar.DataAccess.Data;
using BookRadar.DataAccess.Utilities.ADO;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BookRadar.DataAccess.Repositories.Historial
{
    public class HistorialRepository : IHistorialRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly ISqlExecutor _sql;

        public HistorialRepository(AppDbContext dbContext, ISqlExecutor sql)
        {
            _dbContext = dbContext;
            _sql = sql;
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

        public async Task<(List<HistorialBusqueda> Items, int TotalRows)> ObtenerHistorialAsync(
        int pageNumber, int pageSize)
        {
            var p = new[]
            {
            SqlParam.In("PageNumber", SqlDbType.Int, pageNumber),
            SqlParam.In("PageSize", SqlDbType.Int, pageSize),
            SqlParam.Out("TotalRows", SqlDbType.Int)
        };

            var items = await _sql.QueryAsync(
                "ListarHistorialBusquedas",
                CommandType.StoredProcedure,
                DataReaderMapper.MapToList<HistorialBusqueda>,
                parameters: p
            );

            var totalRows = DbValue.FromDb<int>(p.First(x => x.ParameterName == "@TotalRows").Value);
            return (items, totalRows);
        }
    }
}