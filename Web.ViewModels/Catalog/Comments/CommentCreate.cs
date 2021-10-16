using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.ViewModels.Catalog.Comments
{
    public class CommentCreate
    {
        public string Content { get; set; }
        public int Star { get; set; }
        public int ProductId { get; set; }
        public string Email { get; set; }
        public int? ParentId { get; set; }
    }
}