using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Order_Aggregate;

namespace Talabat.Repository.Data.Specification.OrdersSpecifications
{
    public class OrderSpecifications:BaseSpecification<Order>
    {
        public OrderSpecifications(string Email):base(x=>x.BuyerEmail==Email)
        {
            Includes.Add(x => x.DeliveryMethod);
            Includes.Add(x => x.Items);
            OrderByDesc=x=>x.OrderDate;
        }
        public OrderSpecifications(int orderid,string Email) : base(x=>x.Id==orderid && x.BuyerEmail == Email)
        {
            Includes.Add(x => x.DeliveryMethod);
            Includes.Add(x => x.Items);
        }
    }
}
