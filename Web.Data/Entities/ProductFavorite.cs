using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Data.Enums;

namespace Web.Data.Entities
{
    public class ProductFavorite
    {
        public int Id { get; set; }
        public Status Status { get; set; }
        public int ProductId { get; set; }
        public Guid UserId { get; set; }

        public Product Product { get; set; }
        public User User { get; set; }
    }
}