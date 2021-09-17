using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Web.Application.Catalog.Products;
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

        [HttpPost]
        [Consumes("multipart/form-data")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> Create([FromForm] ProductCreate request)
        {
            var result = await _manageservice.CreateProduct(request);
            return Ok(result);
        }
    }
}