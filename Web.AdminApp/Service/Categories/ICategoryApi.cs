using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.ViewModels.Catalog.Categories;
using Web.ViewModels.Catalog.Common;

namespace Web.AdminApp.Service.Categories
{
    public interface ICategoryApi
    {
        public Task<ResultApi<List<CategoryViewModel>>> GetAll(string languageId, string BearerToken);
    }
}