using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Core.Data.EFCore.Interfaces;

namespace Core.Data.EFCore.Utils
{
    public partial class EfDbContext<TEntity> : IEfDbContext<TEntity>
        where TEntity : class, IEntity
    {
        public async Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default) =>
            await this.dbSet.AsNoTracking().FirstOrDefaultAsync(predicate, cancellationToken);

        public async Task<TResult> SingleOrDefaultAsync<TResult>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> projection, CancellationToken cancellationToken = default) =>
            await this.dbSet.AsNoTracking().Where(predicate).Select(projection).FirstOrDefaultAsync(cancellationToken);

        public async Task<IEnumerable<TEntity>> SelectAllAsync(CancellationToken cancellationToken = default) =>
            await this.dbSet
                .AsNoTracking()
                .ToListAsync(cancellationToken);

        public async Task<IEnumerable<TEntity>> SelectAsync(
            Expression<Func<TEntity, bool>> predicate,
            int offset = 0,
            int limit = 10,
            string ordering = "+Id",
            CancellationToken cancellationToken = default) =>
            await this.dbSet
                .AsNoTracking()
                .Where(predicate)
                .Skip(offset)
                .Take(limit)
                //ToDo: add ordering
                .ToListAsync(cancellationToken);

        public async Task<IEnumerable<TResult>> SelectAsync<TResult>(
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, TResult>> projection,
            int offset = 0,
            int limit = 10,
            string ordering = "+Id",
            CancellationToken cancellationToken = default) =>
            await this.dbSet
                .AsNoTracking()
                .Where(predicate)
                .Skip(offset)
                .Take(limit)
                //ToDo: add ordering
                .Select(projection)
                .ToListAsync(cancellationToken);

        public Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            this.dbSet.Add(entity);

            return Task.FromResult(entity);
        }

        public Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default) =>
            Task.FromResult(this.dbSet.Update(entity));

        public async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            TEntity entity = await this.dbSet.SingleOrDefaultAsync(predicate, cancellationToken);

            if (entity is null)
                return;
            
            this.dbSet.Remove(entity);
        }

        public async Task SaveAsync(CancellationToken cancellationToken = default) =>
            await this.SaveChangesAsync(cancellationToken);
    }
}
