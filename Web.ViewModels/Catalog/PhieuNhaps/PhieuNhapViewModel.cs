using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.ViewModels.Catalog.PhieuNhaps
{
    public class PhieuNhapViewModel
    {
        public int Id { get; set; }

        //public int LoaiPhieuId { get; set; }
        public string TenLoaiphieu { get; set; }

        public string TenCongTy { get; set; }

        //public int CongTyId { get; set; }
        public DateTime NgayNhap { set; get; }

        //public List<PhieuNXchitietViewModel> listCT { set; get; } = new List<PhieuNXchitietViewModel>();
    }
}