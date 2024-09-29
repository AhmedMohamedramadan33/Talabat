using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Repository.Data.Specification;

namespace Talabat.Repository.Data.GenericRepository.RepositoriesContract
{
    public interface IgenericRepositories<T> where T : BaseEntity
    {
        public Task<T?> GetAync(int id);
        public Task<IReadOnlyList<T>> GetAllAync();
        public Task<T?> GetWithSpecAync(ISpecification<T> spec);
        public Task<IReadOnlyList<T>> GetAllWithSpecAync(ISpecification<T> spec);
        public Task<int> GetCountAync(ISpecification<T> spec);
        public Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
