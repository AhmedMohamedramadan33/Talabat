using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Repository.Data.GenericRepository.RepositoriesContract;
using Talabat.Repository.Data.GenericRepository.ServicesContract;

namespace Talabat.Repository.Data.UnitOfWork
{
    public class UnitOfWorkIm : IUnitOfWork
    {
        private readonly StoreContext _storeContext;
        private Hashtable _repositories;
        public UnitOfWorkIm(StoreContext storeContext) {
            _storeContext = storeContext;
            _repositories= new Hashtable();
        }
        public async Task<int> Completeasync()
        {
            return await _storeContext.SaveChangesAsync();
        }
        public async ValueTask DisposeAsync()
        {
             await _storeContext.DisposeAsync();
        }

        public IgenericRepositories<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            var key= typeof(TEntity).Name;
            if(!_repositories.ContainsKey(key))
            {
                var ob= new GenericRepositories<TEntity>(_storeContext) ;
                _repositories.Add(key, ob);
            }
            return _repositories[key] as IgenericRepositories<TEntity>;
        }
    }
}
