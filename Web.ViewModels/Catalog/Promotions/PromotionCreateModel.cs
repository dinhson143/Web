using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.ViewModels.Catalog.Common;

namespace Web.ViewModels.Catalog.Promotions
{
    public class PromotionCreateModel
    {
        public List<SelectItems> listCate { get; set; } = new List<SelectItems>();
        public List<SelectItems> listPro { get; set; } = new List<SelectItems>();

        [Display(Name = "Ngày bắt đầu")]
        public DateTime FromDate { get; set; }

        [Display(Name = "Ngày kết thúc")]
        public DateTime ToDate { get; set; }

        [Display(Name = "Áp dụng cho tất cả")]
        public Boolean ApplyAll { get; set; }

        [Display(Name = "Khuyến mãi %")]
        public int? DiscountPercent { get; set; }

        [Display(Name = "Khuyễn mãi giá")]
        public decimal? DiscountAmount { get; set; }

        [Display(Name = "Tên sự kiện")]
        public string Name { get; set; }
    }
}