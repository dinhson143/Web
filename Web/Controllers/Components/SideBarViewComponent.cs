using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Web.ServiceApi_Admin_User.Service.Categories;

namespace Web.Controllers.Components
{
    public class SideBarViewcomponent : ViewComponent
    {
        private readonly ICategoryApi _categoryApi;

        public SideBarViewcomponent(ICategoryApi categoryApi)
        {
            _categoryApi = categoryApi;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var items = await _categoryApi.GetAll(CultureInfo.CurrentCulture.Name, "");
            return View(items.ResultObj);
        }
    }
}