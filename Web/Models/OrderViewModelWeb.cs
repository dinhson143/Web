using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.ViewModels.Catalog.Orders;

namespace Web.Models
{
    public class OrderViewModelWeb
    {
        public List<OrderViewModel> listOrder { get; set; }
        public List<OrderViewModel> listOrderHistory { get; set; }
    }
}