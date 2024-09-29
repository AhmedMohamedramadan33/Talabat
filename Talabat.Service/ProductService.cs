using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Repository.Data.Repositories;
using Talabat.Repository.Data.Specification;
using Talabat.Repository.Data.Specification.ProductSpecifications;
using Talabat.Repository.Data.UnitOfWork;

namespace Talabat.Service
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork unitOfWork;

        public ProductService(IUnitOfWork unitOfWork) {
            this.unitOfWork = unitOfWork;
        }
        public async Task<IReadOnlyList<ProductBrand>> GetBrandsAsync()
        {
            return await unitOfWork.Repository<ProductBrand>().GetAllAync();
        }

        public async Task<IReadOnlyList<ProductCategory>> GetcategoriesAsync()
        {
            return await unitOfWork.Repository<ProductCategory>().GetAllAync();
        }

        public async Task<int> GetCounttAsync(ProductSpecParam productSpecParam)
        {
            var countspec = new ProductWithfilterationForCountSpec(productSpecParam);
            var count = await unitOfWork.Repository<Product>().GetCountAync(countspec);
            return count; 
        }

        public async Task<Product> GetProductAsync(int id)
        {
            var spec = new ProductWithBrandAndCategory(id);
            var res = await unitOfWork.Repository<Product>().GetWithSpecAync(spec);
            return res;
        }

        public async Task<IReadOnlyList<Product>> GetProductsAsync(ProductSpecParam productSpecParam)
        {
            var spec = new ProductWithBrandAndCategory(productSpecParam);
            var res = await unitOfWork.Repository<Product>().GetAllWithSpecAync(spec);
            return res;
        }
    }
}
