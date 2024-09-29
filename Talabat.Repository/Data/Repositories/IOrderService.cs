using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Order_Aggregate;

namespace Talabat.Repository.Data.Repositories
{
    public interface IOrderService
    {
        public Task<Order> CreateOrder(string BuyerEmail,string basketId,int deliveryMethodId,Address shippingAddress);
        public Task<IReadOnlyList<Order>> GetOrderForUserAsync(string BuyerEmail);
        Task<Order?> GetOrderByIdForUserAsync(int orderId,string BuyerEmail);
        public Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodForUserAsync();
    }
}
