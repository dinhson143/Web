using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.ViewModels.Catalog.Common;
using Web.ViewModels.Catalog.Sizes;

namespace Web.ServiceApi_Admin_User.Service.Sizes
{
    public interface ISizeApi
    {
        public Task<ResultApi<List<SizeViewModel>>> GetAll(string BearerToken);
    }
}