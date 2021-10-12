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
            var identity = (ClaimsIdentity)User.Identity;
            var claimsPrincipal = new ClaimsPrincipal(identity);

            // Get the claims values
            var username = identity.Claims.Where(c => c.Type == ClaimTypes.Name)
                              .Select(c => c.Value).SingleOrDefault();
            var firstname = identity.Claims.Where(c => c.Type == ClaimTypes.GivenName)
                              .Select(c => c.Value).SingleOrDefault();
            var lastname = identity.Claims.Where(c => c.Type == ClaimTypes.Surname)
                              .Select(c => c.Value).SingleOrDefault();
            var dob = identity.Claims.Where(c => c.Type == ClaimTypes.DateOfBirth)
                              .Select(c => c.Value).SingleOrDefault();
            var email = identity.Claims.Where(c => c.Type == ClaimTypes.Email)
                               .Select(c => c.Value).SingleOrDefault();
            var address = identity.Claims.Where(c => c.Type == ClaimTypes.StreetAddress)
                               .Select(c => c.Value).SingleOrDefault();
            var phone = identity.Claims.Where(c => c.Type == ClaimTypes.MobilePhone)
                               .Select(c => c.Value).SingleOrDefault();
            var id = identity.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                               .Select(c => c.Value).SingleOrDefault();

            if (username == null && email == null && address == null && phone == null)
            {
                return RedirectToAction("Login", "Account");
            }
            string[] dobs = dob.Split(' ');
            var culture = CultureInfo.CurrentCulture.Name;
            string token = HttpContext.Session.GetString(SystemContants.AppSettings.Token);
            var data = new ProductFVrequest()
            {
                Email = email,
                LanguageID = culture
            };
            var list = await _productApi.GetProductsFavorite(data, token);

            var viewModel = new PersonalViewmodel()
            {
                Address = address,
                Email = email,
                FirstName = firstname,
                LastName = lastname,
                Dob = DateTime.ParseExact(dobs[0], "MM/dd/yyyy", CultureInfo.InvariantCulture),
                Phone = phone,
                Id = new Guid(id),
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
    }
}