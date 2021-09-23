using System.Collections.Generic;
using System.Threading.Tasks;
using Web.ViewModels.Catalog.Categories;
using Web.ViewModels.Catalog.Common;

namespace Web.ServiceApi_Admin_User.Service.Categories
{
    public interface ICategoryApi
    {
        public Task<ResultApi<List<CategoryViewModel>>> GetAll(string languageId, string BearerToken);

        public Task<CategoryViewModel> GetCategoryById(int id, string languageId);
    }
}