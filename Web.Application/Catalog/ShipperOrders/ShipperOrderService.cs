using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Data.EF;
using System.Linq;
using Web.Data.Enums;
using Web.ViewModels.Catalog.Common;
using Web.ViewModels.Catalog.ShipperOrders;
using Web.ViewModels.Catalog.Orders;
using Microsoft.EntityFrameworkCore;

namespace Web.Application.Catalog.ShipperOrder
{
    public class ShipperOrderService : IShipperOrderService
    {
        private readonly AppDbContext _context;

        public ShipperOrderService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResultApi<string>> CreateShipperOrder(ShipperOrderCreate request)
        {
            var shipperOrder = new Web.Data.Entities.ShipperOrder()
            {
                OrderID = request.OrderID,
                ShipperId = request.ShipperId,
                Status = ShipStatus.InProgress,
            };

            await _context.ShipperOrders.AddAsync(shipperOrder);

            var order = await _context.Orders.FindAsync(request.OrderID);
            order.Status = OrderStatus.Shipping;

            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return new ResultSuccessApi<string>("Nhận đơn hàng thành công");
            }
            return new ResultErrorApi<string>("Nhận đơn hàng thất bại");
        }

        public async Task<List<OrderViewModel>> GetAll(Guid shipperId)
        {
            var listODsp = new List<OrderViewModel>();

            var qrSPOD = from os in _context.ShipperOrders
                         where os.ShipperId == shipperId
                         select os;
            var listODSP = await qrSPOD.Select(x => new ShipperOrderViewModel()
            {
                OrderID = x.OrderID
            }).ToListAsync();
            foreach (var item in listODSP)
            {
                var order = await _context.Orders.FindAsync(item.OrderID);
                var data = new OrderViewModel()
                {
                    Id = order.Id,
                    OrderDate = order.OrderDate,
                    ShipAddress = order.ShipAddress,
                    ShipEmail = order.ShipEmail,
                    ShipName = order.ShipName,
                    ShipPhone = order.ShipPhoneNumber,
                    Status = order.Status
                };
                listODsp.Add(data);
            }

            foreach (var order in listODsp)
            {
                var list = new List<OrderDetailViewModel>();
                var result = from od in _context.OrderDetails
                             join pt in _context.ProductTranslations on od.ProductId equals pt.ProductId
                             join pi in _context.ProductImages on od.ProductId equals pi.ProductId
                             join ods in _context.Sizes on od.SizeId equals ods.Id
                             where od.OrderId == order.Id && pt.LanguageId == "vi" && pi.IsDefault == true
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
            return new List<OrderViewModel>(listODsp);
        }
    }
}