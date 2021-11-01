using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Application.Catalog.PhieuXuat;
using Web.ViewModels.Catalog.PhieuXuats;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PhieuXuatsController : ControllerBase
    {
        private readonly IPhieuXuatService _phieuXuatService;

        public PhieuXuatsController(IPhieuXuatService phieuXuatService)
        {
            _phieuXuatService = phieuXuatService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PhieuXuatCreate request)
        {
            var result = await _phieuXuatService.CreatePhieuXuat(request);
            return Ok(result);
        }

        [HttpPost("chi-tiet-phieu-xuat")]
        public async Task<IActionResult> CreateCTPX([FromBody] CTPhieuXuatCreate request)
        {
            var result = await _phieuXuatService.CreateCTPhieXuat(request);
            return Ok(result);
        }

        [HttpGet("danh-sach")]
        public async Task<List<PhieuXuatViewModel>> GetAll()
        {
            var result = await _phieuXuatService.GetAll();
            return result;
        }

        [HttpGet("phieuxuat_detail/{pxId}/{languageId}")]
        public async Task<IActionResult> GetDetailPXById(int pxId, string languageId)
        {
            var ctpn = await _phieuXuatService.GetDetailPXById(pxId, languageId);
            return Ok(ctpn);
        }
    }
}