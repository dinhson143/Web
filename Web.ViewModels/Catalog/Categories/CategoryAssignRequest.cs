using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.ViewModels.Catalog.Common;

namespace Web.ViewModels.Catalog.Categories
{
    public class CategoryAssignRequest : RequestBase
    {
        public int Id { get; set; }
        public List<SelectItems> Categories { get; set; } = new List<SelectItems>();
    }
}