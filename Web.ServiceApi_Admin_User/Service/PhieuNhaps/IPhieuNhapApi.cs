using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.ViewModels.Catalog.Common;
using Web.ViewModels.Catalog.PhieuNhaps;

namespace Web.ServiceApi_Admin_User.Service.PhieuNhaps
{
    public interface IPhieuNhapApi
    {
        public Task<ResultApi<string>> CreatePhieuNhap(PhieuNhapCreate request, string BearerToken);

        public Task<ResultApi<List<PhieuNhapViewModel>>> GetAll(string BearerToken);

        public Task<List<PhieuNXchitietViewModel>> GetPhieuNhapById(int id, string languageId, string BearerToken);

        public Task<bool> CreateCTPhieuNhap(CTPhieuNhapCreate request, string BearerToken);
    }
}