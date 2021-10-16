using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Data.Enums;

namespace Web.ViewModels.Catalog.Orders
{
    public class OrderDetailViewModel
    {
        public int OrderID { get; set; }
        public int SizeID { get; set; }
        public int ProductID { get; set; }
        public string Image { get; set; }
        public string ProductName { get; set; }
        public string SizeName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}