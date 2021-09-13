using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.AdminApp.Service;
using Web.ViewModels.System.User;

namespace Web.AdminApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserApi _userApi;

        public UserController(IUserApi userApi)
        {
            _userApi = userApi;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(ModelState);
            }
            var token = await _userApi.Login(request);
            return View(token);
        }
    }
}