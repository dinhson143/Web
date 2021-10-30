using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Data.Entities;
using Web.ViewModels.Catalog.Categories;

namespace Web.ViewModels.Catalog.Products
{
    public class ProductViewModel
    {
        [Display(Name = "Mã SP")]
        public int Id { get; set; }

        [Display(Name = "Lượt xem")]
        public int ViewCount { get; set; }

        public decimal Price { get; set; }

        [Display(Name = "Ngày nhập")]
        public DateTime DateCreated { get; set; }

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

        public int? DiscountPercent { get; set; }
        public decimal? DiscountAmount { get; set; }
        public bool? IsFeatured { get; set; }
        public string LanguageId { set; get; }

        public string Image { set; get; }

        public List<string> Categories { set; get; }
        public List<string> Sizes { set; get; }
        public List<ProductImagesModel> Images { set; get; }
        public List<ProductSizeViewModel> listPS { get; set; }
        public int SluongDaban { get; set; }
        public decimal Diem { get; set; }
    }
}