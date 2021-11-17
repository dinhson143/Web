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
            return View(list);
        }
    }
}
