using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.ViewModels.Catalog.Common;
using Web.ViewModels.Catalog.Orders;
using Web.ViewModels.Catalog.Products;

namespace Web.Application.Catalog.ThongKes
{
    public interface IThongKeService
    {
        public Task<ResultApi<List<ProductViewModel>>> ProductLovest(string languageId);

        public Task<ResultApi<List<ProductViewModel>>> ProductSavest(string from, string to, string languageId);

        public Task<ResultApi<List<OrderViewModel>>> DoanhThu(string from, string to, string languageId);
    }
}