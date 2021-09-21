using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Data.EF;
using Web.ViewModels.Catalog.Sliders;

namespace Web.Application.Catalog.Sliders
{
    public class SliderService : ISliderService
    {
        private readonly AppDbContext _context;

        public SliderService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<SliderViewModel>> GetAll()
        {
            var query = from sl in _context.Sliders
                        select new { sl };
            return await query.Select(x => new SliderViewModel()
            {
                Id = x.sl.Id,
                Name = x.sl.Name,
                Description = x.sl.Description,
                Image = x.sl.Image,
                Url = x.sl.Url,
                SortOrder = x.sl.SortOrder,
                Status = x.sl.Status
            }).ToListAsync();
        }
    }
}