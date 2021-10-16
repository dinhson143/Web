using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.ViewModels.Catalog.Categories;
using Web.ViewModels.Catalog.Common;

namespace Web.Application.Catalog.Categories
{
    public interface ICategoryService
    {
        public Task<List<CategoryViewModel>> GetAll(string languageId);

        public Task<CategoryViewModel> GetCategoryById(int id, string languageId);

        public Task<List<CategoryViewModel>> GetAllCategory_parent(string languageId);

        public Task<List<CategoryViewModel>> GetAllCategory_child(int parentId, string languageId);

        public Task<ResultApi<string>> CreateCategory(CategoryCreate request);

        public Task<int> UpdateCategory(CategoryUpdateRequest request);

        public Task<int> Delete(int id);
    }
}