using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Data.Enums;

namespace Web.Data.Entities
{
    public class OrderOfShipper
    {
        public int Id { get; set; }
        public Guid UserId { set; get; }
        public int OrderID { set; get; }
        public ShipStatus Status { set; get; }
        public DateTime Date { set; get; }
    }
}