using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Application.Catalog.Products;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IManageProductService _manageservice;

        public ProductController(IManageProductService manageservice)
        {
            _manageservice = manageservice;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var listProduct = await _manageservice.GetAll();
            return Ok(listProduct);
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetSize_Color(int productId)
        {
            var listProduct = await _manageservice.GetSize_Color(productId);
            return Ok(listProduct);
        }
    }
}