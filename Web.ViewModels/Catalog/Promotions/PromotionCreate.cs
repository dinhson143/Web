using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.ViewModels.Catalog.Common;

namespace Web.ViewModels.Catalog.Promotions
{
    public class PromotionCreate
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public Boolean ApplyAll { get; set; }
        public int? DiscountPercent { get; set; }
        public decimal? DiscountAmount { get; set; }

        public string ProductIDs { get; set; }
        public string ProductCategoryIds { get; set; }
        public string Name { get; set; }
    }
}