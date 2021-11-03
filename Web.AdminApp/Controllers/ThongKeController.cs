using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Models;
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

        [HttpGet]
        public async Task<IActionResult> ProductSavest()
        {
            //var token = HttpContext.Session.GetString(SystemContants.AppSettings.Token);
            //var languageId = HttpContext.Session.GetString(SystemContants.AppSettings.DefaultLanguageId);
            //var data = await _thongKeApi.ProductSavest(languageId, token);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ProductSavest(DateTime From, DateTime To)
        {
            var day = From.Day;
            var month = From.Month;
            var year = From.Year;
            var dateF = month.ToString() + "-" + day.ToString() + "-" + year.ToString();
            //
            var day2 = To.Day;
            var month2 = To.Month;
            var year2 = To.Year;
            var dateT = month2.ToString() + "-" + day2.ToString() + "-" + year2.ToString();
            var token = HttpContext.Session.GetString(SystemContants.AppSettings.Token);
            var languageId = HttpContext.Session.GetString(SystemContants.AppSettings.DefaultLanguageId);
            var data = await _thongKeApi.ProductSavest(dateF, dateT, languageId, token);
            var kq = new ProductSavestModel()
            {
                listPro = data.ResultObj
            };
            return View(kq);
        }

        [HttpGet]
        public IActionResult Doanhthu()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Doanhthu(DateTime From, DateTime To)
        {
            var day = From.Day;
            var month = From.Month;
            var year = From.Year;
            var dateF = month.ToString() + "-" + day.ToString() + "-" + year.ToString();
            //
            var day2 = To.Day;
            var month2 = To.Month;
            var year2 = To.Year;
            var dateT = month2.ToString() + "-" + day2.ToString() + "-" + year2.ToString();
            var token = HttpContext.Session.GetString(SystemContants.AppSettings.Token);
            var languageId = HttpContext.Session.GetString(SystemContants.AppSettings.DefaultLanguageId);
            var data = await _thongKeApi.Doanhthu(dateF, dateT, languageId, token);
            var kq = new ProductSavestModel()
            {
                listOd = data.ResultObj
            };
            return View(kq);
        }
    }
}