using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Order_Aggregate;

namespace Talabat.Repository.Data.Config
{
    public class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(x => x.ShippingAddress, x => x.WithOwner());
            builder.Property(o => o.OrderStatusStatus).HasConversion(o => o.ToString(), o =>(OrderStatus)Enum.Parse(typeof(OrderStatus), o));
            builder.HasOne(x => x.DeliveryMethod).WithMany().OnDelete(DeleteBehavior.SetNull);
        }
    }
}
