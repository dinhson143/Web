using Web.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Data.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public Status Status { get; set; }
        public int? ParentId { get; set; }
        public string Content { get; set; }
        public int Star { get; set; }
        public DateTime DateCreated { get; set; }
        public int ProductId { get; set; }
        public Guid UserId { get; set; }

        public Product Product { get; set; }
        public User User { get; set; }
    }
}