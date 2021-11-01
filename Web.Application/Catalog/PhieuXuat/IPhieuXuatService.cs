using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.ViewModels.Catalog.Common;
using Web.ViewModels.Catalog.PhieuNhaps;
using Web.ViewModels.Catalog.PhieuXuats;

namespace Web.Application.Catalog.PhieuXuat
{
    public interface IPhieuXuatService
    {
        public Task<ResultApi<string>> CreatePhieuXuat(PhieuXuatCreate request);

        public Task<bool> CreateCTPhieXuat(CTPhieuXuatCreate request);

        public Task<List<PhieuXuatViewModel>> GetAll();

        public Task<List<PhieuNXchitietViewModel>> GetDetailPXById(int id, string languageId);
    }
}