using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.ViewModels.Catalog.Products
{
    public class ProductLienQuanViewModel
    {
        [Display(Name = "Mã SP")]
        public int Id { get; set; }

        [Display(Name = "Tên")]
        public string Name { set; get; }

        [Display(Name = "Mô tả")]
        public string Description { set; get; }

        [Display(Name = "Chi tiết")]
        public string Details { set; get; }

        [Display(Name = "Mô tả Seo")]
        public string SeoDescription { set; get; }

        [Display(Name = "Tiêu đề Seo")]
        public string SeoTitle { set; get; }

        [Display(Name = "Link Seo")]
        public string SeoAlias { get; set; }

        public string Image { set; get; }
        public List<ProductSizeViewModel> listPS { get; set; }
    }
}