using Web.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Data.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public decimal OriginalPrice { get; set; }

        //public int Stock { get; set; }
        public int ViewCount { get; set; }

        public DateTime DateCreated { get; set; }
        public Status Status { get; set; }

        public List<Cart> Carts { get; set; }
        public List<Comment> Comments { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }

        //public List<PhieuNhap_Xuatchitiet> PhieuNXchitiets { get; set; }
        public List<ProductImage> ProductImages { get; set; }

        public List<ProductInCategory> ProductInCategories { get; set; }
        public List<ProductTranslation> ProductTranslations { get; set; }
        public List<Product_Color_Size> PCS { get; set; }
    }
}