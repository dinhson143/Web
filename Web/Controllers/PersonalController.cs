using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Web.Models;
using Web.ServiceApi_Admin_User.Service.Products;
using Web.ServiceApi_Admin_User.Service.Users;
using Web.Utilities.Contants;
using Web.ViewModels.Catalog.Products;
using Web.ViewModels.System.User;

namespace Web.Controllers
{
    public class PersonalController : Controller
    {
        private readonly IProductApi _productApi;
        private readonly IUserApi _userApi;

        public PersonalController(IProductApi productApi, IUserApi userApi)
        {
            _productApi = productApi;
            _userApi = userApi;
        }

        public async Task<IActionResult> Index()
        {
            string token = HttpContext.Session.GetString(SystemContants.AppSettings.Token);
            var culture = CultureInfo.CurrentCulture.Name;
            var identity = (ClaimsIdentity)User.Identity;
            var claimsPrincipal = new ClaimsPrincipal(identity);

            // Get the claims values
            var id = identity.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                               .Select(c => c.Value).SingleOrDefault();
            var dob = identity.Claims.Where(c => c.Type == ClaimTypes.DateOfBirth)
                               .Select(c => c.Value).SingleOrDefault();

            if (id == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var user = await _userApi.GetUserById(new Guid(id), token);
            string[] dobs = dob.Split(' ');

            var data = new ProductFVrequest()
            {
                Email = user.Email,
                LanguageID = culture
            };
            var list = await _productApi.GetProductsFavorite(data, token);

            var viewModel = new PersonalViewmodel()
            {
                Address = user.Address,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Dob = DateTime.ParseExact(dobs[0], "MM/dd/yyyy", CultureInfo.InvariantCulture),
                Phone = user.PhoneNumber,
                Id = new Guid(id),
                Diem = user.Diem,
                ListLove = list
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UserViewModelWeb request)
        {
            if (!ModelState.IsValid)
            {
                TempData["Message"] = "Cập nhật thông tin cá nhân thất bại";
                return RedirectToAction("Index", "Personal");
            }
            string token = HttpContext.Session.GetString(SystemContants.AppSettings.Token);
            var data = new UpdateUserRequest()
            {
                Address = request.Address,
                Email = request.Email,
                Phonenumber = request.Phone,
                Id = new Guid(request.Id),
                BearerToken = token,
                LastName = request.LastName,
                Dob = request.Dob,
                FirstName = request.FirstName
            };
            var response = await _userApi.Update(new Guid(request.Id), data);

            if (response.IsSuccess == true)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                HttpContext.Session.Remove("Token");
            }
            else
            {
                TempData["Message"] = "Cập nhật thông tin cá nhân thất bại";
            }
            return RedirectToAction("Index", "Personal");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var identity = (ClaimsIdentity)User.Identity;
            var claimsPrincipal = new ClaimsPrincipal(identity);

            // Get the claims values
            var userId = identity.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                               .Select(c => c.Value).SingleOrDefault();
            string token = HttpContext.Session.GetString(SystemContants.AppSettings.Token);

            await _productApi.DeleteProductFV(new Guid(userId), id, token);
            return RedirectToAction("Index", "Personal");
        }
    }
}