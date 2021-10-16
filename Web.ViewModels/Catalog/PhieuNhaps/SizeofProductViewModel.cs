using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.ViewModels.Catalog.Products;

namespace Web.ViewModels.Catalog.PhieuNhaps
{
    public class SizeofProductViewModel
    {
        public int Id { get; set; }
        public string Name { set; get; }
        public List<ProductSizeViewModel> listPS { get; set; }
    }
}