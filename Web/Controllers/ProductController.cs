using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Web.Models;
using Web.ServiceApi_Admin_User.Service.Categories;
using Web.ServiceApi_Admin_User.Service.Comments;
using Web.ServiceApi_Admin_User.Service.Products;
using Web.Utilities.Contants;
using Web.ViewModels.Catalog.Common;
using Web.ViewModels.Catalog.Products;

namespace Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductApi _productApi;
        private readonly ICategoryApi _categoryApi;
        private readonly ICommentApi _commentApi;

        public ProductController(IProductApi productApi, ICategoryApi categoryApi, ICommentApi commentApi)
        {
            _productApi = productApi;
            _categoryApi = categoryApi;
            _commentApi = commentApi;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ProductforCategory(int id, int PageIndex)
        {
            if (PageIndex == 0)
            {
                PageIndex = 1;
            }
            var culture = CultureInfo.CurrentCulture.Name;
            var request = new GetPublicProductPagingRequest()
            {
                CategoryId = id,
                LanguageId = culture,
                pageIndex = PageIndex,
                pageSize = 6
            };
            var category = await _categoryApi.GetCategoryById(id, culture);

            var result = await _productApi.GetAllByCategoryId(request);
            var data = new ProductForCategoryViewModel()
            {
                Products = result,
                Categories = category
            };
            return View(data);
        }

        public async Task<IActionResult> ProductDetail(int id)
        {
            var av = await _productApi.AddViewCount(id);
            if (av.IsSuccess == false)
            {
                return RedirectToAction("Index", "Home");
            }
            var culture = CultureInfo.CurrentCulture.Name;
            var result = await _productApi.GetProductById(id, "", culture);
            var images = await _productApi.GetListImage(id);
            var comments = await _commentApi.GetAllWeb(id, culture);
            var productLQ = await _productApi.GetProductLQ(id, culture);
            var product = new ProductDetailViewModel()
            {
                ProductInfo = result.ResultObj,
                Images = images.ResultObj,
                Comments = comments.ResultObj,
                ProductLQ = productLQ.ResultObj
            };
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> ProductFavorite(int ProductId)
        {
            var identity = (ClaimsIdentity)User.Identity;
            var claimsPrincipal = new ClaimsPrincipal(identity);

            // Get the claims values
            var email = identity.Claims.Where(c => c.Type == ClaimTypes.Email)
                               .Select(c => c.Value).SingleOrDefault();

            if (email == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var data = new ProductFVCreate()
            {
                Email = email,
                ProductId = ProductId
            };
            string token = HttpContext.Session.GetString(SystemContants.AppSettings.Token);
            var result = await _productApi.CreateProductFV(data, token);
            if (result.IsSuccess == true)
            {
                TempData["msg"] = "Đã cập nhật danh sách yêu thích";
            }
            else
            {
                TempData["error"] = "Cập nhật danh sách yêu thích thất bại";
            }
            return RedirectToAction("ProductDetail", "Product", new { id = ProductId });
        }

        public async Task<IActionResult> ListProducts(int pageIndex, int? categoryId)
        {
            var culture = CultureInfo.CurrentCulture.Name;
            if (pageIndex == 0) pageIndex = 1;
            var model = new GetManageProductPagingRequest()
            {
                LanguageId = culture,
                CategoryId = categoryId,
                pageIndex = pageIndex,
                pageSize = 6
            };
            var pageResult = await _productApi.GetAllPaging(model);
            var page = new PagedResultBase()
            {
                PageIndex = pageIndex,
                PageSize = 6,
                TotalRecords = pageResult.TotalRecords
            };
            var data = new ListProductViewModel()
            {
                listPro= pageResult.Items,
                Pager = page
            };
            return View(data);
        }
    }
}