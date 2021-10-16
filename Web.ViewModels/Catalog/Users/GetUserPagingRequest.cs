using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.ViewModels.Catalog.Common;

namespace Web.ViewModels.Catalog.Users
{
    public class GetUserPagingRequest : PagingRequest
    {
        public string Keyword { get; set; }
    }
}