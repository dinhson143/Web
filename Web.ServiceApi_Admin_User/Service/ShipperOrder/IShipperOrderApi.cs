using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.ViewModels.Catalog.Common;
using Web.ViewModels.Catalog.Orders;
using Web.ViewModels.Catalog.ShipperOrders;

namespace Web.ServiceApi_Admin_User.Service.ShipperOrder
{
    public interface IShipperOrderApi
    {
        public Task<ResultApi<string>> CreateShipperOrder(ShipperOrderCreate request);

        public Task<List<OrderViewModel>> GetAll(Guid shipperId);
    }
}