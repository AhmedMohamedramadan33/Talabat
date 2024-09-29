using Talabat.Core.Order_Aggregate;

namespace Talabat.Api.Dtos.OrderDtos
{
    public class OrderToReturnDto
    {
        public int Id { get; set; }
        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; }
        public string  Status { get; set; }
        public virtual Address ShippingAddress { get; set; }
        public  string DeliveryMethod { get; set; }
        public decimal DeliveryMethodCost { get; set; }

        public virtual ICollection<OrderItemDto> Items { get; set; } = new HashSet<OrderItemDto>();
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }
        public string PaymentIntendId { get; set; } = "";

    }

  
}
