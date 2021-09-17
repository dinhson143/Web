using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.AdminApp.Service.Categories;
using Web.AdminApp.Service.Products;
using Web.Utilities.Contants;
using Web.ViewModels.Catalog.Products;

namespace Web.AdminApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductApi _productApi;
        private readonly IConfiguration _config;
        private readonly ICategoryApi _categoryApi;

        public ProductController(IProductApi productApi, IConfiguration config, ICategoryApi categoryApi)
        {
            _productApi = productApi;
            _config = config;
            _categoryApi = categoryApi;
        }

        public async Task<IActionResult> Index(int? categoryId)
        {
            var languageId = HttpContext.Session.GetString(SystemContants.AppSettings.DefaultLanguageId);
            var token = HttpContext.Session.GetString(SystemContants.AppSettings.Token);

            var categories = await _categoryApi.GetAll(languageId, token);
            ViewBag.Categories = categories.ResultObj.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString(),
                Selected = categoryId.HasValue && categoryId == x.Id
            });
            var model = new GetManageProductPagingRequest()
            {
                LanguageId = languageId,
                BearerToken = token,
                CategoryId = categoryId
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