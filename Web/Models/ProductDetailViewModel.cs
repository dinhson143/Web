using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.ViewModels.Catalog.Products;

namespace Web.Models
{
    public class ProductDetailViewModel
    {
        public ProductViewModel ProductInfo { get; set; }
        public List<ProductImagesModel> Images { get; set; }

        //size  color soluong
    }
}