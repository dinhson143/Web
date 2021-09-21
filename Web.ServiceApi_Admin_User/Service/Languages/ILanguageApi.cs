using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.ViewModels.Catalog.Common;
using Web.ViewModels.Catalog.Languages;

namespace Web.ServiceApi_Admin_User.Service.Languages
{
    public interface ILanguageApi
    {
        public Task<ResultApi<List<LanguageViewModel>>> GetAll(string BearerToken);
    }
}