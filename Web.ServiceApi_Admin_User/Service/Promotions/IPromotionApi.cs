using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.ViewModels.Catalog.Common;
using Web.ViewModels.Catalog.Promotions;

namespace Web.ServiceApi_Admin_User.Service.Promotions
{
    public interface IPromotionApi
    {
        public Task<ResultApi<string>> CreatePromotion(PromotionCreate request, string BearerToken);

        public Task<ResultApi<List<PromotionViewModel>>> GetAll();

        public Task<bool> Block(int promotionId, string BearerToken);
        public Task<string> KiemtraPromotions();
    }
}