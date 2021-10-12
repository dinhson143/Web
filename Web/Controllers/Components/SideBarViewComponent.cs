using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Web.Models;
using Web.ServiceApi_Admin_User.Service.Categories;
using Web.ServiceApi_Admin_User.Service.Products;

namespace Web.Controllers.Components
{
    public class SideBarViewcomponent : ViewComponent
    {
        private readonly ICategoryApi _categoryApi;
        private readonly IProductApi _productApi;

        public SideBarViewcomponent(ICategoryApi categoryApi, IProductApi productApi)
        {
            _categoryApi = categoryApi;
            _productApi = productApi;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var culture = CultureInfo.CurrentCulture.Name;
            var items = await _categoryApi.GetAll(CultureInfo.CurrentCulture.Name, "");
            var OrderProducts = await _productApi.GetProductsOrderMax(culture, 2);
            var data = new SideBarViewModel()
            {
                listCate = items.ResultObj,
                listpro = OrderProducts
            };
            return View(data);
        }
    }
}