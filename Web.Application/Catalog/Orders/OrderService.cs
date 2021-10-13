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
using Web.ViewModels.Catalog.Common;
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
                OrderDetails = orderdetails
            };
            await _context.Orders.AddAsync(order);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return new ResultSuccessApi<string>("Tạo đơn hàng thành công");
            }
            return new ResultErrorApi<string>("Tạo đơn hàng thất bại");
        }

        public async Task<List<OrderViewModel>> GetOrderUser(Guid userId, string languageID)
        {
            var query = from o in _context.Orders
                        where o.Status != OrderStatus.Success && o.Status != OrderStatus.Canceled && o.UserId == userId
                        select new { o };
            var data = await query.Select(x => new OrderViewModel()
            {
                Id = x.o.Id,
                OrderDate = x.o.OrderDate,
                ShipAddress = x.o.ShipAddress,
                ShipEmail = x.o.ShipEmail,
                ShipName = x.o.ShipName,
                ShipPhone = x.o.ShipPhoneNumber,
                Status = x.o.Status
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