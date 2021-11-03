using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.ViewModels.Catalog.Products;

namespace Web.Models
{
    public class ProductSavestModel
    {
        public List<ProductViewModel> listPro { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}