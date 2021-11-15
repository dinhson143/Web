using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.ViewModels.Catalog.Congtys;
using Web.ViewModels.Catalog.PhieuNhaps;

namespace Web.AdminApp.Models
{
    public class PhieuNhapCreateViewModel
    {
        public List<CongtyViewModel> Congtys { get; set; }
        public List<SizeofProductViewModel> listProduct { get; set; } = new List<SizeofProductViewModel>();
        public int ProductId { get; set; }
        public int SizeId { get; set; }
        public int Soluong { get; set; }
        public decimal Dongia { get; set; }
        public decimal Giaban { get; set; }
        public string CurrentCongtyId { get; set; }
    }
}