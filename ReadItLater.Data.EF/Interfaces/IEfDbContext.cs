using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Core.Data.EFCore.Interfaces
{
    public interface IEfDbContext
    {
        DatabaseFacade Database { get; }
    }

    public interface IEfDbContext<TEntity> : IWriteableDbContext<TEntity>, IReadableDbContext<TEntity>
        where TEntity : class, IEntity
    {

    }
}
