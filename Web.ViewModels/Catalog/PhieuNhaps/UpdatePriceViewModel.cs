using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.ViewModels.Catalog.PhieuNhaps
{
    public class UpdatePriceViewModel
    {
        public List<SizeofProductViewModel> listProduct { get; set; } = new List<SizeofProductViewModel>();
        public int ProductId { get; set; }
        public int SizeId { get; set; }
        public decimal Price { get; set; }
    }
}