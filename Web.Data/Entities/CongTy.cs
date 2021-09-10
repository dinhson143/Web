using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Data.Entities
{
    public class CongTy
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Masothue { get; set; }
        public string Diachi { get; set; }
        public string Sdt { get; set; }

        public List<PhieuNhap_Xuat> PhieuNXs { get; set; }
    }
}