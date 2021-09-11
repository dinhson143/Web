using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.ViewModels.Catalog.Common
{
    public class PagingRequest
    {
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
    }
}