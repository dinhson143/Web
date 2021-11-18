using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.ViewModels.Catalog.ShipperOrders
{
    public class ShipperOrderViewModel
    {
        public int OrderID { get; set; }
        public DateTime DateSuccess { get; set; }
        public string Status { get; set; }
        public string IdUser { get; set; }
    }
}