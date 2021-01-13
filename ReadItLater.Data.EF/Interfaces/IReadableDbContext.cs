using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Data.EFCore.Interfaces
{
    public interface IReadableDbContext<TEntity>
        where TEntity : class, IEntity
    {
        Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

        Task<TResult> SingleOrDefaultAsync<TResult>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> projection, CancellationToken cancellationToken = default);

        Task<IEnumerable<TEntity>> SelectAllAsync(CancellationToken cancellationToken = default);

        Task<IEnumerable<TEntity>> SelectAsync(
            Expression<Func<TEntity, bool>> predicate,
            int offset = 0,
            int limit = 10,
            string ordering = "+Id",
            CancellationToken cancellationToken = default
        );

        Task<IEnumerable<TResult>> SelectAsync<TResult>(
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, TResult>> projection,
            int offset = 0,
            int limit = 10,
            string ordering = "+Id",
            CancellationToken cancellationToken = default
        );
    }
}
