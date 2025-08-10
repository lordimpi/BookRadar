using BookRadar.DataAccess.Repositories.Historial;

namespace BookRadar.DataAccess.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IHistorialRepository HistorialRepository { get; }

        Task<int> SaveChangesAsync();

        Task BeginTransactionAsync();

        Task CommitAsync();

        Task RollbackAsync();
    }
}