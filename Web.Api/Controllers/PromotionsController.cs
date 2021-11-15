using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Application.Catalog.Promotions;
using Web.ViewModels.Catalog.Promotions;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PromotionsController : ControllerBase
    {
        private readonly IPromotionService _manageservice;

        public PromotionsController(IPromotionService manageservice)
        {
            _manageservice = manageservice;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PromotionCreate request)
        {
            var result = await _manageservice.CreatePromotion(request);
            return Ok(result);
        }

        [HttpGet("danh-sach-khuyen-mai")]
        [AllowAnonymous]
        public async Task<List<PromotionViewModel>> GetAll()
        {
            var result = await _manageservice.GetAll();
            return result;
        }

        [HttpGet("kiem-tra-khuyen-mai")]
        [AllowAnonymous]
        public async Task<string> KiemtraPromotions()
        {
            var result = await _manageservice.KiemtraPromotions();
            return result;
        }

        [HttpDelete("Block/{promotionId}")]
        public async Task<IActionResult> Delete(int promotionId)
        {
            var result = await _manageservice.Block(promotionId);
            return Ok(result);
        }
    }
}