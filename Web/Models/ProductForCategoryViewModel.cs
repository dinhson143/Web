using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.ViewModels.Catalog.Categories;
using Web.ViewModels.Catalog.Common;
using Web.ViewModels.Catalog.Products;

namespace Web.Models
{
    public class ProductForCategoryViewModel
    {
        public PageResult<ProductViewModel> Products { get; set; }
        public CategoryViewModel Categories { get; set; }
        public string keyWord { get; set; }
    }
}