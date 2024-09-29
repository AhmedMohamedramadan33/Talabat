using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Repository.Data.GenericRepository.RepositoriesContract;

namespace Talabat.Repository.Data.UnitOfWork
{
    public interface IUnitOfWork:IAsyncDisposable 
    {
      public  IgenericRepositories<TEntity> Repository<TEntity>() where TEntity : BaseEntity;
      public  Task<int> Completeasync();
    }
}
