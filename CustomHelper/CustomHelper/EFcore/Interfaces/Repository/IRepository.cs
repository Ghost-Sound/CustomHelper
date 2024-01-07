using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CustomHelper.EFcore.Interfaces.Repository
{
    public interface IRepository<TEntity>
        where TEntity : class
    {
        Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
        void Add(TEntity entity);
        void Delete(TEntity entity);

        Task<TEntity> GetEntityAsync(Ulid id, CancellationToken cancellationToken = default);
        TEntity GetEntity(Ulid id);

        void Update(TEntity entity);

        Task<IQueryable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);

        IQueryable<TEntity> GetAll();

        Task<IQueryable<TEntity>> TakeAsync(int skipElements, int takeElements, (Expression<Func<TEntity, object>> expression, bool ascending) sortOrder, CancellationToken cancellationToken = default);
        IQueryable<TEntity> Take(int skipElements, int takeElements, (Expression<Func<TEntity, object>> expression, bool ascending) sortOrder);

        void UpdateRange(IEnumerable<TEntity> entities);

        Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        bool DeleteRange(IEnumerable<TEntity> entities);

        Task<bool> DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
    }
}
