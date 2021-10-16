using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.ViewModels.Catalog.Products
{
    public class ProductImagesModel
    {
        public string URL { get; set; }
        public bool isDefault { get; set; }
        public string Caption { get; set; }
    }
}