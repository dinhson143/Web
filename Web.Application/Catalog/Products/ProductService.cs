using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Data.EF;
using Web.Data.Entities;
using Web.Utilities.Exceptions;
using Web.ViewModels.Catalog.Common;
using Web.ViewModels.Catalog.Products;

namespace Web.Application.Catalog.Products
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        public Task AddImage()
        {
            throw new NotImplementedException();
        }

        public async Task AddSize_Color(int productId, Size_Color request)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new WebException($"Can not find a Product: {productId}");

            product.PCS = new List<Product_Color_Size>()
            {
                new Product_Color_Size()
                {
                    Stock = 0,
                    ColorId = request.ColorId,
                    SizeId = request.SizeId
                }
            };
        }

        public Task AddViewCount()
        {
            throw new NotImplementedException();
        }

        public async Task<int> CreateProduct(ProductCreate request)
        {
            var product = new Product()
            {
                Price = request.Price,
                OriginalPrice = request.OriginalPrice,
                //Stock = request.Stock,
                ViewCount = 0,
                DateCreated = DateTime.Now,
                ProductTranslations = new List<ProductTranslation>()
                {
                    new ProductTranslation()
                    {
                        Name = request.Name,
                        Description = request.Description,
                        Details = request.Details,
                        SeoDescription = request.SeoDescription,
                        SeoAlias = request.SeoAlias,
                        SeoTitle = request.SeoTitle,
                        LanguageId = request.LanguageId
                    }
                },
                ProductInCategories = new List<ProductInCategory>()
                {
                    new ProductInCategory()
                    {
                        CategoryId = request.CategoryId,
                    }
                }
            };

            if (request.ImageURL.Count > 0)
            {
                List<ProductImage> list = new List<ProductImage>();
                var dem = 0;
                foreach (var item in request.ImageURL)
                {
                    dem++;
                    var x = new ProductImage()
                    {
                        ImagePath = item,
                        Caption = request.Name,
                        DateCreated = DateTime.Now,
                        FileSize = item.Length,
                        IsDefault = true,
                        SortOrder = dem,
                    };
                    list.Add(x);
                }
            }
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return product.Id;
        }

        public Task DeleteProduct()
        {
            throw new NotImplementedException();
        }

        public async Task<PageResult<ProductViewModel>> GetAll()
        {
            // 1.Select join
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId
                        join c in _context.Categories on pic.CategoryId equals c.Id
                        select new { p, pt, pic };

            // 3 .Paging
            int totalRow = await query.CountAsync();
            var data = await query.Skip((1 - 1) * 0)
                .Take(10)
                .Select(x => new ProductViewModel()
                {
                    Id = x.p.Id,
                    Price = x.p.Price,
                    OriginalPrice = x.p.OriginalPrice,
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
            var pageResult = new PageResult<ProductViewModel>()
            {
                TotalRecord = totalRow,
                Items = data
            };

            return pageResult;
        }

        public async Task<PageResult<ProductViewModel>> GetAllPaging(GetManageProductPagingRequest request)
        {
            // 1.Select join
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId
                        join c in _context.Categories on pic.CategoryId equals c.Id
                        select new { p, pt, pic };
            // 2. Filter
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.pt.Name.Contains(request.Keyword));
            }
            if (request.CategoryIds.Count > 0)
            {
                query = query.Where(x => request.CategoryIds.Contains(x.pic.CategoryId));
            }
            // 3 .Paging
            int totalRow = await query.CountAsync();
            var data = await query.Skip((request.pageIndex - 1) * request.pageSize)
                .Take(request.pageSize)
                .Select(x => new ProductViewModel()
                {
                    Id = x.p.Id,
                    Price = x.p.Price,
                    OriginalPrice = x.p.OriginalPrice,
                    ViewCount = x.p.ViewCount,
                    DateCreated = x.p.DateCreated,
                    Name = x.pt.Name,
                    Description = x.pt.Description,
                    Details = x.pt.Details,
                    SeoDescription = x.pt.SeoDescription,
                    SeoTitle = x.pt.SeoTitle,
                    SeoAlias = x.pt.SeoAlias,
                    LanguageId = x.pt.LanguageId,
                }).ToListAsync();
            // 4 Select Page Result
            var pageResult = new PageResult<ProductViewModel>()
            {
                TotalRecord = totalRow,
                Items = data
            };

            return pageResult;
        }

        public Task GetImageId()
        {
            throw new NotImplementedException();
        }

        public Task GetListImage()
        {
            throw new NotImplementedException();
        }

        public Task GetProductById()
        {
            throw new NotImplementedException();
        }

        public async Task<List<Size_Color>> GetSize_Color(int productId)
        {
            List<Size_Color> list = new List<Size_Color>();
            var query = from p in _context.PCSs
                        join c in _context.Colors on p.ColorId equals c.Id
                        join s in _context.Sizes on p.SizeId equals s.Id
                        where p.ProductId == productId
                        select new { p, c, s };

            foreach (var pcs in query)
            {
                var x = new Size_Color()
                {
                    ColorId = pcs.p.ColorId,
                    SizeId = pcs.p.ProductId,
                    Mamau = pcs.c.Mamau,
                    Tenmau = pcs.c.Name,
                    Size = pcs.s.Name,
                    Stock = pcs.p.Stock
                };
                list.Add(x);
            }
            return list;
        }

        public Task RemoveImage()
        {
            throw new NotImplementedException();
        }

        public Task UpdateImage()
        {
            throw new NotImplementedException();
        }

        public Task UpdatePrice()
        {
            throw new NotImplementedException();
        }

        public Task UpdateProduct()
        {
            throw new NotImplementedException();
        }

        public Task UpdateStock()
        {
            throw new NotImplementedException();
        }

        public async Task<PageResult<ProductViewModel>> GetAllByCategoryId(string languageId, GetPublicProductPagingRequest request)
        {
            // 1. Select Join
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId
                        join c in _context.Categories on pic.CategoryId equals c.Id
                        where pt.LanguageId == languageId
                        select new { p, pt, pic };

            // 2. Filter
            if (request.CategoryId.Value > 0 && request.CategoryId.HasValue)
            {
                query = query.Where(x => x.pic.CategoryId == request.CategoryId);
            }

            // Paging
            int totalRow = await query.CountAsync();
            var data = await query.Skip((request.pageIndex - 1) * request.pageSize)
                .Take(request.pageSize)
                .Select(x => new ProductViewModel()
                {
                    Id = x.p.Id,
                    Price = x.p.Price,
                    OriginalPrice = x.p.OriginalPrice,
                    ViewCount = x.p.ViewCount,
                    DateCreated = x.p.DateCreated,
                    Name = x.pt.Name,
                    Description = x.pt.Description,
                    Details = x.pt.Details,
                    SeoDescription = x.pt.SeoDescription,
                    SeoTitle = x.pt.SeoTitle,
                    SeoAlias = x.pt.SeoAlias,
                    LanguageId = x.pt.LanguageId,
                }).ToListAsync();
            // 4 Select Page Result
            var pageResult = new PageResult<ProductViewModel>()
            {
                TotalRecord = totalRow,
                Items = data
            };

            return pageResult;
        }
    }
}