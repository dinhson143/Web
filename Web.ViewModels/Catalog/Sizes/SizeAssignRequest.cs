using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.ViewModels.Catalog.Common;

namespace Web.ViewModels.Catalog.Sizes
{
    public class SizeAssignRequest : RequestBase
    {
        public int Id { get; set; }
        public List<SelectItems> Sizes { get; set; } = new List<SelectItems>();
    }
}