﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Web.Models;
using Web.ServiceApi_Admin_User.Service.Orders;
using Web.Utilities.Contants;
using Web.ViewModels.Catalog.Orders;

namespace Web.Controllers
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
            var identity = (ClaimsIdentity)User.Identity;
            var claimsPrincipal = new ClaimsPrincipal(identity);

            // Get the claims values
            var id = identity.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                               .Select(c => c.Value).SingleOrDefault();

            if (id == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var culture = CultureInfo.CurrentCulture.Name;
            string token = HttpContext.Session.GetString(SystemContants.AppSettings.Token);
            var result = await _orderApi.GetOrderUser(new Guid(id), culture, token);
            if (!result.IsSuccess)
            {
                TempData["msg"] = "Lỗi khi lấy danh sách đơn đặt hàng";
                return View();
            }
            var data = new OrderViewModelWeb()
            {
                listOrder = result.ResultObj
            };
            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var identity = (ClaimsIdentity)User.Identity;
            var claimsPrincipal = new ClaimsPrincipal(identity);

            // Get the claims values
            var userId = identity.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                               .Select(c => c.Value).SingleOrDefault();

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var token = HttpContext.Session.GetString(SystemContants.AppSettings.Token);
            var culture = CultureInfo.CurrentCulture.Name;
            var result = await _orderApi.GetOrderUser(new Guid(userId), culture, token);

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
    }
}