using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
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

        public IActionResult ProductDetail(int id)
        {
            return View();
        }
    }
}