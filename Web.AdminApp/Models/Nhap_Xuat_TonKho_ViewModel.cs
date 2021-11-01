using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.ViewModels.Catalog.Orders;
using Web.ViewModels.Catalog.PhieuNhaps;
using Web.ViewModels.Catalog.PhieuXuats;
using Web.ViewModels.Catalog.Products;

namespace Web.AdminApp.Models
{
    public class Nhap_Xuat_TonKho_ViewModel
    {
        public List<PhieuNhapViewModel> PhieuNhap { get; set; }
        public List<PhieuXuatViewModel> PhieuXuat { get; set; }
        public List<ProductViewModel> Products { get; set; }
        public int Id { get; set; }
    }
}