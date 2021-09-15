using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using Web.AdminApp.Service.Roles;
using Web.AdminApp.Service.Users;
using Web.ViewModels.Catalog.Common;
using Web.ViewModels.Catalog.Users;
using Web.ViewModels.System.User;

namespace Web.AdminApp.Controllers
{
    public class UserController : CheckTokenController
    {
        private readonly IUserApi _userApi;
        private readonly IConfiguration _config;
        private readonly IRoleApi _roleApi;

        public UserController(IUserApi userApi, IRoleApi roleApi, IConfiguration config)
        {
            _userApi = userApi;
            _config = config;
            _roleApi = roleApi;
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

        [HttpGet]
        public async Task<IActionResult> RoleAssign(Guid IdUser)
        {
            var response = await GetRoleAssignRequets(IdUser);
            response.Id = IdUser;
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> RoleAssign(RoleAssignRequest request)
        {
            var session = HttpContext.Session.GetString("Token");
            request.BearerToken = session;
            var response = await _userApi.RoleAssign(request.Id, request);
            if (response.IsSuccess)
            {
                TempData["Message"] = "Cấp quyền thành công";
                return RedirectToAction("Index", "User");
            }
            var roles = await GetRoleAssignRequets(request.Id);
            return View(roles);
        }

        private async Task<RoleAssignRequest> GetRoleAssignRequets(Guid IdUser)
        {
            var session = HttpContext.Session.GetString("Token");
            var response = await _roleApi.GetAll(session);
            var user = await _userApi.GetUserById(IdUser, session);

            var roleAssignRequest = new RoleAssignRequest();
            foreach (var role in response.ResultObj)
            {
                roleAssignRequest.Roles.Add(new SelectItems()
                {
                    Id = role.Id,
                    Name = role.Name,
                    Selected = user.Roles.Contains(role.Name)
                });
            }
            if (response.IsSuccess == true)
            {
                return roleAssignRequest;
            }
            return null;
        }
    }
}