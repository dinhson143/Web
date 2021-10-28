using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.ViewModels.Catalog.Categories;
using Web.ViewModels.Catalog.Products;

namespace Web.ViewModels.Catalog.Promotions
{
    public class PromotionViewModel
    {
        [Display(Name = "Mã sự kiện")]
        public int Id { get; set; }

        [Display(Name = "Ngày bắt đầu")]
        public DateTime FromDate { get; set; }

        [Display(Name = "Ngày kết thúc")]
        public DateTime ToDate { get; set; }

        [Display(Name = "Áp dụng")]
        public Boolean ApplyAll { get; set; }

        [Display(Name = "Khuyến mãi %")]
        public int? DiscountPercent { get; set; }

        [Display(Name = "Khuyễn mãi giá")]
        public decimal? DiscountAmount { get; set; }

        public string ProductIDs { get; set; }
        public string ProductCategoryIds { get; set; }

        [Display(Name = "Tên sự kiện")]
        public string Name { get; set; }

        public string Status { get; set; }
        public List<ProductViewModel> listPro { get; set; } = new List<ProductViewModel>();
        public List<CategoryViewModel> listCate { get; set; } = new List<CategoryViewModel>();
    }
}