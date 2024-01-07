using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomHelper.Service.Interfaces
{
    public interface IService<TEntity>
        where TEntity : class
    {
        Task AddASync(TEntity entity, CancellationToken cancellationToken = default);
        void Add(TEntity entity);
        Task<bool> DeleteAsync(Ulid id, CancellationToken cancellationToken = default);
        bool Delete(Ulid id);
        Task<List<TEntity>> GetAllAsync(int pageNumber, int pageSize, string attribute = "", string order = "asc", CancellationToken cancellationToken = default);
        List<TEntity> GetAll(int pageNumber, int pageSize, string attribute = "", string order = "asc");
        Task UpdateAsync(Ulid id, TEntity entitydTO, CancellationToken cancellationToken = default);
        void Update(Ulid id, TEntity entitydTO);
        Task<TEntity> GetByIdAsync(Ulid id, CancellationToken cancellationToken = default);
        TEntity GetById(Ulid id);
    }
}
