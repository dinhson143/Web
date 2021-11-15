using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.ViewModels.Catalog.Common;
using Web.ViewModels.Catalog.PhieuNhaps;

namespace Web.Application.Catalog.PhieuNhap
{
    public interface IPhieuNhapService
    {
        public Task<int> Delete(int id);
        public Task<string> CreatePhieuNhap(PhieuNhapCreate request);

        public Task<bool> CreateCTPhieuNhap(CTPhieuNhapCreate request);

        public Task<List<PhieuNhapViewModel>> GetAll();

        public Task<List<PhieuNXchitietViewModel>> GetDetailPNById(int id, string languageId);
    }
}