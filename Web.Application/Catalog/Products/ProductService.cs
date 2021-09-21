using Firebase.Auth;
using Firebase.Storage;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Web.Data.EF;
using Web.Data.Entities;
using Web.Data.Enums;
using Web.Utilities.Exceptions;
using Web.ViewModels.Catalog.Categories;
using Web.ViewModels.Catalog.Common;
using Web.ViewModels.Catalog.Products;

namespace Web.Application.Catalog.Products
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        private readonly IHostingEnvironment _env;
        private static string ApiKey = "AIzaSyDsIhxUtoEuX-GsYhTCd3T6tSUr2VA2MiA";
        private static string Bucket = "bds-asp-mvc.appspot.com";
        private static string AuthEmail = "dinhson14399@gmail.com";
        private static string AuthPassword = "tranthingocyen";

        public ProductService(AppDbContext context, IHostingEnvironment env)
        {
            _context = context;
            _env = env;
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

        public async Task<ResultApi<string>> CreateProduct(ProductCreate request)
        {
            List<ProductImage> list = new List<ProductImage>();
            var product = new Product()
            {
                Price = request.Price,
                OriginalPrice = request.OriginalPrice,
                //Stock = request.Stock,
                ViewCount = 0,
                DateCreated = DateTime.Now,
                Status = Status.Active,
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
                }
            };

            if (request.ImageURL.Length > 0)
            {
                var dem = 0;
                string folderName = "firebaseFiles";
                string path = Path.Combine(_env.WebRootPath, $"images/{folderName}");
                // firebase uploading
                var auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
                var a = await auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);
                foreach (var item in request.ImageURL)
                {
                    FileStream fs = null;
                    // upload file to firebase
                    if (Directory.Exists(path))
                    {
                        using (fs = new FileStream(Path.Combine(path, item.FileName), FileMode.Create))
                        {
                            await item.CopyToAsync(fs);
                        }

                        fs = new FileStream(Path.Combine(path, item.FileName), FileMode.Open);
                    }
                    else
                    {
                        Directory.CreateDirectory(path);
                    }

                    // Cacellation token
                    var cancellation = new CancellationTokenSource();
                    var upload = await new FirebaseStorage(
                            Bucket,
                            new FirebaseStorageOptions
                            {
                                AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                                ThrowOnCancel = true
                            }
                        )
                        .Child("assets")
                        .Child($"{item.FileName}.{Path.GetExtension(item.FileName).Substring(1)}")
                        .PutAsync(fs, cancellation.Token);
                    try
                    {
                        var linkImg = upload;
                        dem++;
                        var x = new ProductImage()
                        {
                            ImagePath = linkImg,
                            Caption = request.Name,
                            DateCreated = DateTime.Now,
                            FileSize = item.Length,
                            IsDefault = true,
                            SortOrder = dem,
                        };
                        list.Add(x);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                        throw;
                    }
                }
            }
            product.ProductImages = list;
            await _context.Products.AddAsync(product);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return new ResultSuccessApi<string>("Thêm sản phẩm thành công");
            }
            return new ResultErrorApi<string>("Thêm sản phẩm thất bại");
        }

        public Task DeleteProduct()
        {
            throw new NotImplementedException();
        }

        public async Task<ResultApi<PageResult<ProductViewModel>>> GetAll(GetManageProductPagingRequest request)
        {
            // 1.Select join (Left Join)
            var query = (from p in _context.Products
                         join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                         join pic in _context.ProductInCategories on p.Id equals pic.ProductId into ppic
                         from pic in ppic.DefaultIfEmpty()
                         join c in _context.Categories on pic.CategoryId equals c.Id into picc
                         from c in picc.DefaultIfEmpty()
                         where pt.LanguageId == request.LanguageId
                         select new { p, pt, pic });
            // 2. Filter
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.pt.Name.Contains(request.Keyword));
            }
            if (request.CategoryId > 0)
            {
                query = query.Where(x => x.pic.CategoryId == request.CategoryId);
            }
            // 3 .Paging
            int totalRow = await query.CountAsync();
            var data = await query.Select(x => new ProductViewModel()
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
            var pageResult = new PageResult<ProductViewModel>()
            {
                TotalRecords = totalRow,
                Items = data
            };
            return new ResultSuccessApi<PageResult<ProductViewModel>>(pageResult);
        }

        public async Task<PageResult<ProductViewModel>> GetAllPaging(GetManageProductPagingRequest request)
        {
            // 1.Select join
            var query = (from p in _context.Products
                         join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                         join pic in _context.ProductInCategories on p.Id equals pic.ProductId into ppic
                         from pic in ppic.DefaultIfEmpty()
                         join c in _context.Categories on pic.CategoryId equals c.Id into picc
                         from c in picc.DefaultIfEmpty()
                         where pt.LanguageId == request.LanguageId
                         select new { p, pt, pic });
            // 2. Filter
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.pt.Name.Contains(request.Keyword));
            }
            //if (request.CategoryIds.Count > 0)
            //{
            //    query = query.Where(x => request.CategoryIds.Contains(x.pic.CategoryId));
            //}
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
                TotalRecords = totalRow,
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

        public async Task<ResultApi<ProductViewModel>> GetProductById(int productId, string languageId)
        {
            var product = await _context.Products.FindAsync(productId);
            var productTranslation = await _context.ProductTranslations.FirstOrDefaultAsync(x => x.ProductId == productId
            && x.LanguageId == languageId);

            var categories = await (from c in _context.Categories
                                    join ct in _context.CategoryTranslations on c.Id equals ct.CategoryId
                                    join pc in _context.ProductInCategories on c.Id equals pc.CategoryId
                                    where pc.ProductId == productId && ct.LanguageId == languageId
                                    select ct.Name).ToListAsync();
            var data = new ProductViewModel()
            {
                Id = product.Id,
                Price = product.Price,
                OriginalPrice = product.OriginalPrice,
                ViewCount = product.ViewCount,
                DateCreated = product.DateCreated,
                Name = productTranslation.Name,
                Description = productTranslation.Description,
                Details = productTranslation.Details,
                SeoDescription = productTranslation.SeoDescription,
                SeoTitle = productTranslation.SeoTitle,
                SeoAlias = productTranslation.SeoAlias,
                LanguageId = productTranslation.LanguageId,
                Categories = categories
            };
            return new ResultSuccessApi<ProductViewModel>(data);
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

        public async Task<PageResult<ProductViewModel>> GetAllByCategoryId(GetPublicProductPagingRequest request)
        {
            // 1. Select Join
            var query = (from p in _context.Products
                         join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                         join pic in _context.ProductInCategories on p.Id equals pic.ProductId into ppic
                         from pic in ppic.DefaultIfEmpty()
                         join pi in _context.ProductImages on p.Id equals pi.ProductId into ppi
                         from pi in ppi.DefaultIfEmpty()
                         join c in _context.Categories on pic.CategoryId equals c.Id into picc
                         from c in picc.DefaultIfEmpty()
                         where pt.LanguageId == request.LanguageId && p.Status == Status.Active && (pi.IsDefault == true || pi == null)
                         select new { p, pt, pic, pi });

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
                    Image = x.pi.ImagePath
                }).ToListAsync();
            // 4 Select Page Result
            var pageResult = new PageResult<ProductViewModel>()
            {
                TotalRecords = totalRow,
                PageIndex = request.pageIndex,
                PageSize = request.pageSize,
                Items = data
            };

            return pageResult;
        }

        public async Task<ResultApi<bool>> AssignCategory(int productId, CategoryAssignRequest request)
        {
            var user = await _context.Products.FindAsync(productId);
            if (user == null)
            {
                return new ResultErrorApi<bool>($"Sản phẩm với id {productId} không tồn tại");
            }
            foreach (var category in request.Categories)
            {
                var productInCategory = await _context.ProductInCategories
                    .FirstOrDefaultAsync(x => x.CategoryId == int.Parse(category.Id)
                    && x.ProductId == productId);
                if (productInCategory != null && category.Selected == false)
                {
                    _context.ProductInCategories.Remove(productInCategory);
                }
                else if (productInCategory == null && category.Selected)
                {
                    await _context.ProductInCategories.AddAsync(new ProductInCategory()
                    {
                        CategoryId = int.Parse(category.Id),
                        ProductId = productId
                    });
                }
            }
            await _context.SaveChangesAsync();
            return new ResultSuccessApi<bool>();
        }

        public async Task<List<ProductViewModel>> GetFeaturedProducts(string languageId, int soluong)
        {
            // 1.Select join
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId into ppic
                        from pic in ppic.DefaultIfEmpty()
                        join pi in _context.ProductImages on p.Id equals pi.ProductId into ppi
                        from pi in ppi.DefaultIfEmpty()
                        join c in _context.Categories on pic.CategoryId equals c.Id into picc
                        from c in picc.DefaultIfEmpty()
                        where pt.LanguageId == languageId && p.Status == Status.Active && p.IsFeatured == true && (pi.IsDefault == true || pi == null)
                        select new { p, pt, pic, pi };

            // 3 .Paging
            int totalRow = await query.CountAsync();
            var data = await query.OrderByDescending(x => x.p.DateCreated).Take(soluong).Select(x => new ProductViewModel()
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
                Image = x.pi.ImagePath
            }).ToListAsync();

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
            return new List<ProductViewModel>(data);
        }

        public async Task<List<ProductViewModel>> GetLatestProducts(string languageId, int soluong)
        {
            // 1.Select join
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId into ppic
                        from pic in ppic.DefaultIfEmpty()
                        join pi in _context.ProductImages on p.Id equals pi.ProductId into ppi
                        from pi in ppi.DefaultIfEmpty()
                        join c in _context.Categories on pic.CategoryId equals c.Id into picc
                        from c in picc.DefaultIfEmpty()
                        where pt.LanguageId == languageId && (pi.IsDefault == true || pi == null)
                        select new { p, pt, pic, pi };

            // 3 .Paging
            int totalRow = await query.CountAsync();
            var data = await query.OrderByDescending(x => x.p.DateCreated).Take(soluong).Select(x => new ProductViewModel()
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
                Image = x.pi.ImagePath
            }).ToListAsync();

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
            return new List<ProductViewModel>(data);
        }
    }
}