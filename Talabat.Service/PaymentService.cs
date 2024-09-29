using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Order_Aggregate;
using Talabat.Repository.Data.GenericRepository.RepositoriesContract;
using Talabat.Repository.Data.Repositories;
using Talabat.Repository.Data.Specification.OrdersSpecifications;
using Talabat.Repository.Data.UnitOfWork;
using Product = Talabat.Core.Entities.Product;

namespace Talabat.Service
{
    public class PaymentService : IpaymentService
    {
        private readonly IConfiguration configuration;
        private readonly IBasketRepository basketRepository;
        private readonly IUnitOfWork unitOfWork;

        public PaymentService(IConfiguration configuration,IBasketRepository basketRepository,IUnitOfWork unitOfWork)
        {
            this.configuration = configuration;
            this.basketRepository = basketRepository;
            this.unitOfWork = unitOfWork;
        }
        public async Task<CustomerBasket?>CreateOrUpdatePaymentIntent(string BasketId)
        {
            StripeConfiguration.ApiKey =configuration["StripeSettings:Secretkey"];
            var basket = await basketRepository.GetBasketAsync(BasketId);
            if(basket == null) { return null; }
            var shippingPrice = 0m;
            if (basket.DeliveryMethodId.HasValue)
            {
                var deliverymethod = await unitOfWork.Repository<DeliveryMethod>().GetAync(basket.DeliveryMethodId.Value);
                basket.ShippingPrice = deliverymethod.Cost;
                shippingPrice = deliverymethod.Cost;
            }
            if (basket?.Items.Count > 0)
            {
                foreach (var item in basket.Items)
                {
                    var product = await unitOfWork.Repository<Product>().GetAync(item.Id);
                    if (item.Price != product.Price)
                    {
                        item.Price = product.Price;
                    }
                }
            }
                PaymentIntent paymentIntent;
                PaymentIntentService paymentIntentService= new PaymentIntentService();
                if (string.IsNullOrEmpty(basket.PaymentIntentId))
                {
                    var options= new PaymentIntentCreateOptions()
                    {
                        Amount = (long)basket.Items.Sum(item => item.Price * 100 * item.Quantity)+(long)shippingPrice,
                        Currency = "usd",
                        PaymentMethodTypes = new List<string>() { "card" },
                    };
                    paymentIntent = await paymentIntentService.CreateAsync(options);
                    basket.PaymentIntentId = paymentIntent.Id;
                    basket.ClientSecret = paymentIntent.ClientSecret;
                }
                else
                {
                    var update = new PaymentIntentUpdateOptions()
                    {
                        Amount = (long)basket.Items.Sum(item => item.Price * 100 * item.Quantity) +(long)shippingPrice,
                    };
                    await paymentIntentService.UpdateAsync(basket.PaymentIntentId, update);
                }
            await basketRepository.UpdateBaseket(basket);
            return basket;
           
        }

        public async Task<Order> Updatepaymentintendtosuccedorfailed(string paymentintend, bool issucced)
        {
            var specorder = new OrderWithPaymentIntentSpecification(paymentintend);
            var order = await unitOfWork.Repository<Order>().GetWithSpecAync(specorder);
            if(issucced)
            {
                order.OrderStatusStatus = OrderStatus.PaymentRecieved;
            }
            else
            {
                order.OrderStatusStatus = OrderStatus.PaymentFailed;

            }
            unitOfWork.Repository<Order>().Update(order);
            await unitOfWork.Completeasync();
            return order;
        }
    }
}
