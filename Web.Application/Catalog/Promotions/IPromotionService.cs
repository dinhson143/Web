using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.ViewModels.Catalog.Common;
using Web.ViewModels.Catalog.Promotions;

namespace Web.Application.Catalog.Promotions
{
    public interface IPromotionService
    {
        public Task<ResultApi<string>> CreatePromotion(PromotionCreate request);

        public Task<List<PromotionViewModel>> GetAll();

        public Task<int> Block(int id);
        public Task<string> KiemtraPromotions();
    }
}