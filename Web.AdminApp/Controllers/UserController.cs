using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.AdminApp.Service;
using Web.ViewModels.Catalog.Common;
using Web.ViewModels.Catalog.Users;

namespace Web.AdminApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserApi _userApi;
        private readonly IConfiguration _config;

        public UserController(IUserApi userApi, IConfiguration config)
        {
            _userApi = userApi;
            _config = config;
        }

        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 10)
        {
            var session = HttpContext.Session.GetString("Token");
            var newRequest = new GetUserPagingRequest()
            {
                BearerToken = session,
                Keyword = keyword,
                pageIndex = pageIndex,
                pageSize = pageSize
            };
            var response = await _userApi.GetAllPaging(newRequest);
            return View(response);
        }
    }
}