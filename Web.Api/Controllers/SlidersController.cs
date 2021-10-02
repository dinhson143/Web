using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Application.Catalog.Sliders;
using Web.ViewModels.Catalog.Sliders;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SlidersController : ControllerBase
    {
        private readonly ISliderService _sliderService;

        public SlidersController(ISliderService sliderService)
        {
            _sliderService = sliderService;
        }

        [HttpGet("danh-sach")]
        [AllowAnonymous]
        public async Task<List<SliderViewModel>> GetAll()
        {
            var result = await _sliderService.GetAll();
            return result;
        }

        [HttpGet("slider_detail/{sliderId}")]
        public async Task<IActionResult> GetSliderById(int sliderId)
        {
            var slider = await _sliderService.GetSliderById(sliderId);
            return Ok(slider);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> Create([FromForm] SliderCreate request)
        {
            var result = await _sliderService.CreateSlider(request);
            return Ok(result);
        }

        [HttpPut("{sliderId}")]
        [Consumes("multipart/form-data")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> UpdateSlider(int sliderId, [FromForm] SliderUpdateRequest request)
        {
            request.Id = sliderId;
            var result = await _sliderService.UpdateSlider(request);
            return Ok(result);
        }

        [HttpDelete("Delete/{sliderId}")]
        public async Task<IActionResult> Delete(int sliderId)
        {
            var result = await _sliderService.Delete(sliderId);
            return Ok(result);
        }
    }
}