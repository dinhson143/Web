using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Data.EF;
using Web.ViewModels.Catalog.Common;
using Web.ViewModels.Catalog.Products;

namespace Web.Application.Catalog.ThongKes
{
    public class ThongKeService : IThongKeService
    {
        private readonly AppDbContext _context;

        public ThongKeService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResultApi<List<ProductViewModel>>> ProductLovest(string languageId)
        {
            // get list product
            var query = (from p in _context.Products
                         join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                         join pic in _context.ProductInCategories on p.Id equals pic.ProductId into ppic
                         from pic in ppic.DefaultIfEmpty()
                         join c in _context.Categories on pic.CategoryId equals c.Id into picc
                         from c in picc.DefaultIfEmpty()
                         where pt.LanguageId == languageId
                         select new { p, pt, pic });
            // 3 .Paging
            int totalRow = await query.CountAsync();
            var data = await query.Select(x => new ProductViewModel()
            {
                Id = x.p.Id,
                //Price = x.p.Price,
                //OriginalPrice = x.p.OriginalPrice,
                ViewCount = x.p.ViewCount,
                DateCreated = x.p.DateCreated,
                Name = x.pt.Name,
                Description = x.pt.Description,
                Details = x.pt.Details,
                SeoDescription = x.pt.SeoDescription,
                SeoTitle = x.pt.SeoTitle,
                SeoAlias = x.pt.SeoAlias,
            }).ToListAsync();
            // 4 Select Page Result

            // remove phần tử trùng
            for (var i = 0; i < data.Count; i++)
            {
                for (var j = i + 1; j < data.Count; j++)
                {
                    if (data[i].Id == data[j].Id)
                    {
                        data.Remove(data[j]);
                    }
                }
            }
            //
            foreach (var item in data)
            {
                var danhgia = await (from c in _context.Comments
                                     where c.ProductId == item.Id
                                     select c).ToListAsync();
                decimal diem = 0;
                var dem = 0;
                foreach (var dg in danhgia)
                {
                    diem += dg.Star;
                    dem++;
                }
                if (dem != 0)
                {
                    decimal diemDM = diem / dem;
                    diem = Math.Round(diemDM);
                }
                item.Diem = diem;
            }

            foreach (var item in data)
            {
                var product = await _context.Products.FindAsync(item.Id);
                var images = from pi in _context.ProductImages
                             where pi.ProductId == item.Id
                             select pi;
                var Listimage = await images.Select(x => new ProductImagesModel()
                {
                    URL = x.ImagePath,
                    isDefault = x.IsDefault,
                    Caption = x.Caption,
                    Id = x.Id
                }).ToListAsync();
                item.Images = Listimage;
            }

            data = data.OrderByDescending(item => item.Diem).ToList();
            return new ResultSuccessApi<List<ProductViewModel>>(data);
        }
    }
}