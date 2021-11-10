using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.ViewModels.Catalog.Common;
using Web.ViewModels.Catalog.Products;

namespace Web.Models
{
    public class ListProductViewModel
    {
        public List<ProductViewModel> listPro { get; set; }
        public PagedResultBase Pager { get; set; }
        public string keyWord { get; set; }
    }
}
