using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Repository.Data.Specification.ProductSpecifications
{
    public class ProductWithfilterationForCountSpec : BaseSpecification<Product>
    {
        public ProductWithfilterationForCountSpec(ProductSpecParam productSpecParam) : base(P =>
        (string.IsNullOrEmpty(productSpecParam.Name) || P.Name.ToLower().Contains(productSpecParam.Name)) &&
        (!productSpecParam.Brandid.HasValue || P.BrandId == productSpecParam.Brandid.Value) &&
        (!productSpecParam.CategoryId.HasValue || P.CategoryId == productSpecParam.CategoryId.Value))
        {



        }
    }
}
