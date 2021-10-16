using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Application.Catalog.Categories;
using Web.Application.Catalog.Sliders;
using Web.ViewModels.Catalog.Categories;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("danh-sach")]
        [AllowAnonymous]
        public async Task<List<CategoryViewModel>> GetAll(string languageID)
        {
            var result = await _categoryService.GetAll(languageID);
            return result;
        }

        [HttpGet("category_detail/{categoryId}/{languageId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCategoryById(int categoryId, string languageId)
        {
            var Category = await _categoryService.GetCategoryById(categoryId, languageId);
            return Ok(Category);
        }

        [HttpGet("category/{languageId}")]
        //[AllowAnonymous]
        public async Task<IActionResult> GetAllCategory_parent(string languageId)
        {
            var Category = await _categoryService.GetAllCategory_parent(languageId);
            return Ok(Category);
        }

        [HttpGet("category_child/{languageId}/{parentId}")]
        //[AllowAnonymous]
        public async Task<IActionResult> GetAllCategory_child(int parentId, string languageId)
        {
            var Category = await _categoryService.GetAllCategory_child(parentId, languageId);
            return Ok(Category);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategoryCreate request)
        {
            var result = await _categoryService.CreateCategory(request);
            return Ok(result);
        }

        [HttpPut("{categoryId}")]
        public async Task<IActionResult> Update(int categoryId, [FromBody] CategoryUpdateRequest request)
        {
            request.Id = categoryId;
            var result = await _categoryService.UpdateCategory(request);
            return Ok(result);
        }

        [HttpDelete("Delete/{categoryId}")]
        public async Task<IActionResult> Delete(int categoryId)
        {
            var result = await _categoryService.Delete(categoryId);
            return Ok(result);
        }
    }
}