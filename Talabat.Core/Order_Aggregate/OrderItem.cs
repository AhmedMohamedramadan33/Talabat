﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Order_Aggregate
{
   public class OrderItem:BaseEntity
    {
        public OrderItem(ProductItemOrder product, decimal price, int quantity)
        {
            this.product = product;
            Price = price;
            Quantity = quantity;
        }
        public OrderItem()
        {

        }
        public virtual ProductItemOrder product { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}