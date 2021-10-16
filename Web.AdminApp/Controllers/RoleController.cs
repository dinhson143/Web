using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using Web.ServiceApi_Admin_User.Service.Roles;
using Web.Utilities.Contants;
using Web.ViewModels.Catalog.Roles;

namespace Web.AdminApp.Controllers
{
    public class RoleController : Controller
    {
        private readonly IRoleApi _roleApi;
        private readonly IConfiguration _config;

        public RoleController(IRoleApi roleApi, IConfiguration config)
        {
            _roleApi = roleApi;
            _config = config;
        }

        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Session.GetString(SystemContants.AppSettings.Token);
            var result = await _roleApi.GetAll(token);
            return View(result.ResultObj);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var token = HttpContext.Session.GetString(SystemContants.AppSettings.Token);
            var result = await _roleApi.Delete(id, token);
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
            return RedirectToAction("Index", "Role");
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var token = HttpContext.Session.GetString(SystemContants.AppSettings.Token);

            var result = await _roleApi.CreateRole(request, token);

            if (result.IsSuccess == false)
            {
                ModelState.AddModelError("", result.Message);
                return View(request);
            }
            TempData["Message"] = "Thêm Role thành công";
            return RedirectToAction("Index", "Role");
        }
    }
}