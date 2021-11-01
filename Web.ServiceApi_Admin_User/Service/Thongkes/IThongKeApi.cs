using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.ViewModels.Catalog.Common;
using Web.ViewModels.Catalog.Products;

namespace Web.ServiceApi_Admin_User.Service.Thongkes
{
    public interface IThongKeApi
    {
        public Task<ResultApi<List<ProductViewModel>>> ProductLovest(string languageId, string BearerToken);
    }
}