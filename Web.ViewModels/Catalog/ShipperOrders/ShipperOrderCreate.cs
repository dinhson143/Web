using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.ViewModels.Catalog.ShipperOrders
{
    public class ShipperOrderCreate
    {
        public Guid ShipperId { set; get; }
        public int OrderID { set; get; }
    }
}