using CustomHelper.EFcore.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomHelper.EFcore.Interfaces.Factory
{
    public interface IRepositoryFactory
    {
        IRepository<TEntity> Create<TEntity>(DbContext context) where TEntity : class;

        void RegisterAllRepositories(DbContext context);
    }
}
