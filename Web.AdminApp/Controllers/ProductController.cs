using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Web.ServiceApi_Admin_User.Service.Categories;
using Web.ServiceApi_Admin_User.Service.Products;
using Web.Utilities.Contants;
using Web.ViewModels.Catalog.Categories;
using Web.ViewModels.Catalog.Common;
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

        [HttpGet]
        public async Task<IActionResult> Details(int productID)
        {
            var token = HttpContext.Session.GetString(SystemContants.AppSettings.Token);
            var languageId = HttpContext.Session.GetString(SystemContants.AppSettings.DefaultLanguageId);
            var response = await _productApi.GetProductById(productID, token, languageId);

            if (response.IsSuccess != false)
            {
                return View(response.ResultObj);
            }
            return RedirectToAction("Error", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> AssignCategory(int id)
        {
            var response = await GetCategoryAssignRequets(id);
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> AssignCategory(CategoryAssignRequest request)
        {
            var session = HttpContext.Session.GetString("Token");
            request.BearerToken = session;
            var response = await _productApi.AssignCategory(request.Id, request);
            if (response.IsSuccess)
            {
                TempData["Message"] = "Gán danh mục thành công";
                return RedirectToAction("Index", "Product");
            }
            var roles = await GetCategoryAssignRequets(request.Id);
            return View();
        }

        private async Task<CategoryAssignRequest> GetCategoryAssignRequets(int productId)
        {
            var languageId = HttpContext.Session.GetString(SystemContants.AppSettings.DefaultLanguageId);
            var token = HttpContext.Session.GetString(SystemContants.AppSettings.Token);
            var categories = await _categoryApi.GetAll(languageId, token);
            if (categories.IsSuccess == false)
            {
                return null;
            }
            var data = await _productApi.GetProductById(productId, token, languageId);
            var product = data.ResultObj;

            var CategoryAssignRequest = new CategoryAssignRequest();
            foreach (var category in categories.ResultObj)
            {
                CategoryAssignRequest.Categories.Add(new SelectItems()
                {
                    Id = category.Id.ToString(),
                    Name = category.Name,
                    Selected = product.Categories.Contains(category.Name)
                });
            }
            return CategoryAssignRequest;
        }
    }
}