using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.ServiceApi_Admin_User.Service.Sizes;
using Web.Utilities.Contants;
using Web.ViewModels.Catalog.Sizes;

namespace Web.AdminApp.Controllers
{
    public class SizeController : Controller
    {
        private readonly ISizeApi _sizeApi;

        public SizeController(ISizeApi sizeApi)
        {
            _sizeApi = sizeApi;
        }

        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Session.GetString(SystemContants.AppSettings.Token);
            var result = await _sizeApi.GetAll(token);

            if (result.IsSuccess == false)
            {
                return null;
            }
            return View(result.ResultObj);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var token = HttpContext.Session.GetString(SystemContants.AppSettings.Token);
            var result = await _sizeApi.Delete(id, token);
            string message = "";
            if (result == true)
            {
                message = "Xóathành công";
                TempData["Message"] = message;
            }
            else
            {
                message = "Xóa thất bại";
                TempData["Message"] = message;
            }
            return RedirectToAction("Index", "Size");
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(SizeViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var token = HttpContext.Session.GetString(SystemContants.AppSettings.Token);

            var result = await _sizeApi.CreateSize(request, token);

            if (result.IsSuccess == false)
            {
                ModelState.AddModelError("", result.Message);
                return View(request);
            }
            TempData["Message"] = "Thêm Size thành công";
            return RedirectToAction("Index", "Size");
        }
    }
}