using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.ViewModels.Catalog.Categories;

namespace Web.AdminApp.Models
{
    public class DetailCategoryViewModel
    {
        public List<CategoryViewModel> listCategory { get; set; }
        public int? ParentId { get; set; }
    }
}