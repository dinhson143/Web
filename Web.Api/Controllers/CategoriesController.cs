using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Application.Catalog.Categories;
using Web.ViewModels.Catalog.Categories;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("categories")]
        public async Task<List<CategoryViewModel>> GetAll(string languageID)
        {
            var result = await _categoryService.GetAll(languageID);
            return result;
        }
    }
}