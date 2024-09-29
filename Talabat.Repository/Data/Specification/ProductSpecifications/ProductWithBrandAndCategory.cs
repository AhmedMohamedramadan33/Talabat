using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Repository.Data.Specification.ProductSpecifications
{
    public class ProductWithBrandAndCategory : BaseSpecification<Product>
    {
        public ProductWithBrandAndCategory(ProductSpecParam productSpecParam) : base(P =>
        (string.IsNullOrEmpty(productSpecParam.Name) || P.Name.ToLower().Contains(productSpecParam.Name)) &&
        (!productSpecParam.Brandid.HasValue || P.BrandId == productSpecParam.Brandid.Value) &&
        (!productSpecParam.CategoryId.HasValue || P.CategoryId == productSpecParam.CategoryId.Value)
        )
        {
            ApplyInclude();
            if (!string.IsNullOrEmpty(productSpecParam.sort))
            {
                switch (productSpecParam.sort.ToLower())
                {
                    case "priceasc":
                        OrderBy = p => p.Price;
                        break;
                    case "pricedesc":
                        OrderByDesc = p => p.Price;
                        break;
                    case "namedesc":
                        OrderBy = p => p.Name;
                        break;
                    default:
                        OrderByDesc = p => p.Name;
                        break;
                }
            }
            else
            {
                OrderBy = p => p.Id;

            }
            ApplyPagination((productSpecParam.PageIndex - 1) * productSpecParam.PageSize, productSpecParam.PageSize);

        }

        public ProductWithBrandAndCategory(int id) : base(p => p.Id == id)
        {
            ApplyInclude();
        }


        private void ApplyInclude()
        {
            Includes.Add(x => x.Brand);
            Includes.Add(x => x.Category);
        }
    }
}
