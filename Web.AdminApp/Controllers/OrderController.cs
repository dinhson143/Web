using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.ServiceApi_Admin_User.Service.Orders;
using Web.Utilities.Contants;
using Web.ViewModels.Catalog.Orders;

namespace Web.AdminApp.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderApi _orderApi;

        public OrderController(IOrderApi orderApi)
        {
            _orderApi = orderApi;
        }

        public async Task<IActionResult> Index()
        {
            var languageId = HttpContext.Session.GetString(SystemContants.AppSettings.DefaultLanguageId);
            var token = HttpContext.Session.GetString(SystemContants.AppSettings.Token);
            var result = await _orderApi.GetAllOrder(languageId, token);
            var listOrder = result.ResultObj;
            return View(listOrder);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var token = HttpContext.Session.GetString(SystemContants.AppSettings.Token);
            var languageId = HttpContext.Session.GetString(SystemContants.AppSettings.DefaultLanguageId);
            var result = await _orderApi.GetAllOrder(languageId, token);

            var data = new List<OrderDetailViewModel>();
            foreach (var order in result.ResultObj)
            {
                if (order.Id == id)
                {
                    data = order.ListOrDetail;
                    break;
                }
            }
            if (result.IsSuccess != false)
            {
                return View(data);
            }
            return RedirectToAction("Error", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmOrder(int id)
        {
            var token = HttpContext.Session.GetString(SystemContants.AppSettings.Token);
            var result = await _orderApi.ConfirmOrder(id, token);

            if (result != false)
            {
                @TempData["Message"] = "Xác nhận đơn hàng thành công";
                return RedirectToAction("index", "Order");
            }
            return RedirectToAction("Error", "Home");
        }
    }
}