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
        public async Task<IActionResult> Details(Guid IdUser)
        {
            var session = HttpContext.Session.GetString("Token");
            var response = await _userApi.GetUserById(IdUser, session);

            if (response.Email != null)
            {
                return View(response);
            }
            return RedirectToAction("Error", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid IdUser)
        {
            var session = HttpContext.Session.GetString("Token");
            var response = await _userApi.GetUserById(IdUser, session);

            if (response.Email != null)
            {
                var us = new UpdateUserRequest()
                {
                    Dob = response.Dob,
                    Email = response.Email,
                    FirstName = response.FirstName,
                    LastName = response.LastName,
                    Phonenumber = response.PhoneNumber,
                    Id = IdUser
                };
                return View(us);
            }
            return RedirectToAction("Error", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateUserRequest request)
        {
            var session = HttpContext.Session.GetString("Token");
            request.BearerToken = session;
            var response = await _userApi.Update(request.Id, request);
            TempData["Message"] = response.ResultObj;
            return RedirectToAction("Index", "User");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid IdUser)
        {
            var session = HttpContext.Session.GetString("Token");
            var response = await _userApi.DeleteUser(IdUser, session);
            TempData["Message"] = response.ResultObj;
            return RedirectToAction("Index", "User");
        }
    }
}