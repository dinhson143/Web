using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.ViewModels.Catalog.Common;
using Web.ViewModels.Catalog.Products;
using Web.ViewModels.Catalog.Sliders;

namespace Web.Models
{
    public class HomeModel
    {
        public List<SliderViewModel> sliders { get; set; }
        public List<ProductViewModel> featured_products { get; set; }
        public List<ProductViewModel> latest_products { get; set; }
    }
}