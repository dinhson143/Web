using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Data.Enums;

namespace Web.Data.Entities
{
    public class LoaiPhieu
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime NgayNhap { set; get; }
        public Status Status { get; set; }

        public List<PhieuNhap_Xuat> PhieuNXs { get; set; }
    }
}