using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Data.EF;
using Web.Data.Entities;
using Web.Data.Enums;
using Web.ViewModels.Catalog.Categories;
using Web.ViewModels.Catalog.Common;
using Web.ViewModels.Catalog.Products;
using Web.ViewModels.Catalog.Promotions;

namespace Web.Application.Catalog.Promotions
{
    public class PromotionService : IPromotionService
    {
        private readonly AppDbContext _context;

        public PromotionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> Block(int id)
        {
            var promotion = await _context.Promotions.FindAsync(id);
            if (promotion.Status == Status.Active)
            {
                promotion.Status = Status.InActive;
            }
            else
            {
                promotion.Status = Status.Active;
            }
            return await _context.SaveChangesAsync();
        }

        public async Task<ResultApi<string>> CreatePromotion(PromotionCreate request)
        {
            var promotion = new Promotion()
            {
                ToDate = request.ToDate,
                FromDate = request.FromDate,
                ApplyForAll = request.ApplyAll,
                DiscountAmount = request.DiscountAmount,
                DiscountPercent = request.DiscountPercent,
                Name = request.Name,
                ProductCategoryIds = request.ProductCategoryIds,
                ProductIds = request.ProductIDs,
                Status = Status.Active
            };

            await _context.Promotions.AddAsync(promotion);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return new ResultSuccessApi<string>("Thêm khuyến mãi thành công");
            }
            return new ResultErrorApi<string>("Thêm khuyến mãi thất bại");
        }

        public async Task<List<PromotionViewModel>> GetAll()
        {
            var query = from p in _context.Promotions
                            //where p.Status == Status.Active
                        select new { p };

            var list = await query.Select(x => new PromotionViewModel()
            {
                ApplyAll = x.p.ApplyForAll,
                DiscountAmount = x.p.DiscountAmount,
                DiscountPercent = x.p.DiscountPercent,
                FromDate = x.p.FromDate,
                ToDate = x.p.ToDate,
                Name = x.p.Name,
                Id = x.p.Id,
                ProductCategoryIds = x.p.ProductCategoryIds,
                ProductIDs = x.p.ProductIds,
                Status = x.p.Status.ToString()
            }).ToListAsync();
            return list;
        }
        public async Task<string> KiemtraPromotions()
        {
            string message = "Hiện tại cửa hàng không có đợt khuyến mãi nào 😅😅😅.";
            // get list promotion
            var query2 = from p in _context.Promotions
                             //where p.Status == Status.Active
                         select new { p };
            var listPromotion = await query2.Select(x => new PromotionViewModel()
            {
                ApplyAll = x.p.ApplyForAll,
                DiscountAmount = x.p.DiscountAmount,
                DiscountPercent = x.p.DiscountPercent,
                FromDate = x.p.FromDate,
                ToDate = x.p.ToDate,
                Name = x.p.Name,
                Id = x.p.Id,
                ProductCategoryIds = x.p.ProductCategoryIds,
                ProductIDs = x.p.ProductIds,
                Status = x.p.Status.ToString()
            }).ToListAsync();
            var dn = DateTime.Now;
            foreach (var item in listPromotion)
            {
                if ((dn.Ticks >= item.FromDate.Ticks) && (dn.Ticks <= item.ToDate.Ticks))
                {
                    message = " Hiện tại cửa hàng đang có đợt khuyến mãi " + item.Name + " 🌟🌟🌟. Xem các sản phẩm đang được khuyến mãi 💁 https://localhost:44388/vi/Product/ListProducts 💁.";
                }
            }
            return message;
        }
    }
}