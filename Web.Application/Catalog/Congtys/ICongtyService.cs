using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.ViewModels.Catalog.Common;
using Web.ViewModels.Catalog.Congtys;

namespace Web.Application.Catalog.Congtys
{
    public interface ICongtyService
    {
        public Task<ResultApi<string>> CreateCongty(CongtyCreate request);

        public Task<List<CongtyViewModel>> GetAll();

        public Task<int> UpdateCongty(CongtyViewModel request);

        public Task<int> Delete(int id);

        public Task<CongtyViewModel> GetCongtytById(int id);
    }
}