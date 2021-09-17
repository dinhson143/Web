using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Data.EF;
using Web.ViewModels.Catalog.Categories;

namespace Web.Application.Catalog.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;

        public CategoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<CategoryViewModel>> GetAll(string languageID)
        {
            var query = from c in _context.Categories
                        join ct in _context.CategoryTranslations on c.Id equals ct.CategoryId
                        where ct.LanguageId == languageID
                        select new { c, ct };
            return await query.Select(x => new CategoryViewModel()
            {
                Id = x.c.Id,
                Name = x.ct.Name
            }).ToListAsync();
        }
    }
}