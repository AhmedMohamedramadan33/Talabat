using AutoMapper;
using Talabat.Api.Dtos.BasketDto;
using Talabat.Api.Dtos.OrderDtos;
using Talabat.Api.Dtos.ProductDto;
using Talabat.Api.Helpers;
using Talabat.Core.Entities;
using Talabat.Core.Order_Aggregate;

namespace Talabat.Api.AutoMapper
{
    public class MappingProfiles:Profile
    {
        public MappingProfiles() {
            CreateMap<Product, ProductToReturnDto>().
               ForMember(x => x.Brand, y => y.MapFrom(y => y.Brand.Name)).
               ForMember(x => x.Category, y => y.MapFrom(y => y.Category.Name)).
               ForMember(x => x.PictureUrl, y => y.MapFrom<ProductPictureUrl>());

            CreateMap<BasketItemDto, BasketItem>();

            CreateMap<CustomerBasketDto,CustomerBasket>();

            CreateMap<AddressDto, Address>();
            CreateMap<Talabat.Core.Entities.Identity.Address, AddressDto>().ReverseMap();

            CreateMap<Order, OrderToReturnDto>().
               ForMember(x => x.DeliveryMethod, x => x.MapFrom(x => x.DeliveryMethod.ShortName)).
               ForMember(x => x.DeliveryMethodCost, x => x.MapFrom(x => x.DeliveryMethod.Cost));

            CreateMap<OrderItem, OrderItemDto>().
               ForMember(x => x.ProductName, x => x.MapFrom(x => x.product.ProductName)).
               ForMember(x => x.ProductId, x => x.MapFrom(x => x.product.ProductId)).
               ForMember(x => x.ProductUrl, x => x.MapFrom(x => x.product.ProductUrl)).
               ForMember(x => x.ProductUrl, x => x.MapFrom<OrderItemPictureUrlResolver>());
;




            






        }
    }
}
