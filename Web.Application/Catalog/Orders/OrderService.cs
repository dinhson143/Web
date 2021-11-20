using Microsoft.AspNetCore.Identity;
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
using Web.ViewModels.Catalog.Congtys;
using Web.ViewModels.Catalog.LoaiPhieus;
using Web.ViewModels.Catalog.Orders;
using Web.ViewModels.Catalog.Sales;

namespace Web.Application.Catalog.Orders
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;

        public OrderService(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<int> CancelOrder(Guid userId, int orderId)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == orderId && x.UserId == userId);
            if (order == null) throw new WebException($"Cannot find a order: {orderId}");

            var orderdetails = from od in _context.OrderDetails
                               where od.OrderId == order.Id
                               select new { od };

            var data = new List<OrderDetailViewModel>();
            foreach (var item in orderdetails)
            {
                var x = new OrderDetailViewModel()
                {
                    OrderID = item.od.OrderId,
                    Quantity = item.od.Quantity,
                    SizeID = item.od.SizeId,
                    ProductID = item.od.ProductId
                };
                data.Add(x);
            }
            foreach (var item in data)
            {
                var product = await _context.PCSs.FirstOrDefaultAsync(x => x.ProductId == item.ProductID && x.SizeId == item.SizeID);
                product.Stock += item.Quantity;
                var kt = await _context.SaveChangesAsync();
                if (kt <= 0)
                {
                    return 0;
                }
            }

            order.Status = OrderStatus.Canceled;
            return await _context.SaveChangesAsync();
        }

        public async Task<int> ConfirmOrder(int orderId)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == orderId);
            if (order == null) throw new WebException($"Cannot find a order: {orderId}");

            order.Status = OrderStatus.Confirmed;

            // tạo phiếu xuất
            var list = await _context.LoaiPhieus.Where(x => x.Status == Status.Active).Select(x => new LoaiPhieuViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                NgayNhap = x.NgayNhap
            }).ToListAsync();

            var query = from c in _context.CongTys
                        where c.Status == Status.Active
                        select new { c };
            var listCT = await query.Select(x => new CongtyViewModel()
            {
                Id = x.c.Id,
                Name = x.c.Name,
                Diachi = x.c.Diachi,
                Masothue = x.c.Masothue,
                Status = x.c.Status,
                Sdt = x.c.Sdt
            }).ToListAsync();

            var loaiphieu = new LoaiPhieuViewModel();
            foreach (var item in list)
            {
                if (String.Compare(item.Name, "Xuất") == 0)
                {
                    loaiphieu = item;
                    break;
                }
            }
            var px = new PhieuNhap_Xuat()
            {
                CongTyId = listCT[0].Id,
                LoaiPhieuId = loaiphieu.Id,
                NgayNhap = DateTime.Now,
                Status = Status.Active
            };

            await _context.PhieuNXs.AddAsync(px);
            var kq = await _context.SaveChangesAsync();
            // tạo ctpx
            if (kq > 0)
            {
                var x = await _context.Orders.FindAsync(orderId);
                var orderData = new OrderViewModel()
                {
                    Id = x.Id,
                    OrderDate = x.OrderDate,
                    ShipAddress = x.ShipAddress,
                    ShipEmail = x.ShipEmail,
                    ShipName = x.ShipName,
                    ShipPhone = x.ShipPhoneNumber,
                    Status = x.Status.ToString()
                };

                var listCTorder = new List<OrderDetailViewModel>();
                var result = from od in _context.OrderDetails
                             join pt in _context.ProductTranslations on od.ProductId equals pt.ProductId
                             join pi in _context.ProductImages on od.ProductId equals pi.ProductId
                             join ods in _context.Sizes on od.SizeId equals ods.Id
                             where od.OrderId == order.Id && pt.LanguageId == "vi" && pi.IsDefault == true
                             select new { od, pt, ods, pi };

                var listCTPNX = new List<PhieuNhap_Xuatchitiet>();
                foreach (var item in result)
                {
                    var ctpx = new PhieuNhap_Xuatchitiet()
                    {
                        Giaban = item.od.Price,
                        Dongia = 1000,
                        PhieuNXId = px.Id,
                        ProductId = item.od.ProductId,
                        SizeId = item.od.SizeId,
                        Soluong = item.od.Quantity
                    };
                    listCTPNX.Add(ctpx);
                }
                foreach (var item in listCTPNX)
                {
                    await _context.PhieuNXchitiets.AddAsync(item);
                }
            }

            return await _context.SaveChangesAsync();
        }

        public async Task<int> SuccessOrder(int orderId)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == orderId);
            if (order == null) throw new WebException($"Cannot find a order: {orderId}");
            var orderSP = await _context.OrderOfShippers.FirstOrDefaultAsync(x => x.OrderID == orderId);
            if (orderSP == null) throw new WebException($"Cannot find a order shiper: {orderId}");
            order.Status = OrderStatus.Success;
            orderSP.Status = ShipStatus.Success;
            orderSP.Date = DateTime.Now;
            return await _context.SaveChangesAsync();
        }

        public async Task<ResultApi<string>> CreateOrder(CheckoutRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return null;
            }

            var orderdetails = new List<OrderDetail>();

            foreach (var item in request.OrderDetails)
            {
                orderdetails.Add(new OrderDetail()
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    SizeId = item.SizeId
                });
                var product = await _context.PCSs.FirstOrDefaultAsync(x => x.ProductId == item.ProductId && x.SizeId == item.SizeId);
                product.Stock -= item.Quantity;
                var kt = await _context.SaveChangesAsync();
                if (kt <= 0)
                {
                    return new ResultErrorApi<string>("Tạo đơn hàng thất bại");
                }
            }
            // tính tổng tiền
            decimal tongtien = 0;
            foreach (var item in orderdetails)
            {
                tongtien += item.Price * item.Quantity;
            }
            var order = new Order()
            {
                OrderDate = DateTime.Now,
                ShipAddress = request.Address,
                ShipEmail = request.Email,
                ShipName = request.Name,
                ShipPhoneNumber = request.PhoneNumber,
                UserId = user.Id,
                Status = OrderStatus.InProgress,
                OrderDetails = orderdetails,
                ThanhToan = request.ThanhToan,
                Tongtien = tongtien
            };
            await _context.Orders.AddAsync(order);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                user.Diem += orderdetails.Count;
                var result2 = await _context.SaveChangesAsync();
                if (result2 > 0)
                {
                    return new ResultSuccessApi<string>("Tạo đơn hàng thành công");
                }
            }
            return new ResultErrorApi<string>("Tạo đơn hàng thất bại");
        }

        public async Task<List<OrderViewModel>> GetallOrder(string languageID)
        {
            var query = from o in _context.Orders
                            //where o.Status != OrderStatus.Success && o.Status != OrderStatus.Canceled && o.Status != OrderStatus.Shipping
                        select new { o };
            var data = await query.Select(x => new OrderViewModel()
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

            foreach (var order in data)
            {
                var list = new List<OrderDetailViewModel>();
                var result = from od in _context.OrderDetails
                             join pt in _context.ProductTranslations on od.ProductId equals pt.ProductId
                             join pi in _context.ProductImages on od.ProductId equals pi.ProductId
                             join ods in _context.Sizes on od.SizeId equals ods.Id
                             where od.OrderId == order.Id && pt.LanguageId == languageID && pi.IsDefault == true
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
                        OrderID = order.Id
                    };
                    list.Add(x);
                }
                order.ListOrDetail = list;
            }
            return new List<OrderViewModel>(data);
        }

        public async Task<List<OrderViewModel>> GetallOrderSuccess(string languageID)
        {
            var query = from o in _context.Orders
                        where o.Status == OrderStatus.Success
                        select new { o };
            var data = await query.Select(x => new OrderViewModel()
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

            foreach (var order in data)
            {
                var list = new List<OrderDetailViewModel>();
                var result = from od in _context.OrderDetails
                             join pt in _context.ProductTranslations on od.ProductId equals pt.ProductId
                             join pi in _context.ProductImages on od.ProductId equals pi.ProductId
                             join ods in _context.Sizes on od.SizeId equals ods.Id
                             where od.OrderId == order.Id && pt.LanguageId == languageID && pi.IsDefault == true
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
                        OrderID = order.Id
                    };
                    list.Add(x);
                }
                order.ListOrDetail = list;
            }
            return new List<OrderViewModel>(data);
        }

        public async Task<List<OrderViewModel>> GetallOrderConfirm(string languageID)
        {
            var query = from o in _context.Orders
                        where o.Status == OrderStatus.Confirmed || o.Status == OrderStatus.ShipperRequest
                        select new { o };
            var data = await query.Select(x => new OrderViewModel()
            {
                Id = x.o.Id,
                OrderDate = x.o.OrderDate,
                ShipAddress = x.o.ShipAddress,
                ShipEmail = x.o.ShipEmail,
                ShipName = x.o.ShipName,
                ShipPhone = x.o.ShipPhoneNumber,
                Status = x.o.Status.ToString(),
                Tongtien = x.o.Tongtien,
                LoaiThanhToan = x.o.ThanhToan
            }).ToListAsync();

            foreach (var order in data)
            {
                var list = new List<OrderDetailViewModel>();
                var result = from od in _context.OrderDetails
                             join pt in _context.ProductTranslations on od.ProductId equals pt.ProductId
                             join pi in _context.ProductImages on od.ProductId equals pi.ProductId
                             join ods in _context.Sizes on od.SizeId equals ods.Id
                             where od.OrderId == order.Id && pt.LanguageId == languageID && pi.IsDefault == true
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
                        OrderID = order.Id
                    };
                    list.Add(x);
                }
                order.ListOrDetail = list;
            }
            return new List<OrderViewModel>(data);
        }

        public async Task<List<OrderViewModel>> GetallOrderInProgress(string languageID)
        {
            var query = from o in _context.Orders
                        where o.Status == OrderStatus.InProgress
                        select new { o };
            var data = await query.Select(x => new OrderViewModel()
            {
                Id = x.o.Id,
                OrderDate = x.o.OrderDate,
                ShipAddress = x.o.ShipAddress,
                ShipEmail = x.o.ShipEmail,
                ShipName = x.o.ShipName,
                ShipPhone = x.o.ShipPhoneNumber,
                Status = x.o.Status.ToString(),
                Tongtien = x.o.Tongtien,
                LoaiThanhToan = x.o.ThanhToan
            }).ToListAsync();

            foreach (var order in data)
            {
                var list = new List<OrderDetailViewModel>();
                var result = from od in _context.OrderDetails
                             join pt in _context.ProductTranslations on od.ProductId equals pt.ProductId
                             join pi in _context.ProductImages on od.ProductId equals pi.ProductId
                             join ods in _context.Sizes on od.SizeId equals ods.Id
                             where od.OrderId == order.Id && pt.LanguageId == languageID && pi.IsDefault == true
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
                        OrderID = order.Id
                    };
                    list.Add(x);
                }
                order.ListOrDetail = list;
            }
            return new List<OrderViewModel>(data);
        }

        public async Task<OrderViewModel> GetOrderByID(int orderId, string languageID)
        {
            var x = await _context.Orders.FindAsync(orderId);
            var order = new OrderViewModel()
            {
                Id = x.Id,
                OrderDate = x.OrderDate,
                ShipAddress = x.ShipAddress,
                ShipEmail = x.ShipEmail,
                ShipName = x.ShipName,
                ShipPhone = x.ShipPhoneNumber,
                Status = x.Status.ToString(),
                Tongtien = x.Tongtien
            };

            var list = new List<OrderDetailViewModel>();
            var result = from od in _context.OrderDetails
                         join pt in _context.ProductTranslations on od.ProductId equals pt.ProductId
                         join pi in _context.ProductImages on od.ProductId equals pi.ProductId
                         join ods in _context.Sizes on od.SizeId equals ods.Id
                         where od.OrderId == order.Id && pt.LanguageId == languageID && pi.IsDefault == true
                         select new { od, pt, ods, pi };
            foreach (var item in result)
            {
                var oddt = new OrderDetailViewModel()
                {
                    Price = item.od.Price,
                    ProductName = item.pt.Name,
                    Quantity = item.od.Quantity,
                    SizeName = item.ods.Name,
                    Image = item.pi.ImagePath,
                    OrderID = order.Id
                };
                list.Add(oddt);
            }
            order.ListOrDetail = list;

            return order;
        }

        public async Task<List<OrderViewModel>> GetOrderUser(Guid userId, string languageID)
        {
            var query = from o in _context.Orders
                        where o.Status != OrderStatus.Canceled && o.UserId == userId && o.Status != OrderStatus.Success
                        select new { o };
            var data = await query.Select(x => new OrderViewModel()
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

            foreach (var order in data)
            {
                var list = new List<OrderDetailViewModel>();
                var result = from od in _context.OrderDetails
                             join pt in _context.ProductTranslations on od.ProductId equals pt.ProductId
                             join pi in _context.ProductImages on od.ProductId equals pi.ProductId
                             join ods in _context.Sizes on od.SizeId equals ods.Id
                             where od.OrderId == order.Id && pt.LanguageId == languageID && pi.IsDefault == true
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
                        OrderID = order.Id
                    };
                    list.Add(x);
                }
                order.ListOrDetail = list;
            }
            return new List<OrderViewModel>(data);
        }

        public async Task<List<OrderViewModel>> GetAllOrderUser(Guid userId, string languageID)
        {
            var query = from o in _context.Orders
                        where o.Status != OrderStatus.Canceled && o.UserId == userId
                        select new { o };
            var data = await query.Select(x => new OrderViewModel()
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

            foreach (var order in data)
            {
                var list = new List<OrderDetailViewModel>();
                var result = from od in _context.OrderDetails
                             join pt in _context.ProductTranslations on od.ProductId equals pt.ProductId
                             join pi in _context.ProductImages on od.ProductId equals pi.ProductId
                             join ods in _context.Sizes on od.SizeId equals ods.Id
                             where od.OrderId == order.Id && pt.LanguageId == languageID && pi.IsDefault == true
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
                        OrderID = order.Id
                    };
                    list.Add(x);
                }
                order.ListOrDetail = list;
            }
            return new List<OrderViewModel>(data);
        }

        public async Task<List<OrderViewModel>> GetOrderUserHistory(Guid userId, string languageID)
        {
            var query = from o in _context.Orders
                        where o.Status == OrderStatus.Success && o.UserId == userId
                        select new { o };
            var data = await query.Select(x => new OrderViewModel()
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

            foreach (var order in data)
            {
                var list = new List<OrderDetailViewModel>();
                var result = from od in _context.OrderDetails
                             join pt in _context.ProductTranslations on od.ProductId equals pt.ProductId
                             join pi in _context.ProductImages on od.ProductId equals pi.ProductId
                             join ods in _context.Sizes on od.SizeId equals ods.Id
                             where od.OrderId == order.Id && pt.LanguageId == languageID && pi.IsDefault == true
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
                        OrderID = order.Id
                    };
                    list.Add(x);
                }
                order.ListOrDetail = list;
            }
            return new List<OrderViewModel>(data);
        }
        
    }
}