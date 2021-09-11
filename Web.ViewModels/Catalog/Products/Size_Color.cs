using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.ViewModels.Catalog.Products
{
    public class Size_Color
    {
        public int ColorId { get; set; }
        public int SizeId { get; set; }
        public string Tenmau { get; set; }
        public string Mamau { get; set; }
        public string Size { get; set; }
        public int Stock { get; set; }
    }
}