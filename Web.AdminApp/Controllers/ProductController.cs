using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.AdminApp.Service.Products;
using Web.Utilities.Contants;
using Web.ViewModels.Catalog.Products;

namespace Web.AdminApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductApi _productApi;
        private readonly IConfiguration _config;

        public ProductController(IProductApi productApi, IConfiguration config)
        {
            _productApi = productApi;
            _config = config;
        }

        public async Task<IActionResult> Index(string keyword)
        {
            var languageId = HttpContext.Session.GetString(SystemContants.AppSettings.DefaultLanguageId);
            var token = HttpContext.Session.GetString(SystemContants.AppSettings.Token);
            var model = new GetManageProductPagingRequest()
            {
                LanguageId = languageId,
                BearerToken = token
            };
            var pageResult = await _productApi.GetAll(model);
            return View(pageResult.Items);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> Create([FromForm] ProductCreate request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            request.BearerToken = HttpContext.Session.GetString(SystemContants.AppSettings.Token);
            request.LanguageId = HttpContext.Session.GetString(SystemContants.AppSettings.DefaultLanguageId);
            var result = await _productApi.CreateProduct(request);
            string message = "";
            if (result.IsSuccess == true)
            {
                message = "Thêm mới thành công";
                TempData["Message"] = message;
            }
            else
            {
                ModelState.AddModelError("", result.Message);
                return View(request);
            }
            return RedirectToAction("Index", "Product");
        }
    }
}