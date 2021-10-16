using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.ViewModels.Catalog.PhieuNhaps
{
    public class CTPhieuNhapCreate
    {
        public int Soluong { get; set; }
        public decimal Dongia { get; set; }
        public decimal Giaban { get; set; }
        public int PhieuNXId { get; set; }
        public int ProductId { get; set; }
        public int SizeId { get; set; }
    }
}