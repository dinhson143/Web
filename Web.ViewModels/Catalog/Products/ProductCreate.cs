using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Data.Enums;
using Web.ViewModels.Catalog.Common;

namespace Web.ViewModels.Catalog.Products
{
    public class ProductCreate : RequestBase
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

        public IFormFile[] ImageURL { set; get; }
    }
}