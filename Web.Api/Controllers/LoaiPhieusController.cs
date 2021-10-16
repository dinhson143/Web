using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Application.Catalog.LoaiPhieus;
using Web.ViewModels.Catalog.LoaiPhieus;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LoaiPhieusController : ControllerBase
    {
        private readonly ILoaiPhieuService _loaiPhieuService;

        public LoaiPhieusController(ILoaiPhieuService loaiPhieuService)
        {
            _loaiPhieuService = loaiPhieuService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LoaiPhieuCreate request)
        {
            var result = await _loaiPhieuService.CreateLoaiPhieu(request);
            return Ok(result);
        }

        [HttpGet("loaiphieus")]
        public async Task<List<LoaiPhieuViewModel>> GetAll()
        {
            var result = await _loaiPhieuService.GetAll();
            return result;
        }

        [HttpDelete("Delete/{loaiphieuId}")]
        public async Task<IActionResult> Delete(int loaiphieuId)
        {
            var result = await _loaiPhieuService.Delete(loaiphieuId);
            return Ok(result);
        }
    }
}