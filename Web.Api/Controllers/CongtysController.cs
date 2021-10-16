using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Application.Catalog.Congtys;
using Web.ViewModels.Catalog.Congtys;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CongtysController : ControllerBase
    {
        private readonly ICongtyService _congtyService;

        public CongtysController(ICongtyService congtyService)
        {
            _congtyService = congtyService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CongtyCreate request)
        {
            var result = await _congtyService.CreateCongty(request);
            return Ok(result);
        }

        [HttpGet("danh-sach")]
        [AllowAnonymous]
        public async Task<List<CongtyViewModel>> GetAll()
        {
            var result = await _congtyService.GetAll();
            return result;
        }

        [HttpDelete("Delete/{congtyId}")]
        public async Task<IActionResult> Delete(int congtyId)
        {
            var result = await _congtyService.Delete(congtyId);
            return Ok(result);
        }

        [HttpPut("{congtyId}")]
        public async Task<IActionResult> Update(int congtyId, [FromBody] CongtyViewModel request)
        {
            request.Id = congtyId;
            var result = await _congtyService.UpdateCongty(request);
            return Ok(result);
        }

        [HttpGet("congty_detail/{congtyId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCongtyById(int congtyId)
        {
            var congty = await _congtyService.GetCongtytById(congtyId);
            return Ok(congty);
        }
    }
}