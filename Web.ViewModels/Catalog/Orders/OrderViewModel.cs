using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Data.Enums;

namespace Web.ViewModels.Catalog.Orders
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime DateSuccess { get; set; }
        public string ShipName { get; set; }
        public string ShipAddress { get; set; }
        public string ShipEmail { get; set; }
        public string ShipPhone { get; set; }
        public string Status { get; set; }
        public string LoaiThanhToan { get; set; }
        public decimal Tongtien { get; set; }
        public decimal TongtienReal { get; set; }
        public List<OrderDetailViewModel> ListOrDetail { get; set; }
    }
}