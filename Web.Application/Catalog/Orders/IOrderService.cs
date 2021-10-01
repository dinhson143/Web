using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.ViewModels.Catalog.Common;
using Web.ViewModels.Catalog.Sales;

namespace Web.Application.Catalog.Orders
{
    public interface IOrderService
    {
        public Task<ResultApi<string>> CreateOrder(CheckoutRequest request);
    }
}