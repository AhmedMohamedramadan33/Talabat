using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Repository.Data.Specification;

namespace Talabat.Repository.Data.Repositories
{
   public interface IProductService
    {
        Task<IReadOnlyList<Product>> GetProductsAsync(ProductSpecParam productSpecParam);
        Task<Product> GetProductAsync(int id);
        Task<int> GetCounttAsync(ProductSpecParam productSpecParam);
        Task<IReadOnlyList<ProductBrand>> GetBrandsAsync();
        Task<IReadOnlyList<ProductCategory>> GetcategoriesAsync();

    }
}
