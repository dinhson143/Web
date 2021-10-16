using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Data.Enums;

namespace Web.Data.Entities
{
    public class PhieuNhap_Xuat
    {
        public int Id { get; set; }
        public int LoaiPhieuId { get; set; }
        public int CongTyId { get; set; }
        public DateTime NgayNhap { set; get; }
        public Status Status { get; set; }

        public CongTy CongTy { get; set; }
        public LoaiPhieu LoaiPhieu { get; set; }

        public List<PhieuNhap_Xuatchitiet> PhieuNXchitiets { get; set; }
    }
}