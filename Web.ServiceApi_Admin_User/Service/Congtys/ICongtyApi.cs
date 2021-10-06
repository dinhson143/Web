using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.ViewModels.Catalog.Common;
using Web.ViewModels.Catalog.Congtys;

namespace Web.ServiceApi_Admin_User.Service.Congtys
{
    public interface ICongtyApi
    {
        public Task<ResultApi<string>> CreateCongty(CongtyCreate request, string BearerToken);

        public Task<ResultApi<List<CongtyViewModel>>> GetAll();

        public Task<bool> Update(CongtyViewModel request, string BearerToken);

        public Task<bool> Delete(int congtyId, string BearerToken);

        public Task<CongtyViewModel> GetCongtyById(int id);
    }
}