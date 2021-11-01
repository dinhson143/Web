using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.ViewModels.Catalog.PhieuXuats
{
    public class PhieuXuatViewModel
    {
        public int Id { get; set; }

        public string TenLoaiphieu { get; set; }

        public string TenCongTy { get; set; }

        public DateTime NgayNhap { set; get; }
    }
}