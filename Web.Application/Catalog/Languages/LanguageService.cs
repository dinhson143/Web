using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Data.EF;
using Web.Data.Entities;
using Web.Data.Enums;
using Web.Utilities.Exceptions;
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

        public async Task<ResultApi<string>> CreateLanguage(LanguageCreate request)
        {
            var language = new Language()
            {
                Id = request.Id,
                Name = request.Name,
                Status = Status.Active
            };

            await _context.Languages.AddAsync(language);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return new ResultSuccessApi<string>("Thêm ngôn ngữ thành công");
            }
            return new ResultErrorApi<string>("Thêm ngôn ngữ thất bại");
        }

        public async Task<int> Delete(string id)
        {
            var language = await _context.Languages.FindAsync(id);
            if (language == null) throw new WebException($"Cannot find a ngôn ngữ with id: {id}");

            language.Status = Status.InActive;
            return await _context.SaveChangesAsync();
        }

        public async Task<List<LanguageViewModel>> GetAll()
        {
            var list = await _context.Languages.Where(x => x.Status == Status.Active).Select(x => new LanguageViewModel()
            {
                Id = x.Id,
                Name = x.Name,
            }).ToListAsync();
            return (list);
        }

        public async Task<LanguageViewModel> GetLanguagetById(string id)
        {
            var language = await _context.Languages.FindAsync(id);

            var data = new LanguageViewModel()
            {
                Id = language.Id,
                Name = language.Name,
            };
            return data;
        }

        public async Task<int> UpdateLanguage(LanguageViewModel request)
        {
            var language = await _context.Languages.FirstOrDefaultAsync(x => x.Id == request.Id);

            if (language == null) throw new WebException($"Cannot find a ngôn ngữ with id: {request.Id}");

            language.Name = request.Name;
            return await _context.SaveChangesAsync();
        }
    }
}