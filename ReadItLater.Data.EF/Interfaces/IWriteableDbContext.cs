using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Data.EFCore.Interfaces
{
    public interface IWriteableDbContext<TEntity>
        where TEntity : class, IEntity
    {
        Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
        Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
        Task DeleteAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

        Task SaveAsync(CancellationToken cancellationToken = default);
    }
}
