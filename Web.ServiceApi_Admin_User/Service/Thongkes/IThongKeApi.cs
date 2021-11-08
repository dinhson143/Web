using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.ViewModels.Catalog.Common;
using Web.ViewModels.Catalog.Orders;
using Web.ViewModels.Catalog.Products;

namespace Web.ServiceApi_Admin_User.Service.Thongkes
{
    public interface IThongKeApi
    {
        public Task<ResultApi<List<ProductViewModel>>> ProductLovest(string languageId, string BearerToken);

        public Task<ResultApi<List<ProductViewModel>>> ProductSavest(string from, string to, string languageId, string BearerToken);

        public Task<ResultApi<List<ProductViewModel>>> ProductSavestFullMonth(string languageId, string BearerToken);

        public Task<ResultApi<List<OrderViewModel>>> Doanhthu(string from, string to, string languageId, string BearerToken);

        public Task<ResultApi<List<OrderViewModel>>> DoanhthuFullMonth(string languageId, string BearerToken);
    }
}