using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Web.AdminApp.Models;
using Web.Models;
using Web.ServiceApi_Admin_User.Service.Thongkes;
using Web.ServiceApi_Admin_User.Service.Users;
using Web.Utilities.Contants;
using Web.ViewModels.Catalog.Users;

namespace Web.AdminApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IThongKeApi _thongKeApi;
        private readonly IUserApi _userApi;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IThongKeApi thongKeApi, IUserApi userApi)
        {
            _logger = logger;
            _thongKeApi = thongKeApi;
            _userApi = userApi;
        }

        public async Task<IActionResult> Index()
        {
            var identity = (ClaimsIdentity)User.Identity;
            var claimsPrincipal = new ClaimsPrincipal(identity);

            // Get the claims values
            var role = identity.Claims.Where(c => c.Type == ClaimTypes.Role)
                               .Select(c => c.Value).SingleOrDefault();
            var user = User.Identity.Name;

            if (role != "admin" && role != "user")
            {
                return RedirectToAction("Login", "Login", new { message = "Tài khoản không có quyền đăng nhập" });
            }
            //
            var date = DateTime.Now;
            var token = HttpContext.Session.GetString(SystemContants.AppSettings.Token);
            var languageId = HttpContext.Session.GetString(SystemContants.AppSettings.DefaultLanguageId);
            var data = await _thongKeApi.DoanhthuFullMonth(languageId, token);
            var listDT = new List<decimal>();
            var listSLOD = new List<int>();
            for (int i = 0; i < 12; i++)
            {
                listDT.Add(0);
                listSLOD.Add(0);
            }
            foreach (var item in data.ResultObj)
            {
                if (item.OrderDate.Month == date.Month && listDT[date.Month - 1] == 0)
                {
                    listDT[date.Month - 1] = (item.TongtienReal - item.Tongtien);
                    listSLOD[date.Month - 1] += 1;
                }
                else if (item.OrderDate.Month == date.Month && listDT[date.Month - 1] > 0)
                {
                    listDT[date.Month - 1] += (item.TongtienReal - item.Tongtien);
                    listSLOD[date.Month - 1] += 1;
                }
            }
            // sản phẩm bán chạy nhất
            var products = await _thongKeApi.ProductSavestFullMonth(languageId, token);
            // get user cua cua hàng
            string keyword = "";
            int pageIndex = 1;
            int pageSize = 10;
            var session = HttpContext.Session.GetString("Token");
            var newRequest = new GetUserPagingRequest()
            {
                BearerToken = session,
                Keyword = keyword,
                pageIndex = pageIndex,
                pageSize = pageSize
            };

            var response = await _userApi.GetAllPaging(newRequest);
            var listTVM = new List<UserViewModel>();
            var listTVD = new List<UserViewModel>();
            var listTVB = new List<UserViewModel>();
            var listTVV = new List<UserViewModel>();
            foreach (var item in response.Items)
            {
                if (item.Diem <= 5)
                {
                    foreach (var x in item.Roles)
                    {
                        if (x == "customer" || x == "shipper")
                        {
                            listTVM.Add(item);
                        }
                    }
                }
                else if (item.Diem > 5 && item.Diem <= 10)
                {
                    foreach (var x in item.Roles)
                    {
                        if (x == "customer" || x == "shipper")
                        {
                            listTVD.Add(item);
                        }
                    }
                }
                else if (item.Diem > 10 && item.Diem <= 15)
                {
                    foreach (var x in item.Roles)
                    {
                        if (x == "customer" || x == "shipper")
                        {
                            listTVB.Add(item);
                        }
                    }
                }
                else if (item.Diem > 15)
                {
                    foreach (var x in item.Roles)
                    {
                        if (x == "customer" || x == "shipper")
                        {
                            listTVV.Add(item);
                        }
                    }
                }
            }
            var listUS = new List<int>();
            listUS.Add(listTVM.Count);
            listUS.Add(listTVD.Count);
            listUS.Add(listTVB.Count);
            listUS.Add(listTVV.Count);
            //
            var kq = new ProductSavestModel()
            {
                listDT = listDT,
                listSLOD = listSLOD,
                listPro = products.ResultObj,
                listUS = listUS
            };
            return View(kq);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult Language(NavigationViewModel data)
        {
            HttpContext.Session.SetString(SystemContants.AppSettings.DefaultLanguageId, data.CurrentLanguageId);
            return Redirect(data.ReturnURL);
        }
    }
}