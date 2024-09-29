using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Api.Dtos.ProductDto;
using Talabat.Api.Errors;
using Talabat.Api.Helpers;
using Talabat.Core.Entities;
using Talabat.Repository.Data.GenericRepository.RepositoriesContract;
using Talabat.Repository.Data.Repositories;
using Talabat.Repository.Data.Specification;
using Talabat.Repository.Data.Specification.ProductSpecifications;

namespace Talabat.Api.Controllers
{

    public class ProductsController : BaseApiController
    {
        private readonly IProductService productService;
        private readonly IMapper _map;
        public ProductsController(IProductService productService, IMapper map)         
        {
            this.productService = productService;
            _map = map;
           
        }
        [CacheAttribute(200)]
        [HttpGet]
      //  [Authorize]
        public async Task<ActionResult<PaginationResponse<ProductToReturnDto>>> GetProducts([FromQuery]ProductSpecParam productSpecParam)
        {        
            var res = await productService.GetProductsAsync(productSpecParam);
            if (res == null) return NotFound(new ApiResponse(404));
            var data = _map.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(res);        
            var count = await productService.GetCounttAsync(productSpecParam);
            return Ok(new PaginationResponse<ProductToReturnDto>(productSpecParam.PageSize,productSpecParam.PageIndex,count, data));
        }

        [ProducesResponseType(typeof(ProductToReturnDto),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProductById(int id)
        {       
            var res = await productService.GetProductAsync(id);
            if (res == null) return NotFound(new ApiResponse(404));
            return Ok(_map.Map<Product, ProductToReturnDto>(res));
        }
        [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            var res = await productService.GetBrandsAsync();
            return Ok(res);
        }
        [HttpGet("categories")]
        public async Task<ActionResult<ProductCategory>>  GetCategories()
        {
            var res= await productService.GetcategoriesAsync();
            return Ok(res);
        }
    }
}
