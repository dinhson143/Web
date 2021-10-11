using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Data.Enums;

namespace Web.ViewModels.Catalog.Comments
{
    public class CommentViewModel
    {
        public int Id { get; set; }
        public Status Status { get; set; }
        public int? ParentId { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }
        public int Star { get; set; }

        public int ProductId { get; set; }
        public Guid UserId { get; set; }
        public string TenUser { get; set; }

        public string TenSP { get; set; }
    }
}