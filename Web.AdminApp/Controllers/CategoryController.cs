using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.AdminApp.Models;
using Web.ServiceApi_Admin_User.Service.Categories;
using Web.Utilities.Contants;
using Web.ViewModels.Catalog.Categories;

namespace Web.AdminApp.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryApi _categoryApi;

        public CategoryController(ICategoryApi categoryApi)
        {
            _categoryApi = categoryApi;
        }

        public async Task<IActionResult> Index()
        {
            HttpContext.Session.SetString("parentIdChung", "0");
            var languageId = HttpContext.Session.GetString(SystemContants.AppSettings.DefaultLanguageId);
            var token = HttpContext.Session.GetString(SystemContants.AppSettings.Token);

            var result = await _categoryApi.GetAllCategory_parent(languageId, token);
            if (result.IsSuccess == false)
            {
                return View(null);
            }
            return View(result.ResultObj);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var token = HttpContext.Session.GetString(SystemContants.AppSettings.Token);
            var languageId = HttpContext.Session.GetString(SystemContants.AppSettings.DefaultLanguageId);
            var response = await _categoryApi.GetAllCategory_child(id, languageId, token);

            if (response.IsSuccess != false)
            {
                var data = new DetailCategoryViewModel();
                data.listCategory = response.ResultObj;
                HttpContext.Session.SetString("parentIdChung", id.ToString());
                return View(data);
            }
            return RedirectToAction("Error", "Home");
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreate request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var token = HttpContext.Session.GetString(SystemContants.AppSettings.Token);
            var languageId = HttpContext.Session.GetString(SystemContants.AppSettings.DefaultLanguageId);
            request.LanguageId = languageId;
            var parentIdChung = int.Parse(HttpContext.Session.GetString("parentIdChung"));
            if (parentIdChung > 0)
            {
                request.ParentId = parentIdChung;
            }
            var result = await _categoryApi.CreateCategory(request, token);

            if (result.IsSuccess == false)
            {
                ModelState.AddModelError("", result.Message);
                return View(request);
            }
            TempData["Message"] = "Thêm loại sản phẩm thành công";
            return RedirectToAction("Index", "Category");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var languageId = HttpContext.Session.GetString(SystemContants.AppSettings.DefaultLanguageId);
            var category = await _categoryApi.GetCategoryById(id, languageId);
            var editVm = new CategoryUpdateRequest()
            {
                Id = id,
                SeoDescription = category.SeoDescription,
                Name = category.Name,
                SeoTitle = category.SeoTitle,
                SeoAlias = category.SeoAlias
            };
            return View(editVm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] CategoryUpdateRequest request)
        {
            var languageId = HttpContext.Session.GetString(SystemContants.AppSettings.DefaultLanguageId);
            var token = HttpContext.Session.GetString(SystemContants.AppSettings.Token);
            request.LanguageId = languageId;
            var result = await _categoryApi.Update(request, token);
            string message = "";
            if (result == true)
            {
                message = "Chình sửa thành công";
                TempData["Message"] = message;
            }
            else
            {
                return View(request);
            }
            return RedirectToAction("Index", "Category");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int Id)
        {
            var token = HttpContext.Session.GetString(SystemContants.AppSettings.Token);
            var result = await _categoryApi.Delete(Id, token);
            string message = "";
            if (result == true)
            {
                message = "Xóathành công";
                TempData["Message"] = message;
            }
            else
            {
                message = "Xóa thất bại";
                TempData["Message"] = message;
            }
            return RedirectToAction("Index", "Category");
        }
    }
}