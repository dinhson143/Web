using System.Collections.Generic;

namespace Web.ViewModels.Catalog.PhieuNhaps
{
    public class CTPhieuNhapCreateViewModel
    {
        public List<SizeofProductViewModel> listProduct { get; set; } = new List<SizeofProductViewModel>();
        public int ProductId { get; set; }
        public int SizeId { get; set; }
        public int Soluong { get; set; }
        public decimal Dongia { get; set; }
        public decimal Giaban { get; set; }
    }
}