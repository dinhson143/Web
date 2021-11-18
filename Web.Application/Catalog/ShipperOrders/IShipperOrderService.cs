using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.ViewModels.Catalog.Common;
using Web.ViewModels.Catalog.Orders;
using Web.ViewModels.Catalog.ShipperOrders;

namespace Web.Application.Catalog.ShipperOrder
{
    public interface IShipperOrderService
    {
        public Task<ResultApi<string>> CreateShipperOrder(ShipperOrderCreate request);

        public Task<List<OrderViewModel>> GetAll(Guid shipperId);
        public Task<List<OrderViewModel>> GetallOrderSPrequest(Guid ShipperID,string languageID);

        public Task<List<OrderViewModel>> GetAll_HistorySP(Guid shipperId);
        public Task<int> ConfirmOrderSP(int orderId, Guid ShipperID);
    }
}