using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Data.EF;
using Web.Data.Entities;
using Web.Data.Enums;
using Web.Utilities.Exceptions;
using Web.ViewModels.Catalog.Categories;
using Web.ViewModels.Catalog.Common;

namespace Web.Application.Catalog.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;

        public CategoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResultApi<string>> CreateCategory(CategoryCreate request)
        {
            var languages = _context.Languages;
            var tranlations = new List<CategoryTranslation>();
            foreach (var language in languages)
            {
                if (language.Id == request.LanguageId)
                {
                    tranlations.Add(new CategoryTranslation()
                    {
                        Name = request.Name,
                        SeoDescription = request.SeoDescription,
                        SeoAlias = request.SeoAlias,
                        SeoTitle = request.SeoTitle,
                        LanguageId = request.LanguageId
                    });
                }
                else
                {
                    tranlations.Add(new CategoryTranslation()
                    {
                        Name = "N/A",
                        SeoDescription = "N/A",
                        SeoTitle = "N/A",
                        SeoAlias = "N/A",
                        LanguageId = language.Id
                    });
                }
            }
            var category = new Category()
            {
                IsShowonHome = true,
                ParentId = request.ParentId,
                CategoryTranslations = tranlations,
                Status = Status.Active
            };

            await _context.Categories.AddAsync(category);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return new ResultSuccessApi<string>("Thêm loại sản phẩm thành công");
            }
            return new ResultErrorApi<string>("Thêm loại sản phẩm thất bại");
        }

        public async Task<int> Delete(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) throw new WebException($"Cannot find a category with id: {id}");

            category.Status = Status.InActive;
            return await _context.SaveChangesAsync();
        }

        public async Task<List<CategoryViewModel>> GetAll(string languageID)
        {
            var query = from c in _context.Categories
                        join ct in _context.CategoryTranslations on c.Id equals ct.CategoryId
                        where ct.LanguageId == languageID && c.Status == Status.Active
                        select new { c, ct };
            return await query.Select(x => new CategoryViewModel()
            {
                Id = x.c.Id,
                Name = x.ct.Name,
                ParentId = x.c.ParentId,
                SeoDescription = x.ct.SeoDescription,
                SeoTitle = x.ct.SeoTitle
            }).ToListAsync();
        }

        public async Task<List<CategoryViewModel>> GetAllCategory_child(int parentId, string languageId)
        {
            var query = from c in _context.Categories
                        join ct in _context.CategoryTranslations on c.Id equals ct.CategoryId
                        where c.ParentId == parentId && ct.LanguageId == languageId && c.Status == Status.Active
                        select new { c, ct };
            return await query.Select(x => new CategoryViewModel()
            {
                Id = x.c.Id,
                Name = x.ct.Name,
                ParentId = x.c.ParentId,
                SeoDescription = x.ct.SeoDescription,
                SeoTitle = x.ct.SeoTitle
            }).ToListAsync();
        }

        public async Task<List<CategoryViewModel>> GetAllCategory_parent(string languageId)
        {
            var query = from c in _context.Categories
                        join ct in _context.CategoryTranslations on c.Id equals ct.CategoryId
                        where c.ParentId == null && ct.LanguageId == languageId && c.Status == Status.Active
                        select new { c, ct };
            return await query.Select(x => new CategoryViewModel()
            {
                Id = x.c.Id,
                Name = x.ct.Name,
                ParentId = x.c.ParentId,
                SeoDescription = x.ct.SeoDescription,
                SeoTitle = x.ct.SeoTitle
            }).ToListAsync();
        }

        public async Task<CategoryViewModel> GetCategoryById(int id, string languageId)
        {
            var category = await _context.Categories.FindAsync(id);
            var categoryTranslation = await _context.CategoryTranslations.FirstOrDefaultAsync(x => x.CategoryId == id
            && x.LanguageId == languageId);

            var data = new CategoryViewModel()
            {
                Id = category.Id,
                Name = categoryTranslation.Name,
                ParentId = category.ParentId,
                SeoDescription = categoryTranslation.SeoDescription,
                SeoTitle = categoryTranslation.SeoTitle,
                SeoAlias = categoryTranslation.SeoAlias
            };
            return data;
        }

        public async Task<int> UpdateCategory(CategoryUpdateRequest request)
        {
            //var category = await _context.Categories.FindAsync(request.Id);
            var categoryTranslations = await _context.CategoryTranslations.FirstOrDefaultAsync(x => x.CategoryId == request.Id
            && x.LanguageId == request.LanguageId);

            if (categoryTranslations == null) throw new WebException($"Cannot find a category with id: {request.Id}");

            categoryTranslations.Name = request.Name;
            categoryTranslations.SeoAlias = request.SeoAlias;
            categoryTranslations.SeoDescription = request.SeoDescription;
            categoryTranslations.SeoTitle = request.SeoTitle;
            return await _context.SaveChangesAsync();
        }
    }
}