using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.ViewModels.Catalog.PhieuNhaps
{
    public class PhieuNXchitietViewModel
    {
        public int Id { get; set; }
        public int Soluong { get; set; }
        public decimal Dongia { get; set; }
        public decimal Giaban { get; set; }
        public int PhieuNXId { get; set; }

        public string TenSP { get; set; }

        public string TenSize { get; set; }
        public int ProductId { get; set; }
        public int SizeId { get; set; }
    }
}