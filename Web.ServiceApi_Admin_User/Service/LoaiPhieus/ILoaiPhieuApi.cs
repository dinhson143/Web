using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.ViewModels.Catalog.Common;
using Web.ViewModels.Catalog.LoaiPhieus;

namespace Web.ServiceApi_Admin_User.Service.LoaiPhieus
{
    public interface ILoaiPhieuApi
    {
        public Task<ResultApi<List<LoaiPhieuViewModel>>> GetAll(string BearerToken);

        public Task<bool> Delete(int loaiphieuId, string BearerToken);

        public Task<ResultApi<string>> CreateLoaiPhieu(LoaiPhieuCreate request, string BearerToken);
    }
}