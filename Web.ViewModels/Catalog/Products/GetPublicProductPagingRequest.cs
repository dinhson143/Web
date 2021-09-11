using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.ViewModels.Catalog.Common;

namespace Web.ViewModels.Catalog.Products
{
    public class GetPublicProductPagingRequest : PagingRequest
    {
        public int? CategoryId { get; set; }
    }
}