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

        [Display(Name = "Giá bán")]
        public decimal Price { get; set; }

        [Display(Name = "Giá nhập")]
        public decimal OriginalPrice { get; set; }

        [Display(Name = "Lượt xem")]
        public int ViewCount { get; set; }

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

        public string LanguageId { set; get; }

        public List<string> Categories { set; get; }
    }
}