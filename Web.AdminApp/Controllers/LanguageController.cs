using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.ServiceApi_Admin_User.Service.Languages;
using Web.Utilities.Contants;
using Web.ViewModels.Catalog.Languages;

namespace Web.AdminApp.Controllers
{
    public class LanguageController : Controller
    {
        private readonly ILanguageApi _languageApi;

        public LanguageController(ILanguageApi languageApi)
        {
            _languageApi = languageApi;
        }

        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Session.GetString(SystemContants.AppSettings.Token);
            var result = await _languageApi.GetAll(token);
            return View(result.ResultObj);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(LanguageCreate request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var token = HttpContext.Session.GetString(SystemContants.AppSettings.Token);
            var result = await _languageApi.CreateLanguage(request, token);

            if (result.IsSuccess == false)
            {
                ModelState.AddModelError("", result.Message);
                return View(request);
            }
            TempData["Message"] = "Thêm Language thành công";
            return RedirectToAction("Index", "Language");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string Id)
        {
            var token = HttpContext.Session.GetString(SystemContants.AppSettings.Token);
            var result = await _languageApi.Delete(Id, token);
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
            return RedirectToAction("Index", "Language");
        }
    }
}