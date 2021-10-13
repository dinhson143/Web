using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Application.Catalog.Orders;
using Web.ViewModels.Catalog.Sales;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _manageservice;

        public OrdersController(IOrderService manageservice)
        {
            _manageservice = manageservice;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CheckoutRequest request)
        {
            var result = await _manageservice.CreateOrder(request);
            return Ok(result);
        }

        [HttpGet("danh-sach-order/{languageID}/{userId}")]
        public async Task<IActionResult> GetOrderUser(Guid userId, string languageID)
        {
            var result = await _manageservice.GetOrderUser(userId, languageID);
            return Ok(result);
        }
    }
}