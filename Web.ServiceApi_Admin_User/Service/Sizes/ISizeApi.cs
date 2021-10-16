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

        public Task<bool> Delete(int sizeId, string BearerToken);

        public Task<ResultApi<string>> CreateSize(SizeViewModel request, string BearerToken);

        public Task<SizeViewModel> GetSizeById(int id, string BearerToken);
    }
}