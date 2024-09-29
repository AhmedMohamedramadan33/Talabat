using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Repository.Data;
using Talabat.Repository.Data.GenericRepository.RepositoriesContract;
using Talabat.Repository.Data.Specification;

namespace Talabat.Repository.Data.GenericRepository.ServicesContract
{
    public class GenericRepositories<T> : IgenericRepositories<T> where T : BaseEntity
    {
        private readonly StoreContext _context;
        public GenericRepositories(StoreContext context)
        {
            _context = context;
        }

        public async Task AddAsync(T entity)
        {
            await  _context.Set<T>().AddAsync(entity);
         }

        public void Delete(T entity)
        {
            _context.Remove(entity);
        }

        public async Task<IReadOnlyList<T>> GetAllAync()
        {
            //if (typeof(T) == typeof(Product))
            //{
            //    return (IEnumerable<T>)await _context.Set<Product>().Include(x => x.Brand).Include(x => x.Category).ToListAsync();
            //}

            return await _context.Set<T>().ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllWithSpecAync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }

        public async Task<T?> GetAync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public Task<int> GetCountAync(ISpecification<T> spec)
        {
            return ApplySpecification(spec).CountAsync();
        }

        public async Task<T?> GetWithSpecAync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }

        public void Update(T entity)
        {
            _context.Update(entity);
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_context.Set<T>(), spec);
        }
    }
}
