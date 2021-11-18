using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.ServiceApi_Admin_User.Service.ShipperOrder;
using Web.ServiceApi_Admin_User.Service.Users;
using Web.Utilities.Contants;
using Web.ViewModels.Catalog.Users;

namespace Web.AdminApp.Controllers.Components
{
    public class ShipperOrderController : Controller
    {
        private readonly IShipperOrderApi _shipperOrderApi;
        private readonly IUserApi _userApi;

        public ShipperOrderController(IUserApi userApi,IShipperOrderApi shipperOrderApi)
        {
            _shipperOrderApi = shipperOrderApi;
            _userApi = userApi;
        }
        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 10)
        {
            var languageId = HttpContext.Session.GetString(SystemContants.AppSettings.DefaultLanguageId);
            var token = HttpContext.Session.GetString(SystemContants.AppSettings.Token);
            var newRequest = new GetUserPagingRequest()
            {
                BearerToken = token,
                Keyword = keyword,
                pageIndex = pageIndex,
                pageSize = pageSize
            };
            var response = await _userApi.GetAllShipperPaging(newRequest);
            
            return View(response);
        }
        public async Task<IActionResult> OrderRequire(string IdUser)
        {
            Guid Id = Guid.Parse(IdUser);
            var languageId = HttpContext.Session.GetString(SystemContants.AppSettings.DefaultLanguageId);
            var token = HttpContext.Session.GetString(SystemContants.AppSettings.Token);
            var list = await _shipperOrderApi.GetallOrderSPrequest(Id,languageId,token);
            foreach(var item in list)
            {
                item.IdUser = IdUser;
            }
            return View(list);
        }
        public async Task<IActionResult> ConfirmOrderSP(int id,string userId)
        {
            Guid IDUser = Guid.Parse(userId);
            var languageId = HttpContext.Session.GetString(SystemContants.AppSettings.DefaultLanguageId);
            var token = HttpContext.Session.GetString(SystemContants.AppSettings.Token);
            var result = await _shipperOrderApi.ConfirmOrderSP(id, IDUser, token);
            var ktra = Int32.Parse(result);
            if (ktra>0)
            {
                @TempData["Message"] = "Xác nhận đơn hàng thành công";

                return RedirectToAction("OrderRequire", new {IdUser= userId });
                }

            return RedirectToAction("Error", "Home");
        }

        public async Task<IActionResult> OrderHistory(string IdUser)
        {
            Guid userID = Guid.Parse(IdUser);
            var languageId = HttpContext.Session.GetString(SystemContants.AppSettings.DefaultLanguageId);
            var token = HttpContext.Session.GetString(SystemContants.AppSettings.Token);
            var list = await _shipperOrderApi.GetAll_HistorySP(userID,token);
            return View(list);
        }
    }
}
