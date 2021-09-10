using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Data.Entities
{
    public class PhieuNhap_Xuatchitiet
    {
        public int Id { get; set; }
        public int Soluong { get; set; }
        public decimal Dongia { get; set; }
        public decimal Giaban { get; set; }
        public int PhieuNXId { get; set; }
        public int ProductId { get; set; }

        public PhieuNhap_Xuat PhieuNX { get; set; }
        public Product Product { get; set; }
    }
}