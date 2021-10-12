using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.ViewModels.Catalog.Categories;
using Web.ViewModels.Catalog.Products;

namespace Web.Models
{
    public class SideBarViewModel
    {
        public List<CategoryViewModel> listCate { get; set; }
        public List<ProductViewModel> listpro { get; set; }
    }
}