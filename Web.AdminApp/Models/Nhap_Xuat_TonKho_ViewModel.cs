using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.ViewModels.Catalog.Orders;
using Web.ViewModels.Catalog.PhieuNhaps;

namespace Web.AdminApp.Models
{
    public class Nhap_Xuat_TonKho_ViewModel
    {
        public List<PhieuNhapViewModel> PhieuNhap { get; set; }
        public List<OrderViewModel> PhieuXuat { get; set; }
        public int Id { get; set; }
    }
}