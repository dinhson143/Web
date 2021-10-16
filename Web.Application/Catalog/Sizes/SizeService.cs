using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Data.EF;
using Web.Data.Entities;
using Web.Data.Enums;
using Web.Utilities.Exceptions;
using Web.ViewModels.Catalog.Common;
using Web.ViewModels.Catalog.Sizes;

namespace Web.Application.Catalog.Sizes
{
    public class SizeService : ISizeService
    {
        private readonly AppDbContext _context;

        public SizeService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResultApi<string>> CreateSize(SizeViewModel request)
        {
            var size = new Size()
            {
                Name = request.Name,
                Status = Status.Active
            };

            await _context.Sizes.AddAsync(size);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return new ResultSuccessApi<string>("Thêm size thành công");
            }
            return new ResultErrorApi<string>("Thêm size thất bại");
        }

        public async Task<int> Delete(int id)
        {
            var size = await _context.Sizes.FindAsync(id);
            if (size == null) throw new WebException($"Cannot find a size with id: {id}");

            size.Status = Status.InActive;
            return await _context.SaveChangesAsync();
        }

        public async Task<List<SizeViewModel>> GetAll()
        {
            var query = from s in _context.Sizes
                        where s.Status == Status.Active
                        select new { s };
            return await query.Select(x => new SizeViewModel()
            {
                Id = x.s.Id,
                Name = x.s.Name
            }).ToListAsync();
        }

        public async Task<SizeViewModel> GetSizeById(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            var categoryTranslation = await _context.CategoryTranslations.FirstOrDefaultAsync(x => x.CategoryId == id);

            var data = new SizeViewModel()
            {
                Id = category.Id,
                Name = categoryTranslation.Name
            };
            return data;
        }
    }
}