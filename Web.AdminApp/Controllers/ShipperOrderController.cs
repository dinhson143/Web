using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
            foreach (var item in response.Items)
            {
                item.Ngaysinh = item.Dob.Day + "/" + item.Dob.Month + "/" + item.Dob.Year;
            }
            // check so luong 
            foreach (var item in response.Items)
            {

                var list = await _shipperOrderApi.GetallOrderSPrequest(item.Id, languageId, token);
                var dem = 0;
                foreach (var us in list)
                {
                    if (us.Status == "AdminConfirm")
                    {
                        dem++;
                    }
                }
                item.SoluongYC = dem;
            }
            return View(response);
        }
        [HttpPost]
        public async Task<Object> Reload(string keyword, int pageIndex = 1, int pageSize = 10)
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
            foreach(var item in response.Items)
            {
                item.FirstName = item.FirstName + " " + item.LastName;
                item.Ngaysinh = item.Dob.Day + "/" + item.Dob.Month + "/" + item.Dob.Year;
            }
            // check so luong 
            foreach(var item in response.Items)
            {
                
                var list = await _shipperOrderApi.GetallOrderSPrequest(item.Id, languageId, token);
                var dem = 0;
                foreach (var us in list)
                {
                    if(us.Status == "AdminConfirm")
                    {
                        dem++;
                    }
                }
                item.SoluongYC = dem;
            }
            
            // 
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("data",response);
            return JsonConvert.SerializeObject(data);
        }
        [HttpPost]
        public async Task<Object> OrderRequireReload(string IdUser)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            HttpContext.Session.SetString("IdUser", IdUser);
            return JsonConvert.SerializeObject(data);
        }
        public async Task<IActionResult> OrderRequire()
        {
            string IdUser = HttpContext.Session.GetString("IdUser");
            HttpContext.Session.Remove("IdUser");
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
                HttpContext.Session.SetString("IdUser", userId);
                return RedirectToAction("OrderRequire");
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
