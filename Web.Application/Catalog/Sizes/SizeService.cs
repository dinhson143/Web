using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Data.EF;
using Web.Data.Enums;
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
    }
}