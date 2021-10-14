using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Web.ServiceApi_Admin_User.Service.Users;
using Web.Utilities.Contants;
using Web.ViewModels.System.User;

namespace Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserApi _userApi;
        private readonly IConfiguration _config;

        public AccountController(IUserApi userApi, IConfiguration config)
        {
            _userApi = userApi;
            _config = config;
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
                return View(request);
            }

            var token = await _userApi.Login(request);

            if (string.IsNullOrEmpty(token))
            {
                ModelState.AddModelError("", "Login failure");
                TempData["err"] = "Username or Password is incorrect";
                return View();
            }
            var userPricipal = this.ValidateToken(token);
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                IsPersistent = request.Rememberme   // tắt đi mở lại vẫn còn cookie login trước đó
            };

            //HttpContext.Session.SetString(SystemContants.AppSettings.DefaultLanguageId, _config["DefaultLanguageId"]);
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
            return RedirectToAction("Index", "Home");
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
                return View(request);
            }

            var result = await _userApi.Register(request);

            if (result.IsSuccess == false)
            {
                ModelState.AddModelError("", result.Message);
                return View(request);
            }

            var token = await _userApi.Login(new LoginRequest()
            {
                Password = request.Password,
                Username = request.Username,
                Rememberme = true
            });

            if (string.IsNullOrEmpty(token))
            {
                return View();
            }
            var userPricipal = this.ValidateToken(token);
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                IsPersistent = false   // tắt đi mở lại vẫn còn cookie login trước đó
            };

            //HttpContext.Session.SetString(SystemContants.AppSettings.DefaultLanguageId, _config["DefaultLanguageId"]);
            HttpContext.Session.SetString(SystemContants.AppSettings.Token, token);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                userPricipal,
                authProperties);

            return RedirectToAction("Index", "Home");
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
        public IActionResult CheckMail()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CheckMail(string Email)
        {
            var result = await _userApi.CheckMail(Email);
            if (result.IsSuccess == false)
            {
                TempData["err"] = result.Message;
            }
            int check = result.ResultObj;
            HttpContext.Session.SetString("ktMail", check.ToString());
            HttpContext.Session.SetString("Mail", Email);
            return RedirectToAction("MaCheckMail");
        }

        [HttpGet]
        public IActionResult MaCheckMail()
        {
            return View();
        }

        [HttpPost]
        public IActionResult MaCheckMail(int maKT)
        {
            int ma = int.Parse(HttpContext.Session.GetString("ktMail"));
            if (maKT == ma)
            {
                return RedirectToAction("ForgetPassword");
            }
            TempData["err"] = "Mã xác nhận không đúng";
            HttpContext.Session.Remove("ktMail");
            HttpContext.Session.Remove("Mail");
            return View();
        }

        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPassViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }
            string email = HttpContext.Session.GetString("Mail");
            request.Email = email;
            var result = await _userApi.ForgetPassword(request);
            if (!result.IsSuccess)
            {
                HttpContext.Session.Remove("ktMail");
                HttpContext.Session.Remove("Mail");
                TempData["err"] = "Cập nhật mật khẩu mới thành công";
            }
            else
            {
                TempData["success"] = "Cập nhật mật khẩu mới thành công";
                HttpContext.Session.Remove("ktMail");
                HttpContext.Session.Remove("Mail");
            }
            return View();
        }
    }
}