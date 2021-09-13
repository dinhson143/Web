using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Web.Application.Catalog.Products;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _manageservice;

        public ProductsController(IProductService manageservice)
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