using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.ServiceApi_Admin_User.Service.LoaiPhieus;
using Web.Utilities.Contants;
using Web.ViewModels.Catalog.LoaiPhieus;

namespace Web.AdminApp.Controllers
{
    public class LoaiPhieuController : Controller
    {
        private readonly ILoaiPhieuApi _loaiPhieuApi;

        public LoaiPhieuController(ILoaiPhieuApi loaiPhieuApi)
        {
            _loaiPhieuApi = loaiPhieuApi;
        }

        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Session.GetString(SystemContants.AppSettings.Token);
            var result = await _loaiPhieuApi.GetAll(token);
            return View(result.ResultObj);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(LoaiPhieuCreate request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var token = HttpContext.Session.GetString(SystemContants.AppSettings.Token);
            var result = await _loaiPhieuApi.CreateLoaiPhieu(request, token);

            if (result.IsSuccess == false)
            {
                ModelState.AddModelError("", result.Message);
                return View(request);
            }
            TempData["Message"] = "Thêm Loại phiếu thành công";
            return RedirectToAction("Index", "LoaiPhieu");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int Id)
        {
            var token = HttpContext.Session.GetString(SystemContants.AppSettings.Token);
            var result = await _loaiPhieuApi.Delete(Id, token);
            string message = "";
            if (result == true)
            {
                message = "Xóa thành công";
                TempData["Message"] = message;
            }
            else
            {
                message = "Xóa thất bại";
                TempData["Message"] = message;
            }
            return RedirectToAction("Index", "LoaiPhieu");
        }
    }
}