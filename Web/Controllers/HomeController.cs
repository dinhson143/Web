using LazZiya.ExpressLocalization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Web.Models;
using Web.ServiceApi_Admin_User.Service.Products;
using Web.ServiceApi_Admin_User.Service.Sliders;
using Web.Utilities.Contants;
using Web.ViewModels.Catalog.Common;
using Web.ViewModels.Catalog.Sliders;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISharedCultureLocalizer _loc;
        private readonly ISliderApi _sliderApi;
        private readonly IProductApi _productApi;

        public HomeController(ILogger<HomeController> logger, ISharedCultureLocalizer loc, ISliderApi sliderApi, IProductApi productApi)
        {
            _logger = logger;
            _loc = loc;
            _sliderApi = sliderApi;
            _productApi = productApi;
        }

        public async Task<IActionResult> Index()
        {
            //var token = HttpContext.Session.GetString(SystemContants.AppSettings.Token);
            var culture = CultureInfo.CurrentCulture.Name;
            var msg = _loc.GetLocalizedString("Vietnamese");
            var Sliders = await _sliderApi.GetAll();
            var sliders = new List<SliderViewModel>();
            for (int i = 0; i < Sliders.ResultObj.Count; i++)
            {
                sliders.Add(Sliders.ResultObj[i]);
            }
            var FeaturedProducts = await _productApi.GetFeaturedProducts(culture, 4);
            var LatestProducts = await _productApi.GetLatestProducts(culture, 6);       
            var data = new HomeModel()
            {
                sliders = sliders,
                featured_products = FeaturedProducts,
                latest_products = LatestProducts,
            };
            return View(data);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult SetCultureCookie(string cltr, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(cltr)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }
    }
}