using System;
using System.Collections.Generic;
using Web.ViewModels.Catalog.Comments;
using Web.ViewModels.Catalog.Orders;
using Web.ViewModels.Catalog.Products;
using Web.ViewModels.Catalog.Users;

namespace Web.Models
{
    public class ProductSavestModel
    {
        public List<ProductViewModel> listPro { get; set; }
        public List<OrderViewModel> listOd { get; set; }
        public List<OrderViewModel> listOdIPro { get; set; }
        public List<CommentViewModel> listCM { get; set; }
        public List<int> listUS { get; set; }
        public List<decimal> listDT { get; set; }
        public List<int> listSLOD { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}