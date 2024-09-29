using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Order_Aggregate
{
   public class Order:BaseEntity
    {
        public Order(string buyerEmail, Address shippingAddress, DeliveryMethod deliveryMethod, ICollection<OrderItem> items, decimal subtotal,string paymentintentid)
        {
            BuyerEmail = buyerEmail;
            ShippingAddress = shippingAddress;
            DeliveryMethod = deliveryMethod;
            Items = items;
            Subtotal = subtotal;
           PaymentIntendId= paymentintentid;
        }
        public Order()
        {

        }
        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; }=DateTimeOffset.Now;
        public OrderStatus OrderStatusStatus { get; set; } = OrderStatus.Pending;
        public virtual Address ShippingAddress { get; set; }

        public virtual DeliveryMethod DeliveryMethod { get; set; }
        public virtual ICollection<OrderItem> Items { get; set; }=new HashSet<OrderItem>();
        public decimal Subtotal { get; set; }
        public decimal GetTotal() => Subtotal + DeliveryMethod.Cost;
        public string PaymentIntendId { get; set; } 

    }
}
