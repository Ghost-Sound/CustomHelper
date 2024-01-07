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
        Task AddASync(TEntity entity);
        void Add(TEntity entity);
        Task<bool> DeleteAsync(Ulid id);
        bool Delete(Ulid id);
        Task<List<TEntity>> GetAllAsync(int pageNumber, int pageSize, string attribute = "", string order = "asc");
        List<TEntity> GetAll(int pageNumber, int pageSize, string attribute = "", string order = "asc");
        Task UpdateAsync(Ulid id, TEntity entitydTO);
        void Update(Ulid id, TEntity entitydTO);
        Task<TEntity> GetByIdAsync(Ulid id);
        TEntity GetById(Ulid id);
    }
}
