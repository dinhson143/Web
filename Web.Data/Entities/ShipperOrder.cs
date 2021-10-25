using System;
using Web.Data.Enums;

namespace Web.Data.Entities
{
    public class ShipperOrder
    {
        public int Id { get; set; }
        public Guid ShipperId { set; get; }
        public int OrderID { set; get; }
        public ShipStatus Status { set; get; }
        public DateTime Date { set; get; }
    }
}