using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Web.Application.Catalog.Products;
using Web.ViewModels.Catalog.Categories;
using Web.ViewModels.Catalog.Products;

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
        public async Task<IActionResult> GetAll([FromQuery] GetManageProductPagingRequest request)
        {
            var listProduct = await _manageservice.GetAll(request);
            return Ok(listProduct.ResultObj);
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetSize_Color(int productId)
        {
            var listProduct = await _manageservice.GetSize_Color(productId);
            return Ok(listProduct);
        }

        [HttpGet("product_detail/{productId}/{languageId}")]
        public async Task<IActionResult> GetProductById(int productId, string languageId)
        {
            var Product = await _manageservice.GetProductById(productId, languageId);
            return Ok(Product);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> Create([FromForm] ProductCreate request)
        {
            var result = await _manageservice.CreateProduct(request);
            return Ok(result);
        }

        [HttpPut("{productId}/categories")]
        public async Task<IActionResult> CategoryAssign(int productId, [FromBody] CategoryAssignRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _manageservice.AssignCategory(productId, request);
            if (result.IsSuccess == false)
            {
                return BadRequest(result.ResultObj);
            }
            return Ok(result.ResultObj);
        }
    }
}