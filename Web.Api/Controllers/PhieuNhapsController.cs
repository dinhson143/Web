using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Application.Catalog.PhieuNhap;
using Web.ViewModels.Catalog.PhieuNhaps;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PhieuNhapsController : ControllerBase
    {
        private readonly IPhieuNhapService _phieuNhapService;

        public PhieuNhapsController(IPhieuNhapService phieuNhapService)
        {
            _phieuNhapService = phieuNhapService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PhieuNhapCreate request)
        {
            var result = await _phieuNhapService.CreatePhieuNhap(request);
            return Ok(result);
        }

        [HttpPost("chi-tiet-phieu-nhap")]
        public async Task<IActionResult> CreateCTPN([FromBody] CTPhieuNhapCreate request)
        {
            var result = await _phieuNhapService.CreateCTPhieuNhap(request);
            return Ok(result);
        }

        [HttpGet("danh-sach")]
        public async Task<List<PhieuNhapViewModel>> GetAll()
        {
            var result = await _phieuNhapService.GetAll();
            return result;
        }

        [HttpGet("phieunhap_detail/{pnId}/{languageId}")]
        public async Task<IActionResult> GetDetailPNById(int pnId, string languageId)
        {
            var ctpn = await _phieuNhapService.GetDetailPNById(pnId, languageId);
            return Ok(ctpn);
        }
    }
}