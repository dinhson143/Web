using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.ViewModels.Catalog.Products
{
    public class ProductFavoriteViewModel
    {
        public int ProductId { get; set; }
        public string URL { get; set; }
        public string TenProduct { get; set; }
        public string Description { get; set; }
    }
}