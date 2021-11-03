using System;
using System.Collections.Generic;
using Web.ViewModels.Catalog.Orders;
using Web.ViewModels.Catalog.Products;

namespace Web.Models
{
    public class ProductSavestModel
    {
        public List<ProductViewModel> listPro { get; set; }
        public List<OrderViewModel> listOd { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}