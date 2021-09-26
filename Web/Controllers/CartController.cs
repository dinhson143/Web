using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Web.Models;
using Web.ServiceApi_Admin_User.Service.Categories;
using Web.ServiceApi_Admin_User.Service.Products;
using Web.Utilities.Contants;

namespace Web.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductApi _productApi;
        private readonly ICategoryApi _categoryApi;

        public CartController(IProductApi productApi, ICategoryApi categoryApi)
        {
            _productApi = productApi;
            _categoryApi = categoryApi;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetListItem()
        {
            var session = HttpContext.Session.GetString(SystemContants.CartSession);
            List<CartItemViewModel> currentCart = new List<CartItemViewModel>();
            if (session != null)
            {
                currentCart = JsonConvert.DeserializeObject<List<CartItemViewModel>>(session);
            }
            return Ok(currentCart);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int id)
        {
            var culture = CultureInfo.CurrentCulture.Name;
            var product = await _productApi.GetProductById(id, "", culture);
            var result = await _productApi.GetListImage(id);
            var images = result.ResultObj;
            var session = HttpContext.Session.GetString(SystemContants.CartSession);
            List<CartItemViewModel> currentCart = new List<CartItemViewModel>();
            if (session != null)
            {
                currentCart = JsonConvert.DeserializeObject<List<CartItemViewModel>>(session);
            }
            int quantity = 1;
            if (currentCart.Any(x => x.ProductId == id))
            {
                quantity = currentCart.First(x => x.ProductId == id).Quantity + 1;
            }
            var cartItem = new CartItemViewModel()
            {
                ProductId = id,
                Description = product.ResultObj.Description,
                Name = product.ResultObj.Name,
                Quantity = quantity,
                Price = product.ResultObj.Price,
                Image = images[0].URL
            };

            currentCart.Add(cartItem);

            HttpContext.Session.SetString(SystemContants.CartSession, JsonConvert.SerializeObject(currentCart));
            return Ok(currentCart);
        }
    }
}