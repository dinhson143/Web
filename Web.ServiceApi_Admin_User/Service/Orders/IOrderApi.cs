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
        public Task<ResultApi<List<OrderViewModel>>> GetAllOrderUser(Guid userId, string languageID, string BearerToken);

        public Task<ResultApi<List<OrderViewModel>>> GetOrderUserHistory(Guid userId, string languageID, string BearerToken);

        public Task<ResultApi<List<OrderViewModel>>> GetAllOrder(string languageID, string BearerToken);

        public Task<ResultApi<OrderViewModel>> GetOrderByID(int orderID, string languageID, string BearerToken);

        public Task<ResultApi<List<OrderViewModel>>> GetallOrderSuccess(string languageID, string BearerToken);

        public Task<ResultApi<List<OrderViewModel>>> GetallOrderConfirm(string languageID, string BearerToken);

        public Task<ResultApi<List<OrderViewModel>>> GetallOrderInProgress(string languageID, string BearerToken);

        public Task<bool> CancelOrder(Guid userId, int orderId, string BearerToken);

        public Task<bool> ConfirmOrder(int orderId, string BearerToken);

        public Task<bool> SuccessOrder(int orderId, string BearerToken);
    }
}