using CustomHelper.EFcore.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CustomHelper.EFcore.Class.Repository
{
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        private readonly DbContext _dbContext;

        public Repository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual void Add(TEntity entity)
        {
            _dbContext.Set<TEntity>().Add(entity);
        }

        public virtual async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await _dbContext.Set<TEntity>().AddAsync(entity, cancellationToken);
        }

        public virtual void Delete(TEntity entity)
        {
            _dbContext.Set<TEntity>().Remove(entity);
        }

        public virtual bool DeleteRange(IEnumerable<TEntity> entities)
        {
            try
            {
                _dbContext.Set<TEntity>().RemoveRange(entities);

                return true;
            }
            catch (System.Exception ex)
            {
                return false;
            }
        }

        public virtual async Task<bool> DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return false;
            }

            try
            {
                _dbContext.Set<TEntity>().RemoveRange(entities);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return true;
            }
            catch (System.Exception ex)
            {
                return false;
            }
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            return _dbContext.Set<TEntity>();
        }

        public virtual async Task<IQueryable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                await Task.CompletedTask;
            }

            var result = _dbContext.Set<TEntity>();

            return result;
        }

        public virtual TEntity GetEntity(Ulid id)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<TEntity> GetEntityAsync(Ulid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<TEntity>().FindAsync(id, cancellationToken);
        }

        public virtual IQueryable<TEntity> Take(int skipElements, int takeElements, (Expression<Func<TEntity, object>> expression, bool ascending) sortOrder)
        {
            var query = _dbContext.Set<TEntity>().AsNoTracking();

            if (sortOrder.ascending)
            {
                query = query.OrderBy(sortOrder.expression);
            }
            else
            {
                query = query.OrderByDescending(sortOrder.expression);
            }

            return query
                .Skip(skipElements)
                .Take(takeElements);
        }

        public virtual async Task<IQueryable<TEntity>> TakeAsync(int skipElements, int takeElements, (Expression<Func<TEntity, object>> expression, bool ascending) sortOrder, CancellationToken cancellationToken = default)
        {
            if(cancellationToken.IsCancellationRequested)
            {
                await Task.CompletedTask;
            }

            var query = _dbContext.Set<TEntity>().AsNoTracking();

            if (sortOrder.ascending)
            {
                query = query.OrderBy(sortOrder.expression);
            }
            else
            {
                query = query.OrderByDescending(sortOrder.expression);
            }

            return query
                .Skip(skipElements)
                .Take(takeElements);
        }

        public virtual void Update(TEntity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual void UpdateRange(IEnumerable<TEntity> entities)
        {
            _dbContext.Set<TEntity>().UpdateRange(entities);
        }

        public virtual async Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                await Task.CompletedTask;
            }

             _dbContext.Set<TEntity>().UpdateRange(entities);
        }
    }
}
