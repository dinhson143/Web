using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.ServiceApi_Admin_User.Service.Thongkes;
using Web.Utilities.Contants;

namespace Web.AdminApp.Controllers
{
    public class ThongKeController : Controller
    {
        private readonly IThongKeApi _thongKeApi;

        public ThongKeController(IThongKeApi thongKeApi)
        {
            _thongKeApi = thongKeApi;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ProductLovest()
        {
            var token = HttpContext.Session.GetString(SystemContants.AppSettings.Token);
            var languageId = HttpContext.Session.GetString(SystemContants.AppSettings.DefaultLanguageId);
            var data = await _thongKeApi.ProductLovest(languageId, token);
            return View(data.ResultObj);
        }
    }
}