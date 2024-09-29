using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Api.Dtos.BasketDto;
using Talabat.Api.Errors;
using Talabat.Core.Entities;
using Talabat.Repository.Data.GenericRepository.RepositoriesContract;

namespace Talabat.Api.Controllers
{

    public class BasketController : BaseApiController
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _map;

        public BasketController(IBasketRepository basketRepository, IMapper map) { 
            _basketRepository = basketRepository;
            _map = map;
        }
        [HttpGet]
        public async Task<ActionResult> GetBasket(string id)
        {
            var basket= await _basketRepository.GetBasketAsync(id);
            return Ok(basket??new Core.Entities.CustomerBasket(id));
        }
        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDto customerBasket)
        {
            var resmap = _map.Map<CustomerBasket>(customerBasket);
            var res=  await _basketRepository.UpdateBaseket(resmap);
            if(res is null) { return BadRequest(new ApiResponse(400)); };
            return Ok(res);
        }
        [HttpDelete]
        public async Task DeleteBasket(string id)
        {
            await _basketRepository.DeleteBasket(id);
        }
    }
}
