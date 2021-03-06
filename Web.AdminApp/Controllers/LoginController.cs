using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Web.ServiceApi_Admin_User.Service.Users;
using Web.Utilities.Contants;
using Web.ViewModels.System.User;

namespace Web.AdminApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserApi _userApi;
        private readonly IConfiguration _config;

        public LoginController(IUserApi userApi, IConfiguration config)
        {
            _userApi = userApi;
            _config = config;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Login(string message)
        {
            TempData["Message"] = message;
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var token = await _userApi.Login(request);

            if (string.IsNullOrEmpty(token))
            {
                TempData["Message"] = "UserName or Password is incorrect";
                return View();
            }
            var userPricipal = this.ValidateToken(token);
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                IsPersistent = false   // tắt đi mở lại vẫn còn cookie login trước đó
            };

            HttpContext.Session.SetString(SystemContants.AppSettings.DefaultLanguageId, _config["DefaultLanguageId"]);
            HttpContext.Session.SetString(SystemContants.AppSettings.Token, token);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                userPricipal,
                authProperties);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("Token");
            return RedirectToAction("Login", "Login");
        }

        private ClaimsPrincipal ValidateToken(string jwtToken)
        {
            IdentityModelEventSource.ShowPII = true;

            SecurityToken validatedToken;
            TokenValidationParameters validationParameters = new TokenValidationParameters();

            validationParameters.ValidateLifetime = true;

            validationParameters.ValidAudience = _config["Tokens:Issuer"];
            validationParameters.ValidIssuer = _config["Tokens:Issuer"];
            validationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));

            ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out validatedToken);

            return principal;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var result = await _userApi.Register(request);

            if (result.IsSuccess == false)
            {
                ModelState.AddModelError("", result.Message);
                return View(request);
            }
            TempData["Message"] = result.ResultObj;
            return RedirectToAction("Index", "User");
        }
    }
}