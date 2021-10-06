using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.ViewModels.Catalog.Common;
using Web.ViewModels.Catalog.LoaiPhieus;

namespace Web.Application.Catalog.LoaiPhieus
{
    public interface ILoaiPhieuService
    {
        public Task<List<LoaiPhieuViewModel>> GetAll();

        public Task<ResultApi<string>> CreateLoaiPhieu(LoaiPhieuCreate request);

        public Task<int> Delete(int id);
    }
}