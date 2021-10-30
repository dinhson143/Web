using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Models
{
    public class CartItemViewModel
    {
        public int ProductId { get; set; }
        public int SizeId { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public string NameSize { get; set; }
        public decimal Price { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public int? DiscountPercent { get; set; }
        public decimal? DiscountAmount { get; set; }
    }
}