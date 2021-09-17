using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.ViewModels.Catalog.Common;

namespace Web.ViewModels.Catalog.Products
{
    public class GetManageProductPagingRequest : PagingRequest
    {
        public string Keyword { get; set; }
        public string LanguageId { get; set; }
        public int? CategoryId { get; set; }
    }
}