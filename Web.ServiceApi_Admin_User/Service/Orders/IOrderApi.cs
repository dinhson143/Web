using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.ViewModels.Catalog.Common;
using Web.ViewModels.Catalog.Orders;
using Web.ViewModels.Catalog.Sales;

namespace Web.ServiceApi_Admin_User.Service.Orders
{
    public interface IOrderApi
    {
        public Task<ResultApi<string>> CreateOrder(CheckoutRequest request, string BearerToken);

        public Task<ResultApi<List<OrderViewModel>>> GetOrderUser(Guid userId, string languageID, string BearerToken);
    }
}