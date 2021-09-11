using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Data.Enums;

namespace Web.ViewModels.Catalog.Products
{
    public class ProductCreate
    {
        public decimal Price { get; set; }
        public decimal OriginalPrice { get; set; }
        public string Name { set; get; }
        public string Description { set; get; }
        public string Details { set; get; }
        public string SeoDescription { set; get; }
        public string SeoTitle { set; get; }

        public string SeoAlias { get; set; }
        public string LanguageId { set; get; }
        public int CategoryId { set; get; }

        public List<string> ImageURL { set; get; }
    }
}