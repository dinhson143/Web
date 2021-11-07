using Firebase.Auth;
using Firebase.Storage;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
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
using Web.ViewModels.Catalog.PhieuNhaps;
using Web.ViewModels.Catalog.Products;
using Web.ViewModels.Catalog.Promotions;
using Web.ViewModels.Catalog.Sizes;

namespace Web.Application.Catalog.Products
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        private readonly IHostingEnvironment _env;
        private readonly UserManager<Data.Entities.User> _userManager;
        private static string ApiKey = "AIzaSyDsIhxUtoEuX-GsYhTCd3T6tSUr2VA2MiA";
        private static string Bucket = "bds-asp-mvc.appspot.com";
        private static string AuthEmail = "dinhson14399@gmail.com";
        private static string AuthPassword = "tranthingocyen";

        public ProductService(AppDbContext context, IHostingEnvironment env, UserManager<Data.Entities.User> userManager)
        {
            _context = context;
            _env = env;
            _userManager = userManager;
        }

        public async Task<ResultApi<int>> AddImage(ProductUpdateRequest request)
        {
            var product = await _context.Products.FindAsync(request.Id);

            var productTranslations = await _context.ProductTranslations.FirstOrDefaultAsync(x => x.ProductId == request.Id
             && x.LanguageId == request.LanguageId);

            if (product == null || productTranslations == null) throw new WebException($"Cannot find a product with id: {request.Id}");

            //Save image
            List<ProductImage> list = new List<ProductImage>();
            if (request.ImageURL != null && request.ImageURL.Length > 0)
            {
                var dem = 0;
                string folderName = "firebaseFiles";
                string path = Path.Combine(_env.WebRootPath, $"images/{folderName}");
                // firebase uploading
                var auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
                var a = await auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);
                foreach (var item in request.ImageURL)
                {
                    //FileStream fs = null;
                    //// upload file to firebase
                    //if (Directory.Exists(path))
                    //{
                    //    using (fs = new FileStream(Path.Combine(path, item.FileName), FileMode.Create))
                    //    {
                    //        await item.CopyToAsync(fs);
                    //    }

                    //    fs = new FileStream(Path.Combine(path, item.FileName), FileMode.Open);
                    //}
                    //else
                    //{
                    //    Directory.CreateDirectory(path);
                    //}
                    var fs = item.OpenReadStream();

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
                            Caption = productTranslations.Name,
                            DateCreated = DateTime.Now,
                            FileSize = item.Length,
                            IsDefault = false,
                            SortOrder = dem,
                            ProductId = product.Id
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
            foreach (var item in list)
            {
                await _context.ProductImages.AddAsync(item);
            }
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return new ResultSuccessApi<int>(result);
            }
            return new ResultErrorApi<int>(result.ToString());
        }

        public async Task AddSize_Color(int productId, ProductSizeViewModel request)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new WebException($"Can not find a Product: {productId}");

            //product.PCS = new List<Product_Size>()
            //{
            //    new Product_Size()
            //    {
            //        Stock = 0,
            //        SizeId = request.SizeId
            //    }
            //};
        }

        public async Task<int> AddViewCount(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            product.ViewCount += 1;
            return await _context.SaveChangesAsync();
        }

        public async Task<ResultApi<string>> CreateProduct(ProductCreate request)
        {
            List<ProductImage> list = new List<ProductImage>();
            var languages = _context.Languages;
            var tranlations = new List<ProductTranslation>();
            foreach (var language in languages)
            {
                if (language.Id == request.LanguageId)
                {
                    tranlations.Add(new ProductTranslation()
                    {
                        Name = request.Name,
                        Description = request.Description,
                        Details = request.Details,
                        SeoDescription = request.SeoDescription,
                        SeoAlias = request.SeoAlias,
                        SeoTitle = request.SeoTitle,
                        LanguageId = request.LanguageId
                    });
                }
                else
                {
                    tranlations.Add(new ProductTranslation()
                    {
                        Name = "N/A",
                        SeoAlias = "N/A",
                        LanguageId = language.Id
                    });
                }
            }
            var product = new Product()
            {
                //Price = request.Price,
                //OriginalPrice = request.OriginalPrice,
                //Stock = request.Stock,
                ViewCount = 0,
                DateCreated = DateTime.Now,
                Status = Status.Active,
                ProductTranslations = tranlations
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
                    //FileStream fs = null;
                    //// upload file to firebase
                    //if (Directory.Exists(path))
                    //{
                    //    using (fs = new FileStream(Path.Combine(path, item.FileName), FileMode.Create))
                    //    {
                    //        await item.CopyToAsync(fs);
                    //    }

                    //    fs = new FileStream(Path.Combine(path, item.FileName), FileMode.Open);
                    //}
                    //else
                    //{
                    //    Directory.CreateDirectory(path);
                    //}
                    var fs = item.OpenReadStream();
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
                        if (dem == 1)
                        {
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
                        else
                        {
                            var x = new ProductImage()
                            {
                                ImagePath = linkImg,
                                Caption = request.Name,
                                DateCreated = DateTime.Now,
                                FileSize = item.Length,
                                IsDefault = false,
                                SortOrder = dem,
                            };
                            list.Add(x);
                        }
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

        public async Task<ResultApi<List<ProductImagesModel>>> GetListImage(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            var images = from pi in _context.ProductImages
                         where pi.ProductId == productId
                         select pi;
            var data = await images.Select(x => new ProductImagesModel()
            {
                URL = x.ImagePath,
                isDefault = x.IsDefault,
                Caption = x.Caption,
                Id = x.Id
            }).ToListAsync();
            return new ResultSuccessApi<List<ProductImagesModel>>(data);
        }

        public async Task<ResultApi<ProductViewModel>> GetProductById(int productId, string languageId)
        {
            var product = await _context.Products.FindAsync(productId);
            var productTranslation = await _context.ProductTranslations.FirstOrDefaultAsync(x => x.ProductId == productId
            && x.LanguageId == languageId);
            //
            var categories = await (from c in _context.Categories
                                    join ct in _context.CategoryTranslations on c.Id equals ct.CategoryId
                                    join pc in _context.ProductInCategories on c.Id equals pc.CategoryId
                                    where pc.ProductId == productId && ct.LanguageId == languageId
                                    select ct).ToListAsync();
            var categoryName = new List<string>();
            foreach (var item in categories)
            {
                categoryName.Add(item.Name);
            }
            //
            var sizes = await (from s in _context.Sizes
                               join ps in _context.PCSs on s.Id equals ps.SizeId
                               where ps.ProductId == productId
                               select s.Name).ToListAsync();
            //
            var danhgia = await (from c in _context.Comments
                                 where c.ProductId == productId
                                 select c).ToListAsync();

            decimal diem = 0;
            var dem = 0;
            foreach (var item in danhgia)
            {
                diem += item.Star;
                dem++;
            }
            if (dem != 0)
            {
                decimal diemDM = diem / dem;
                diem = Math.Round(diemDM);
            }
            //
            List<ProductSizeViewModel> list = new List<ProductSizeViewModel>();
            var query = from p in _context.PCSs
                        join s in _context.Sizes on p.SizeId equals s.Id
                        where p.ProductId == productId
                        select new { p, s };
            foreach (var pcs in query)
            {
                var x = new ProductSizeViewModel()
                {
                    Size = pcs.s.Name,
                    SizeId = pcs.p.SizeId,
                    OriginalPrice = pcs.p.OriginalPrice,
                    Price = pcs.p.Price,
                    Stock = pcs.p.Stock
                };
                list.Add(x);
            }
            //
            var data = new ProductViewModel()
            {
                Id = product.Id,
                ViewCount = product.ViewCount,
                DateCreated = product.DateCreated,
                Name = productTranslation.Name,
                Description = productTranslation.Description,
                Details = productTranslation.Details,
                SeoDescription = productTranslation.SeoDescription,
                SeoTitle = productTranslation.SeoTitle,
                SeoAlias = productTranslation.SeoAlias,
                LanguageId = productTranslation.LanguageId,
                Categories = categoryName,
                Sizes = sizes,
                IsFeatured = product.IsFeatured,
                listPS = list,
                Diem = diem
            };
            //get khuyến mãi
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
                if ((dn.Ticks > item.FromDate.Ticks) && (dn.Ticks < item.ToDate.Ticks))
                {
                    string[] products = item.ProductIDs.Split(',');
                    foreach (var id in products)
                    {
                        if (id != "" && data.Id == Int32.Parse(id))
                        {
                            data.DiscountPercent = item.DiscountPercent;
                            data.DiscountAmount = item.DiscountAmount;
                            break;
                        }
                    }
                }
            }
            foreach (var item in listPromotion)
            {
                if ((dn.Ticks > item.FromDate.Ticks) && (dn.Ticks < item.ToDate.Ticks))
                {
                    string[] categoriesID = item.ProductCategoryIds.Split(',');
                    foreach (var id in categoriesID)
                    {
                        if (id != "")
                        {
                            var kiemtra = (from p in _context.Products
                                           join pic in _context.ProductInCategories on p.Id equals pic.ProductId
                                           where p.Status == Status.Active && (Int32.Parse(id) == pic.CategoryId) && p.Id == data.Id
                                           select new { p });
                            int totalKiemtra = await kiemtra.CountAsync();
                            if (totalKiemtra > 0)
                            {
                                if (data.DiscountPercent == null) data.DiscountPercent = item.DiscountPercent;
                                if (data.DiscountAmount != null) data.DiscountAmount = item.DiscountAmount;
                            }
                        }
                    }
                }
            }

            return new ResultSuccessApi<ProductViewModel>(data);
        }

        public List<ProductSizeViewModel> GetProductSize(int ProductId)
        {
            List<ProductSizeViewModel> list = new List<ProductSizeViewModel>();
            var query = from p in _context.PCSs
                        join s in _context.Sizes on p.SizeId equals s.Id
                        where p.ProductId == ProductId
                        select new { p, s };

            foreach (var pcs in query)
            {
                var x = new ProductSizeViewModel()
                {
                    Size = pcs.s.Name,
                    SizeId = pcs.p.SizeId,
                    OriginalPrice = pcs.p.OriginalPrice,
                    Price = pcs.p.Price
                };
                list.Add(x);
            }
            return list;
        }

        public async Task<int> RemoveImage(int id)
        {
            var image = await _context.ProductImages.FindAsync(id);
            if (image == null) throw new WebException($"Cannot find a image: {id}");

            _context.ProductImages.Remove(image);

            return await _context.SaveChangesAsync();
        }

        public Task UpdateImage()
        {
            throw new NotImplementedException();
        }

        public async Task<ResultApi<string>> UpdatePrice(UpdatePriceRequest request)
        {
            var pcs = await _context.PCSs.FirstOrDefaultAsync(x => x.ProductId == request.ProductId
           && x.SizeId == request.SizeId);

            pcs.Price = request.Price;
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return new ResultSuccessApi<string>("Cập nhật giá thành công");
            }
            return new ResultErrorApi<string>("Cập nhật giá thất bại");
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
                         join ps in _context.PCSs on p.Id equals ps.ProductId into pps
                         from ps in pps.DefaultIfEmpty()
                         where pt.LanguageId == request.LanguageId && p.Status == Status.Active && (pi.IsDefault == true || pi == null)
                         select new { p, pt, pic, pi, ps });

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
            foreach (var product in data)
            {
                var list = new List<ProductSizeViewModel>();
                var result = from p in _context.PCSs
                             join s in _context.Sizes on p.SizeId equals s.Id
                             where p.ProductId == product.Id
                             select new { p, s };
                foreach (var pcs in result)
                {
                    var x = new ProductSizeViewModel()
                    {
                        Size = pcs.s.Name,
                        SizeId = pcs.p.SizeId,
                        OriginalPrice = pcs.p.OriginalPrice,
                        Price = pcs.p.Price,
                        Stock = pcs.p.Stock
                    };
                    list.Add(x);
                }
                product.listPS = list;
            }
            //get khuyến mãi
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
                if ((dn.Ticks > item.FromDate.Ticks) && (dn.Ticks < item.ToDate.Ticks))
                {
                    string[] products = item.ProductIDs.Split(',');
                    foreach (var product in data)
                    {
                        foreach (var id in products)
                        {
                            if (id != "" && product.Id == Int32.Parse(id))
                            {
                                product.DiscountPercent = item.DiscountPercent;
                                product.DiscountAmount = item.DiscountAmount;
                                break;
                            }
                        }
                    }
                }
            }

            foreach (var item in listPromotion)
            {
                if ((dn.Ticks > item.FromDate.Ticks) && (dn.Ticks < item.ToDate.Ticks))
                {
                    string[] categories = item.ProductCategoryIds.Split(',');
                    foreach (var product in data)
                    {
                        foreach (var id in categories)
                        {
                            if (id != "")
                            {
                                var kiemtra = (from p in _context.Products
                                               join pic in _context.ProductInCategories on p.Id equals pic.ProductId
                                               where p.Status == Status.Active && (Int32.Parse(id) == pic.CategoryId) && p.Id == product.Id
                                               select new { p });
                                int totalKiemtra = await kiemtra.CountAsync();
                                if (totalKiemtra > 0)
                                {
                                    if (product.DiscountPercent == null) product.DiscountPercent = item.DiscountPercent;
                                    if (product.DiscountAmount != null) product.DiscountAmount = item.DiscountAmount;
                                }
                            }
                        }
                    }
                }
            }
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
            //

            // 3 .Paging
            int totalRow = await query.CountAsync();
            var data = await query.OrderByDescending(x => x.p.DateCreated).Take(soluong).Select(x => new ProductViewModel()
            {
                Id = x.p.Id,
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

            foreach (var product in data)
            {
                var list = new List<ProductSizeViewModel>();
                var result = from p in _context.PCSs
                             join s in _context.Sizes on p.SizeId equals s.Id
                             where p.ProductId == product.Id
                             select new { p, s };
                foreach (var pcs in result)
                {
                    var x = new ProductSizeViewModel()
                    {
                        Size = pcs.s.Name,
                        SizeId = pcs.p.SizeId,
                        OriginalPrice = pcs.p.OriginalPrice,
                        Price = pcs.p.Price,
                        Stock = pcs.p.Stock
                    };
                    list.Add(x);
                }
                product.listPS = list;
            }

            //get khuyến mãi
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
                if ((dn.Ticks > item.FromDate.Ticks) && (dn.Ticks < item.ToDate.Ticks))
                {
                    string[] products = item.ProductIDs.Split(',');
                    foreach (var product in data)
                    {
                        foreach (var id in products)
                        {
                            if (id != "" && product.Id == Int32.Parse(id))
                            {
                                product.DiscountPercent = item.DiscountPercent;
                                product.DiscountAmount = item.DiscountAmount;
                                break;
                            }
                        }
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

            foreach (var product in data)
            {
                var list = new List<ProductSizeViewModel>();
                var result = from p in _context.PCSs
                             join s in _context.Sizes on p.SizeId equals s.Id
                             where p.ProductId == product.Id
                             select new { p, s };
                foreach (var pcs in result)
                {
                    var x = new ProductSizeViewModel()
                    {
                        Size = pcs.s.Name,
                        SizeId = pcs.p.SizeId,
                        OriginalPrice = pcs.p.OriginalPrice,
                        Price = pcs.p.Price,
                        Stock = pcs.p.Stock
                    };
                    list.Add(x);
                }
                product.listPS = list;
            }

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
                if ((dn.Ticks > item.FromDate.Ticks) && (dn.Ticks < item.ToDate.Ticks))
                {
                    string[] products = item.ProductIDs.Split(',');
                    foreach (var product in data)
                    {
                        foreach (var id in products)
                        {
                            if (id != "" && product.Id == Int32.Parse(id))
                            {
                                product.DiscountPercent = item.DiscountPercent;
                                product.DiscountAmount = item.DiscountAmount;
                                break;
                            }
                        }
                    }
                }
            }

            foreach (var item in listPromotion)
            {
                if ((dn.Ticks > item.FromDate.Ticks) && (dn.Ticks < item.ToDate.Ticks))
                {
                    string[] categories = item.ProductCategoryIds.Split(',');
                    foreach (var product in data)
                    {
                        foreach (var id in categories)
                        {
                            if (id != "")
                            {
                                var kiemtra = (from p in _context.Products
                                               join pic in _context.ProductInCategories on p.Id equals pic.ProductId
                                               where p.Status == Status.Active && (Int32.Parse(id) == pic.CategoryId) && p.Id == product.Id
                                               select new { p });
                                int totalKiemtra = await kiemtra.CountAsync();
                                if (totalKiemtra > 0)
                                {
                                    if (product.DiscountPercent == null) product.DiscountPercent = item.DiscountPercent;
                                    if (product.DiscountAmount != null) product.DiscountAmount = item.DiscountAmount;
                                }
                            }
                        }
                    }
                }
            }

            return new List<ProductViewModel>(data);
        }

        public async Task<List<ProductViewModel>> GetProductsOrderMax(string languageId, int soluong)
        {
            // 1.Select join
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId into ppic
                        from pic in ppic.DefaultIfEmpty()
                        join pi in _context.ProductImages on p.Id equals pi.ProductId into ppi
                        from pi in ppi.DefaultIfEmpty()
                        join po in _context.OrderDetails on p.Id equals po.ProductId
                        where pt.LanguageId == languageId && p.Status == Status.Active && (pi.IsDefault == true || pi == null)
                        select new { p, pt, pic, pi, po };
            //

            // 3 .Paging
            int totalRow = await query.CountAsync();
            var data = await query.OrderByDescending(x => x.po.Quantity).Select(x => new ProductViewModel()
            {
                Id = x.p.Id,
                ViewCount = x.p.ViewCount,
                DateCreated = x.p.DateCreated,
                Name = x.pt.Name,
                Description = x.pt.Description,
                Details = x.pt.Details,
                SeoDescription = x.pt.SeoDescription,
                SeoTitle = x.pt.SeoTitle,
                SeoAlias = x.pt.SeoAlias,
                LanguageId = x.pt.LanguageId,
                Image = x.pi.ImagePath,
                SluongDaban = x.po.Quantity
            }).ToListAsync();

            for (var i = 0; i < data.Count; i++)
            {
                for (var j = i + 1; j < data.Count; j++)
                {
                    if (data[i].Id == data[j].Id)
                    {
                        data[i].SluongDaban += data[j].SluongDaban;
                        data.Remove(data[j]);
                    }
                }
            }

            data = data.OrderBy(o => o.SluongDaban).Take(2).ToList();

            foreach (var product in data)
            {
                var list = new List<ProductSizeViewModel>();
                var result = from p in _context.PCSs
                             join s in _context.Sizes on p.SizeId equals s.Id
                             where p.ProductId == product.Id
                             select new { p, s };
                foreach (var pcs in result)
                {
                    var x = new ProductSizeViewModel()
                    {
                        Size = pcs.s.Name,
                        SizeId = pcs.p.SizeId,
                        OriginalPrice = pcs.p.OriginalPrice,
                        Price = pcs.p.Price,
                        Stock = pcs.p.Stock
                    };
                    list.Add(x);
                }
                product.listPS = list;
            }

            return new List<ProductViewModel>(data);
        }

        public async Task<int> Update(ProductUpdateRequest request)
        {
            var product = await _context.Products.FindAsync(request.Id);
            var productTranslations = await _context.ProductTranslations.FirstOrDefaultAsync(x => x.ProductId == request.Id
            && x.LanguageId == request.LanguageId);

            if (product == null || productTranslations == null) throw new WebException($"Cannot find a product with id: {request.Id}");

            productTranslations.Name = request.Name;
            productTranslations.SeoAlias = request.SeoAlias;
            productTranslations.SeoDescription = request.SeoDescription;
            productTranslations.SeoTitle = request.SeoTitle;
            productTranslations.Description = request.Description;
            productTranslations.Details = request.Details;
            product.IsFeatured = request.IsFeatured;
            //Save image
            //List<ProductImage> list = new List<ProductImage>();
            //if (request.ImageURL != null && request.ImageURL.Length > 0)
            //{
            //    var dem = 0;
            //    string folderName = "firebaseFiles";
            //    string path = Path.Combine(_env.WebRootPath, $"images/{folderName}");
            //    // firebase uploading
            //    var auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
            //    var a = await auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);
            //    foreach (var item in request.ImageURL)
            //    {
            //        FileStream fs = null;
            //        // upload file to firebase
            //        if (Directory.Exists(path))
            //        {
            //            using (fs = new FileStream(Path.Combine(path, item.FileName), FileMode.Create))
            //            {
            //                await item.CopyToAsync(fs);
            //            }

            //            fs = new FileStream(Path.Combine(path, item.FileName), FileMode.Open);
            //        }
            //        else
            //        {
            //            Directory.CreateDirectory(path);
            //        }

            //        // Cacellation token
            //        var cancellation = new CancellationTokenSource();
            //        var upload = await new FirebaseStorage(
            //                Bucket,
            //                new FirebaseStorageOptions
            //                {
            //                    AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
            //                    ThrowOnCancel = true
            //                }
            //            )
            //            .Child("assets")
            //            .Child($"{item.FileName}.{Path.GetExtension(item.FileName).Substring(1)}")
            //            .PutAsync(fs, cancellation.Token);
            //        try
            //        {
            //            var linkImg = upload;
            //            dem++;
            //            var x = new ProductImage()
            //            {
            //                ImagePath = linkImg,
            //                Caption = request.Name,
            //                DateCreated = DateTime.Now,
            //                FileSize = item.Length,
            //                IsDefault = false,
            //                SortOrder = dem,
            //            };
            //            list.Add(x);
            //        }
            //        catch (Exception ex)
            //        {
            //            Debug.WriteLine(ex);
            //            throw;
            //        }
            //    }
            //    product.ProductImages = list;
            //}

            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteProduct(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new WebException($"Cannot find a product: {productId}");

            _context.Products.Remove(product);

            return await _context.SaveChangesAsync();
        }

        public async Task<ResultApi<bool>> AssignSize(int productId, SizeAssignRequest request)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                return new ResultErrorApi<bool>($"Sản phẩm với id {productId} không tồn tại");
            }
            foreach (var size in request.Sizes)
            {
                var pcs = await _context.PCSs
                    .FirstOrDefaultAsync(x => x.SizeId == int.Parse(size.Id)
                    && x.ProductId == productId);
                if (pcs != null && size.Selected == false)
                {
                    _context.PCSs.Remove(pcs);
                }
                else if (pcs == null && size.Selected)
                {
                    await _context.PCSs.AddAsync(new Product_Size()
                    {
                        SizeId = int.Parse(size.Id),
                        ProductId = productId
                    });
                }
            }
            await _context.SaveChangesAsync();
            return new ResultSuccessApi<bool>();
        }

        public async Task<ResultApi<string>> CreateProductFavorite(ProductFVCreate request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return null;
            }
            var kiemtra = await _context.ProductFavorites.FirstOrDefaultAsync(x => x.ProductId == request.ProductId && x.UserId == user.Id);
            if (kiemtra != null && kiemtra.Status == Status.Active)
            {
                kiemtra.Status = Status.InActive;
            }
            else if (kiemtra != null && kiemtra.Status == Status.InActive)
            {
                kiemtra.Status = Status.Active;
            }
            else if (kiemtra == null)
            {
                var x = new ProductFavorite()
                {
                    ProductId = request.ProductId,
                    UserId = user.Id,
                    Status = Status.Active
                };

                await _context.ProductFavorites.AddAsync(x);
            }
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return new ResultSuccessApi<string>("Cập nhật danh sách yêu thích thành công");
            }
            return new ResultErrorApi<string>("Cập nhật danh sách yêu thích thất bại");
        }

        public async Task<List<ProductFavoriteViewModel>> GetProductFavorite(ProductFVrequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return null;
            }
            var listFv = from c in _context.ProductFavorites
                         join ct in _context.ProductTranslations on c.ProductId equals ct.ProductId
                         join ci in _context.ProductImages on c.ProductId equals ci.ProductId
                         where c.UserId == user.Id && ct.LanguageId == request.LanguageID && c.Status == Status.Active
                         select new { c, ct, ci };

            return await listFv.Select(x => new ProductFavoriteViewModel()
            {
                TenProduct = x.ct.Name,
                Description = x.ct.Description,
                URL = x.ci.ImagePath,
                ProductId = x.c.ProductId
            }).ToListAsync();
        }

        public async Task<int> DeleteProductFV(Guid userId, int productId)
        {
            var productFv = await _context.ProductFavorites.FirstOrDefaultAsync(x => x.ProductId == productId && x.UserId == userId);
            if (productFv == null) throw new WebException($"Cannot find: {productId}");

            _context.ProductFavorites.Remove(productFv);

            return await _context.SaveChangesAsync();
        }

        public async Task<ResultApi<List<ProductViewModel>>> GetProductLQ(int productId, string languageId)
        {
            //
            var categories = await (from c in _context.Categories
                                    join ct in _context.CategoryTranslations on c.Id equals ct.CategoryId
                                    join pc in _context.ProductInCategories on c.Id equals pc.CategoryId
                                    where pc.ProductId == productId && ct.LanguageId == languageId
                                    select c).ToListAsync();

            var category = categories[0].Id;

            var product = from p in _context.Products
                          join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                          join pic in _context.ProductInCategories on p.Id equals pic.ProductId into ppic
                          from pic in ppic.DefaultIfEmpty()
                          join c in _context.Categories on pic.CategoryId equals c.Id into picc
                          from c in picc.DefaultIfEmpty()
                          where c.Id == category && p.Id != productId
                          select new { p, pt, pic };
            //
            var listSPLQ = await product.Select(x => new ProductViewModel()
            {
                Id = x.p.Id,
                ViewCount = x.p.ViewCount,
                DateCreated = x.p.DateCreated,
                Name = x.pt.Name,
                Description = x.pt.Description,
                Details = x.pt.Details,
                SeoDescription = x.pt.SeoDescription,
                SeoTitle = x.pt.SeoTitle,
                SeoAlias = x.pt.SeoAlias,
            }).ToListAsync();
            // remove phần tử trùng
            for (var i = 0; i < listSPLQ.Count; i++)
            {
                for (var j = i + 1; j < listSPLQ.Count; j++)
                {
                    if (listSPLQ[i].Id == listSPLQ[j].Id)
                    {
                        listSPLQ.Remove(listSPLQ[j]);
                    }
                }
            }
            //
            foreach (var item in listSPLQ)
            {
                List<ProductSizeViewModel> list = new List<ProductSizeViewModel>();
                var query = from p in _context.PCSs
                            join s in _context.Sizes on p.SizeId equals s.Id
                            where p.ProductId == item.Id
                            select new { p, s };
                foreach (var pcs in query)
                {
                    var x = new ProductSizeViewModel()
                    {
                        Size = pcs.s.Name,
                        SizeId = pcs.p.SizeId,
                        OriginalPrice = pcs.p.OriginalPrice,
                        Price = pcs.p.Price,
                        Stock = pcs.p.Stock
                    };
                    list.Add(x);
                }
                item.listPS = list;
            }
            foreach (var item in listSPLQ)
            {
                var images = from pi in _context.ProductImages
                             where pi.ProductId == item.Id
                             select pi;
                var data = await images.Select(x => new ProductImagesModel()
                {
                    URL = x.ImagePath,
                    isDefault = x.IsDefault,
                    Caption = x.Caption
                }).ToListAsync();

                item.Images = data;
            }
            //get khuyến mãi
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
                if ((dn.Ticks > item.FromDate.Ticks) && (dn.Ticks < item.ToDate.Ticks))
                {
                    string[] products = item.ProductIDs.Split(',');
                    foreach (var productKT in listSPLQ)
                    {
                        foreach (var id in products)
                        {
                            if (id != "" && productKT.Id == Int32.Parse(id))
                            {
                                productKT.DiscountPercent = item.DiscountPercent;
                                productKT.DiscountAmount = item.DiscountAmount;
                                break;
                            }
                        }
                    }
                }
            }
            foreach (var item in listPromotion)
            {
                if ((dn.Ticks > item.FromDate.Ticks) && (dn.Ticks < item.ToDate.Ticks))
                {
                    string[] categoriesID = item.ProductCategoryIds.Split(',');
                    foreach (var productOB in listSPLQ)
                    {
                        foreach (var id in categoriesID)
                        {
                            if (id != "")
                            {
                                var kiemtra = (from p in _context.Products
                                               join pic in _context.ProductInCategories on p.Id equals pic.ProductId
                                               where p.Status == Status.Active && (Int32.Parse(id) == pic.CategoryId) && p.Id == productOB.Id
                                               select new { p });
                                int totalKiemtra = await kiemtra.CountAsync();
                                if (totalKiemtra > 0)
                                {
                                    if (productOB.DiscountPercent == null) productOB.DiscountPercent = item.DiscountPercent;
                                    if (productOB.DiscountAmount != null) productOB.DiscountAmount = item.DiscountAmount;
                                }
                            }
                        }
                    }
                }
            }
            return new ResultSuccessApi<List<ProductViewModel>>(listSPLQ);
        }
    }
}