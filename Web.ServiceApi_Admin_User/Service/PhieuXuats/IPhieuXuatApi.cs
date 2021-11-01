using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.ViewModels.Catalog.Common;
using Web.ViewModels.Catalog.PhieuNhaps;
using Web.ViewModels.Catalog.PhieuXuats;

namespace Web.ServiceApi_Admin_User.Service.PhieuXuats
{
    public interface IPhieuXuatApi
    {
        public Task<ResultApi<string>> CreatePhieuXuat(PhieuXuatCreate request, string BearerToken);

        public Task<ResultApi<List<PhieuXuatViewModel>>> GetAll(string BearerToken);

        public Task<List<PhieuNXchitietViewModel>> GetPhieuXuatById(int id, string languageId, string BearerToken);

        public Task<bool> CreateCTPhieuXuat(CTPhieuXuatCreate request, string BearerToken);
    }
}