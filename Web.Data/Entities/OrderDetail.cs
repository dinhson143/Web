﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Data.Entities
{
    public class OrderDetail
    {
        public int OrderId { set; get; }
        public int ProductId { set; get; }
        public int SizeId { set; get; }
        public int Quantity { set; get; }
        public decimal Price { set; get; }

        public Order Order { set; get; }
        public Product Product { set; get; }
    }
}