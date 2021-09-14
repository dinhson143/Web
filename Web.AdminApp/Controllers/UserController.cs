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
using Web.ViewModels.System.User;

namespace Web.AdminApp.Controllers
{
    public class UserController : CheckTokenController
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

        [HttpGet]
        public async Task<IActionResult> Update(Guid IdUser)
        {
            var session = HttpContext.Session.GetString("Token");
            var response = await _userApi.GetUserById(IdUser, session);

            var us = new UpdateUserRequest()
            {
                Dob = response.Dob,
                Email = response.Email,
                FirstName = response.FirstName,
                LastName = response.LastName,
                Phonenumber = response.PhoneNumber
            };
            return View(us);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Guid IdUser, UpdateUserRequest request)
        {
            var session = HttpContext.Session.GetString("Token");
            request.BearerToken = session;
            var response = await _userApi.Update(IdUser, request);
            return View(response);
        }
    }
}