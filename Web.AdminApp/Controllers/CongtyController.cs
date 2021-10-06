using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.ServiceApi_Admin_User.Service.Congtys;
using Web.Utilities.Contants;
using Web.ViewModels.Catalog.Congtys;

namespace Web.AdminApp.Controllers
{
    public class CongtyController : Controller
    {
        private readonly ICongtyApi _congtyApi;

        public CongtyController(ICongtyApi congtyApi)
        {
            _congtyApi = congtyApi;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _congtyApi.GetAll();
            var congtys = result.ResultObj;
            return View(congtys);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CongtyCreate request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var token = HttpContext.Session.GetString(SystemContants.AppSettings.Token);
            var result = await _congtyApi.CreateCongty(request, token);

            if (result.IsSuccess == false)
            {
                ModelState.AddModelError("", result.Message);
                return View(request);
            }
            TempData["Message"] = "Thêm Công ty thành công";
            return RedirectToAction("Index", "Congty");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var editVm = await _congtyApi.GetCongtyById(id);
            return View(editVm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] CongtyViewModel request)
        {
            var token = HttpContext.Session.GetString(SystemContants.AppSettings.Token);
            var result = await _congtyApi.Update(request, token);
            string message = "";
            if (result == true)
            {
                message = "Chình sửa thành công";
                TempData["Message"] = message;
            }
            else
            {
                return View(request);
            }
            return RedirectToAction("Index", "Congty");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int Id)
        {
            var token = HttpContext.Session.GetString(SystemContants.AppSettings.Token);
            var result = await _congtyApi.Delete(Id, token);
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
            return RedirectToAction("Index", "Congty");
        }
    }
}