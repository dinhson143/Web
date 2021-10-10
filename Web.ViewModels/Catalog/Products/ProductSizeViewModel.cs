using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.ViewModels.Catalog.Products
{
    public class ProductSizeViewModel
    {
        public string Size { get; set; }
        public int SizeId { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }
}