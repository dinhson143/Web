using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Application.Catalog.Sizes;
using Web.ViewModels.Catalog.Sizes;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SizesController : ControllerBase
    {
        private readonly ISizeService _sizeService;

        public SizesController(ISizeService sizeService)
        {
            _sizeService = sizeService;
        }

        [HttpGet("danh-sach")]
        [AllowAnonymous]
        public async Task<List<SizeViewModel>> GetAll()
        {
            var result = await _sizeService.GetAll();
            return result;
        }

        [HttpPost]
        public async Task<IActionResult> Create(SizeViewModel request)
        {
            var result = await _sizeService.CreateSize(request);
            return Ok(result);
        }

        [HttpDelete("Delete/{sizeId}")]
        public async Task<IActionResult> Delete(int sizeId)
        {
            var result = await _sizeService.Delete(sizeId);
            return Ok(result);
        }

        [HttpGet("size_detail/{sizeId}")]
        public async Task<IActionResult> GetCategoryById(int sizeId)
        {
            var size = await _sizeService.GetSizeById(sizeId);
            return Ok(size);
        }
    }
}