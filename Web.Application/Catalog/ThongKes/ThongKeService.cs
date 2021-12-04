using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Data.EF;
using Web.Data.Enums;
using Web.ViewModels.Catalog.Common;
using Web.ViewModels.Catalog.Orders;
using Web.ViewModels.Catalog.PCSs;
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

        public async Task<ResultApi<List<OrderViewModel>>> DoanhThu(string from, string to, string languageId)
        {
            var dateF = DateTime.Parse(from);
            var dateT = DateTime.Parse(to);
            // get list order
            var query = from o in _context.Orders
                        where o.Status == OrderStatus.Success
                        select new { o };
            var orders = await query.Select(x => new OrderViewModel()
            {
                Id = x.o.Id,
                OrderDate = x.o.OrderDate,
                ShipAddress = x.o.ShipAddress,
                ShipEmail = x.o.ShipEmail,
                ShipName = x.o.ShipName,
                ShipPhone = x.o.ShipPhoneNumber,
                Status = x.o.Status.ToString(),
                Tongtien = x.o.Tongtien
            }).ToListAsync();
            int tongOrder = await query.CountAsync();
            for (int i = 0; i < tongOrder; i++)
            {
                foreach (var item in orders)
                {
                    if (item.OrderDate.Date < dateF.Date || item.OrderDate.Date > dateT.Date)
                    {
                        orders.Remove(item);
                        break;
                    }
                }
            }
            foreach (var order in orders)
            {
                var list = new List<OrderDetailViewModel>();
                var result = from od in _context.OrderDetails
                             join pt in _context.ProductTranslations on od.ProductId equals pt.ProductId
                             join pi in _context.ProductImages on od.ProductId equals pi.ProductId
                             join ods in _context.Sizes on od.SizeId equals ods.Id
                             where od.OrderId == order.Id && pt.LanguageId == languageId && pi.IsDefault == true
                             select new { od, pt, ods, pi };
                foreach (var item in result)
                {
                    var x = new OrderDetailViewModel()
                    {
                        Price = item.od.Price,
                        ProductName = item.pt.Name,
                        Quantity = item.od.Quantity,
                        SizeName = item.ods.Name,
                        Image = item.pi.ImagePath,
                        OrderID = order.Id,
                        ProductID = item.od.ProductId,
                        SizeID = item.od.SizeId
                    };
                    list.Add(x);
                }
                order.ListOrDetail = list;
            }

            // list PCS
            var query2 = from p in _context.PCSs
                             //where o.Status != OrderStatus.Success && o.Status != OrderStatus.Canceled && o.Status != OrderStatus.Shipping
                         select new { p };
            var pcss = await query2.Select(x => new PCSViewModel()
            {
                ProductId = x.p.ProductId,
                SizeId = x.p.SizeId,
                OriginalPrice = x.p.OriginalPrice
            }).ToListAsync();

            foreach (var item in pcss)
            {
                foreach (var order in orders)
                {
                    foreach (var orderdetail in order.ListOrDetail)
                    {
                        if (orderdetail.ProductID == item.ProductId && orderdetail.SizeID == item.SizeId)
                        {
                            orderdetail.OriginalPrice = item.OriginalPrice;
                        }
                    }
                }
            }
            // tính tổng tiền theo giá gốc
            foreach (var order in orders)
            {
                decimal tongtienReal = 0;
                foreach (var orderdetail in order.ListOrDetail)
                {
                    tongtienReal += (orderdetail.OriginalPrice * orderdetail.Quantity);
                }
                order.TongtienReal = tongtienReal;
            }
            return new ResultSuccessApi<List<OrderViewModel>>(orders);
        }

        public async Task<ResultApi<List<OrderViewModel>>> DoanhThuFullMonth(string languageId)
        {
            // get list order
            var query = from o in _context.Orders
                        where o.Status == OrderStatus.Success
                        select new { o };
            var orders = await query.Select(x => new OrderViewModel()
            {
                Id = x.o.Id,
                OrderDate = x.o.OrderDate,
                ShipAddress = x.o.ShipAddress,
                ShipEmail = x.o.ShipEmail,
                ShipName = x.o.ShipName,
                ShipPhone = x.o.ShipPhoneNumber,
                Status = x.o.Status.ToString(),
                Tongtien = x.o.Tongtien
            }).ToListAsync();
            foreach (var order in orders)
            {
                var list = new List<OrderDetailViewModel>();
                var result = from od in _context.OrderDetails
                             join pt in _context.ProductTranslations on od.ProductId equals pt.ProductId
                             join pi in _context.ProductImages on od.ProductId equals pi.ProductId
                             join ods in _context.Sizes on od.SizeId equals ods.Id
                             where od.OrderId == order.Id && pt.LanguageId == languageId && pi.IsDefault == true
                             select new { od, pt, ods, pi };
                foreach (var item in result)
                {
                    var x = new OrderDetailViewModel()
                    {
                        Price = item.od.Price,
                        ProductName = item.pt.Name,
                        Quantity = item.od.Quantity,
                        SizeName = item.ods.Name,
                        Image = item.pi.ImagePath,
                        OrderID = order.Id,
                        ProductID = item.od.ProductId,
                        SizeID = item.od.SizeId
                    };
                    list.Add(x);
                }
                order.ListOrDetail = list;
            }

            // list PCS
            var query2 = from p in _context.PCSs
                             //where o.Status != OrderStatus.Success && o.Status != OrderStatus.Canceled && o.Status != OrderStatus.Shipping
                         select new { p };
            var pcss = await query2.Select(x => new PCSViewModel()
            {
                ProductId = x.p.ProductId,
                SizeId = x.p.SizeId,
                OriginalPrice = x.p.OriginalPrice
            }).ToListAsync();

            foreach (var item in pcss)
            {
                foreach (var order in orders)
                {
                    foreach (var orderdetail in order.ListOrDetail)
                    {
                        if (orderdetail.ProductID == item.ProductId && orderdetail.SizeID == item.SizeId)
                        {
                            orderdetail.OriginalPrice = item.OriginalPrice;
                        }
                    }
                }
            }
            // tính tổng tiền theo giá gốc
            foreach (var order in orders)
            {
                decimal tongtienReal = 0;
                foreach (var orderdetail in order.ListOrDetail)
                {
                    tongtienReal += (orderdetail.OriginalPrice * orderdetail.Quantity);
                }
                order.TongtienReal = tongtienReal;
            }
            return new ResultSuccessApi<List<OrderViewModel>>(orders);
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

        public async Task<ResultApi<List<ProductViewModel>>> ProductSavest(string from, string to, string languageId)
        {
            var dateF = DateTime.Parse(from);
            var dateT = DateTime.Parse(to);

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
                SluongDaban = x.po.Quantity,
                OrderID = x.po.OrderId
            }).ToListAsync();

            // get list order
            var query2 = from o in _context.Orders
                             //where o.Status != OrderStatus.Success && o.Status != OrderStatus.Canceled && o.Status != OrderStatus.Shipping
                         select new { o };
            var orders = await query2.Select(x => new OrderViewModel()
            {
                Id = x.o.Id,
                OrderDate = x.o.OrderDate,
                ShipAddress = x.o.ShipAddress,
                ShipEmail = x.o.ShipEmail,
                ShipName = x.o.ShipName,
                ShipPhone = x.o.ShipPhoneNumber,
                Status = x.o.Status.ToString()
            }).ToListAsync();
            int tongOrder = await query2.CountAsync();
            for (int i = 0; i < tongOrder; i++)
            {
                foreach (var item in orders)
                {
                    if (item.OrderDate.Date < dateF.Date || item.OrderDate.Date > dateT.Date)
                    {
                        orders.Remove(item);
                        break;
                    }
                }
            }
            var listData = new List<ProductViewModel>();
            foreach (var item in data)
            {
                foreach (var order in orders)
                {
                    if (order.Id == item.OrderID)
                    {
                        listData.Add(item);
                    }
                }
            }

            for (var i = 0; i < listData.Count; i++)
            {
                for (var j = i + 1; j < listData.Count; j++)
                {
                    if (listData[i].Id == listData[j].Id)
                    {
                        listData[i].SluongDaban += listData[j].SluongDaban;
                        listData.Remove(listData[j]);
                    }
                }
            }

            listData = listData.OrderBy(o => o.SluongDaban).ToList();
            foreach (var product in listData)
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
            foreach (var item in listData)
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
            return new ResultSuccessApi<List<ProductViewModel>>(listData);
        }

        public async Task<ResultApi<List<ProductViewModel>>> ProductSavestFullMonth(string languageId)
        {
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
                SluongDaban = x.po.Quantity,
                OrderID = x.po.OrderId
            }).ToListAsync();

            // get list order
            var query2 = from o in _context.Orders
                             //where o.Status != OrderStatus.Success && o.Status != OrderStatus.Canceled && o.Status != OrderStatus.Shipping
                         select new { o };
            var orders = await query2.Select(x => new OrderViewModel()
            {
                Id = x.o.Id,
                OrderDate = x.o.OrderDate,
                ShipAddress = x.o.ShipAddress,
                ShipEmail = x.o.ShipEmail,
                ShipName = x.o.ShipName,
                ShipPhone = x.o.ShipPhoneNumber,
                Status = x.o.Status.ToString()
            }).ToListAsync();
            var listData = new List<ProductViewModel>();
            foreach (var item in data)
            {
                foreach (var order in orders)
                {
                    if (order.Id == item.OrderID)
                    {
                        listData.Add(item);
                    }
                }
            }

            for (var i = 0; i < listData.Count; i++)
            {
                for (var j = i + 1; j < listData.Count; j++)
                {
                    if (listData[i].Id == listData[j].Id)
                    {
                        listData[i].SluongDaban += listData[j].SluongDaban;
                        listData.Remove(listData[j]);
                    }
                }
            }

            listData = listData.OrderByDescending(o => o.SluongDaban).Take(5).ToList();
            foreach (var product in listData)
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
            foreach (var item in listData)
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
            return new ResultSuccessApi<List<ProductViewModel>>(listData);
        }
    }
}