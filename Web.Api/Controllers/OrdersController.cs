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

        [HttpGet("danh-sach-order-history/{languageID}/{userId}")]
        public async Task<IActionResult> GetOrderUserHistory(Guid userId, string languageID)
        {
            var result = await _manageservice.GetOrderUserHistory(userId, languageID);
            return Ok(result);
        }

        [HttpGet("order-id/{orderID}/{languageID}")]
        public async Task<IActionResult> GetOrderByID(int orderID, string languageID)
        {
            var result = await _manageservice.GetOrderByID(orderID, languageID);
            return Ok(result);
        }

        [HttpGet("get-all-order/{languageID}")]
        public async Task<IActionResult> GetallOrder(string languageID)
        {
            var result = await _manageservice.GetallOrder(languageID);
            return Ok(result);
        }

        [HttpGet("get-all-order-success/{languageID}")]
        public async Task<IActionResult> GetallOrderSuccess(string languageID)
        {
            var result = await _manageservice.GetallOrderSuccess(languageID);
            return Ok(result);
        }

        [HttpGet("get-all-order-confirm/{languageID}")]
        public async Task<IActionResult> GetallOrderConfirm(string languageID)
        {
            var result = await _manageservice.GetallOrderConfirm(languageID);
            return Ok(result);
        }

        [HttpGet("cancel-order/{userId}/{orderId}")]
        public async Task<IActionResult> CancelOrder(Guid userId, int orderId)
        {
            var result = await _manageservice.CancelOrder(userId, orderId);
            return Ok(result);
        }

        [HttpGet("confirm-order/{orderId}")]
        public async Task<IActionResult> ConfirmOrder(int orderId)
        {
            var result = await _manageservice.ConfirmOrder(orderId);
            return Ok(result);
        }

        [HttpGet("success-order/{orderId}")]
        public async Task<IActionResult> SuccessOrder(int orderId)
        {
            var result = await _manageservice.SuccessOrder(orderId);
            return Ok(result);
        }
    }
}