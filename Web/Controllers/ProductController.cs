using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Web.Models;
using Web.ServiceApi_Admin_User.Service.Categories;
using Web.ServiceApi_Admin_User.Service.Products;
using Web.ViewModels.Catalog.Products;

namespace Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductApi _productApi;
        private readonly ICategoryApi _categoryApi;

        public ProductController(IProductApi productApi, ICategoryApi categoryApi)
        {
            _productApi = productApi;
            _categoryApi = categoryApi;
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
            var culture = CultureInfo.CurrentCulture.Name;
            var result = await _productApi.GetProductById(id, "", culture);
            var images = await _productApi.GetListImage(id);
            var product = new ProductDetailViewModel()
            {
                ProductInfo = result.ResultObj,
                Images = images.ResultObj
            };
            return View(product);
        }
    }
}