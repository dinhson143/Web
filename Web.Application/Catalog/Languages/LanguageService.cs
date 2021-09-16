using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Data.EF;
using Web.ViewModels.Catalog.Common;
using Web.ViewModels.Catalog.Languages;

namespace Web.Application.Catalog.Languages
{
    public class LanguageService : ILanguageService
    {
        private readonly AppDbContext _context;

        public LanguageService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<LanguageViewModel>> GetAll()
        {
            var list = await _context.Languages.Select(x => new LanguageViewModel()
            {
                Id = x.Id,
                Name = x.Name,
            }).ToListAsync();
            return (list);
        }
    }
}