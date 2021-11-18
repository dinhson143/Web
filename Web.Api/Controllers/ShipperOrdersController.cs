using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Application.Catalog.ShipperOrder;
using Web.ViewModels.Catalog.ShipperOrders;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ShipperOrdersController : ControllerBase
    {
        private readonly IShipperOrderService _manageservice;

        public ShipperOrdersController(IShipperOrderService manageservice)
        {
            _manageservice = manageservice;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ShipperOrderCreate request)
        {
            var result = await _manageservice.CreateShipperOrder(request);
            return Ok(result);
        }

        [HttpGet("danh-sach-order-shipper/{shipperId}")]
        [Authorize]
        public async Task<IActionResult> GetOrderUser(Guid shipperId)
        {
            var result = await _manageservice.GetAll(shipperId);
            return Ok(result);
        }

        [HttpGet("danh-sach-order-shipper-wait-confirm/{languageID}/{ShipperID}")]
        [Authorize]
        public async Task<IActionResult> GetallOrderSPrequest(Guid ShipperID,string languageID)
        {
            var result = await _manageservice.GetallOrderSPrequest(ShipperID,languageID);
            return Ok(result);
        }

        [HttpGet("lich-su-order-shipper/{shipperId}")]
        [Authorize]
        public async Task<IActionResult> GetAll_HistorySP(Guid shipperId)
        {
            var result = await _manageservice.GetAll_HistorySP(shipperId);
            return Ok(result);
        }

        [HttpGet("confirm-orderSP/{orderId}/{shipperId}")]
        public async Task<IActionResult> ConfirmOrderSP(int orderId, Guid ShipperID)
        {
            var result = await _manageservice.ConfirmOrderSP(orderId,ShipperID);
            return Ok(result);
        }
    }
}