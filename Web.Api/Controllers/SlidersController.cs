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
    public class SlidersController : ControllerBase
    {
        private readonly ISliderService _sliderService;

        public SlidersController(ISliderService sliderService)
        {
            _sliderService = sliderService;
        }

        [HttpGet("danh-sach")]
        public async Task<List<SliderViewModel>> GetAll()
        {
            var result = await _sliderService.GetAll();
            return result;
        }
    }
}