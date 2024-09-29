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
using Address = Talabat.Core.Order_Aggregate.Address;
using Product = Talabat.Core.Entities.Product;

namespace Talabat.Service
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IpaymentService paymentService;

        public OrderService(IBasketRepository basketRepository,IUnitOfWork unitOfWork,IpaymentService paymentService)
        {
           _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
            this.paymentService = paymentService;
        }
        public async Task<Order?> CreateOrder(string BuyerEmail, string basketId, int deliveryMethodId, Address shippingAddress)
        {
            var basket = await _basketRepository.GetBasketAsync(basketId);
            var orderitems=new List<OrderItem>();
            if(basket?.Items?.Count> 0)
            {
                var productrepo = _unitOfWork.Repository<Product>();

                foreach (var item in basket.Items)
                {
                var product=     await productrepo.GetAync(item.Id);
                    var productorderitem = new ProductItemOrder(item.Id,product.Name,product.PictureUrl);
                    var orderitem = new OrderItem(productorderitem, product.Price,item.Quantity);
                    orderitems.Add(orderitem);                   
                }

            }
            var subtotal = orderitems.Sum(x => x.Price*x.Quantity);
            var deliverymethod=await _unitOfWork.Repository<DeliveryMethod>().GetAync(deliveryMethodId);
            var checkorder = _unitOfWork.Repository<Order>();
            var orderintentspec = new OrderWithPaymentIntentSpecification(basket.PaymentIntentId);
            var orderrepo =await checkorder.GetWithSpecAync(orderintentspec);
            if(orderintentspec!=null)
            {
                checkorder.Delete(orderrepo);
                paymentService.CreateOrUpdatePaymentIntent(basketId);
            }


            var order = new Order(BuyerEmail,shippingAddress,deliverymethod,orderitems,subtotal,basket.PaymentIntentId);
           await _unitOfWork.Repository<Order>().AddAsync(order);
         var res= await  _unitOfWork.Completeasync();
            if (res <= 0) return null; 
            return order;


        }

        public Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodForUserAsync()
        {
            var delivery= _unitOfWork.Repository<DeliveryMethod>();
            var res=delivery.GetAllAync();
            return res;
        }

        public Task<Order?> GetOrderByIdForUserAsync(int orderId, string BuyerEmail)
        {
            var order = _unitOfWork.Repository<Order>();
            var orderspec = new OrderSpecifications(orderId,BuyerEmail);
            var res = order.GetWithSpecAync(orderspec);
            return res;
        }

        public Task<IReadOnlyList<Order>> GetOrderForUserAsync(string BuyerEmail)
        {
            var order= _unitOfWork.Repository<Order>();
            var orderspec = new OrderSpecifications(BuyerEmail);
            var res = order.GetAllWithSpecAync(orderspec);
            return res;
        }
    }
}
