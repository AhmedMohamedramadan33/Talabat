using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.Api.Dtos.OrderDtos;
using Talabat.Api.Errors;
using Talabat.Core.Order_Aggregate;
using Talabat.Repository.Data.Repositories;
using Talabat.Repository.Data.Specification.OrdersSpecifications;

namespace Talabat.Api.Controllers
{
    [Authorize]
    public class OrdersController : BaseApiController
    {
        private readonly IOrderService orderService;
        private readonly IMapper mapper;

        public OrdersController(IOrderService orderService,IMapper mapper)
        {
            this.orderService = orderService;
            this.mapper = mapper;
        }
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(OrderToReturnDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<OrderToReturnDto>>  CreateOrder(OrderDto orderDto)
        {
            var email=User.FindFirstValue(ClaimTypes.Email);
            var address = mapper.Map<AddressDto, Address>(orderDto.ShippingAddress);
            var order = await orderService.CreateOrder(email,orderDto.BasketId,orderDto.DeliveryMethodId,address);
            if(order == null) { return BadRequest(new ApiResponse(400));  };
            return Ok(mapper.Map<Order,OrderToReturnDto>(order));
        }

        [HttpGet]     
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>>  GetOrdersForUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var order = await orderService.GetOrderForUserAsync(email);
            return Ok(mapper.Map<IReadOnlyList<Order>,IReadOnlyList<OrderToReturnDto>>(order));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(IReadOnlyList<OrderToReturnDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrderForUser(int id)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var order = await orderService.GetOrderByIdForUserAsync(id,email);
            if (order is null) return NotFound(new ApiResponse(404));
            return Ok(mapper.Map<Order, OrderToReturnDto>(order));
        }
        [HttpGet("deliveryMethods")]
        
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        {
            var res = await orderService.GetDeliveryMethodForUserAsync();
            return Ok(res);
        }
    }
}
