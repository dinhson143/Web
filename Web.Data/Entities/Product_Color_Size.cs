using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Data.Entities
{
    public class Product_Color_Size
    {
        public int ProductId { get; set; }
        public int SizeId { get; set; }
        public int ColorId { get; set; }
        public int Stock { get; set; }

        public Product Product { get; set; }
        public Color Color { get; set; }
        public Size Size { get; set; }

        public List<PhieuNhap_Xuatchitiet> PhieuNXchitiets { get; set; }
    }
}