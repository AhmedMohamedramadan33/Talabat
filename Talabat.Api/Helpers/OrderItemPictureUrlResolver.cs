using AutoMapper;
using Talabat.Api.Dtos.OrderDtos;
using Talabat.Api.Dtos.ProductDto;
using Talabat.Core.Entities;
using Talabat.Core.Order_Aggregate;

namespace Talabat.Api.Helpers
{
    public class OrderItemPictureUrlResolver : IValueResolver<OrderItem, OrderItemDto, string>
    {
        private readonly IConfiguration _configuration;
        public OrderItemPictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
      
        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.product.ProductUrl))
            {
                return $"{_configuration["ApiBaseUrl"]}/{source.product.ProductUrl}";
            }
            return string.Empty;
        }
    }
}
