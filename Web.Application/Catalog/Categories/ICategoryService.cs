using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.ViewModels.Catalog.Categories;

namespace Web.Application.Catalog.Categories
{
    public interface ICategoryService
    {
        public Task<List<CategoryViewModel>> GetAll(string languageId);

        public Task<CategoryViewModel> GetCategoryById(int id, string languageId);
    }
}