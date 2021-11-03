using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Application.Catalog.ThongKes;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThongKesController : ControllerBase
    {
        private readonly IThongKeService _manageservice;

        public ThongKesController(IThongKeService manageservice)
        {
            _manageservice = manageservice;
        }

        [HttpGet("danh-sach-yeu-thich/{languageId}")]
        public async Task<IActionResult> GetProductLovest(string languageId)
        {
            var listProduct = await _manageservice.ProductLovest(languageId);
            return Ok(listProduct);
        }

        [HttpGet("danh-sach-ban-chay/{languageId}/{from}/{to}")]
        public async Task<IActionResult> GetProductSavest(string to, string from, string languageId)
        {
            var listProduct = await _manageservice.ProductSavest(from, to, languageId);
            return Ok(listProduct);
        }
    }
}