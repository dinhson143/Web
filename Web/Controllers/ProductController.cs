using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Web.Models;
using Web.ServiceApi_Admin_User.Service.Products;
using Web.ViewModels.Catalog.Products;

namespace Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductApi _productApi;

        public ProductController(IProductApi productApi)
        {
            _productApi = productApi;
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
                pageSize = 2
            };
            var result = await _productApi.GetAllByCategoryId(request);
            return View(result);
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