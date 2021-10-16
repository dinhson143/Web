using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.ServiceApi_Admin_User.Service.Sliders;
using Web.Utilities.Contants;
using Web.ViewModels.Catalog.Sliders;

namespace Web.AdminApp.Controllers
{
    public class SliderController : Controller
    {
        private readonly ISliderApi _sliderApi;

        public SliderController(ISliderApi sliderApi)
        {
            _sliderApi = sliderApi;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _sliderApi.GetAll();
            return View(result.ResultObj);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> Create([FromForm] SliderCreate request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            string token = HttpContext.Session.GetString(SystemContants.AppSettings.Token);
            var result = await _sliderApi.CreateProduct(request, token);
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
            return RedirectToAction("Index", "Slider");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var languageId = HttpContext.Session.GetString(SystemContants.AppSettings.DefaultLanguageId);
            var token = HttpContext.Session.GetString(SystemContants.AppSettings.Token);
            var result = await _sliderApi.GetSliderById(id, token);
            var slider = result.ResultObj;
            var editVm = new SliderUpdateRequest()
            {
                Id = slider.Id,
                Url = slider.Url,
                Name = slider.Name,
                Description = slider.Description
            };
            return View(editVm);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> Edit([FromForm] SliderUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            string token = HttpContext.Session.GetString(SystemContants.AppSettings.Token);
            var result = await _sliderApi.UpdateSlider(request, token);
            string message = "";
            if (result == true)
            {
                message = "Chỉnh sửa thành công";
                TempData["Message"] = message;
            }
            else
            {
                return View(request);
            }
            return RedirectToAction("Index", "Slider");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int Id)
        {
            var token = HttpContext.Session.GetString(SystemContants.AppSettings.Token);
            var result = await _sliderApi.Delete(Id, token);
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