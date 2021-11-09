using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.ViewModels.Catalog.Common;
using Web.ViewModels.Catalog.Orders;
using Web.ViewModels.Catalog.Sales;

namespace Web.Application.Catalog.Orders
{
    public interface IOrderService
    {
        public Task<ResultApi<string>> CreateOrder(CheckoutRequest request);

        public Task<List<OrderViewModel>> GetOrderUser(Guid userId, string languageID);

        public Task<List<OrderViewModel>> GetOrderUserHistory(Guid userId, string languageID);

        public Task<List<OrderViewModel>> GetallOrder(string languageID);

        public Task<OrderViewModel> GetOrderByID(int orderId, string languageID);

        public Task<List<OrderViewModel>> GetallOrderSuccess(string languageID);

        public Task<List<OrderViewModel>> GetallOrderConfirm(string languageID);

        public Task<List<OrderViewModel>> GetallOrderInProgress(string languageID);

        public Task<int> CancelOrder(Guid userId, int orderId);

        public Task<int> ConfirmOrder(int orderId);

        public Task<int> SuccessOrder(int orderId);
    }
}