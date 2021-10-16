using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Web.Models;
using Web.ServiceApi_Admin_User.Service.Comments;
using Web.ServiceApi_Admin_User.Service.Orders;
using Web.ServiceApi_Admin_User.Service.Products;
using Web.Utilities.Contants;
using Web.ViewModels.Catalog.Comments;
using Web.ViewModels.Catalog.Sales;

namespace Web.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductApi _productApi;
        private readonly IOrderApi _orderApi;
        private readonly ICommentApi _commentApi;

        public CartController(IProductApi productApi, IOrderApi orderApi, ICommentApi commentApi)
        {
            _productApi = productApi;
            _orderApi = orderApi;
            _commentApi = commentApi;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Checkout()
        {
            var data = GetCheckOutViewModel();
            if (data == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutViewModel request)
        {
            var data = GetCheckOutViewModel();
            if (data == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var orderDetails = new List<OrderDetailVM>();
            foreach (var item in data.CartItems)
            {
                orderDetails.Add(new OrderDetailVM()
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    SizeId = item.SizeId
                });
            }
            var checkoutRequest = new CheckoutRequest()
            {
                Address = request.CheckoutModel.Address,
                Email = request.CheckoutModel.Email,
                Name = request.CheckoutModel.Name,
                PhoneNumber = request.CheckoutModel.PhoneNumber,
                OrderDetails = orderDetails
            };
            string token = HttpContext.Session.GetString(SystemContants.AppSettings.Token);
            var result = await _orderApi.CreateOrder(checkoutRequest, token);
            if (result.IsSuccess == true)
            {
                TempData["msg"] = "Đặt hàng thành công";
            }
            else
            {
                TempData["msg"] = null;
            }
            return View(data);
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
        public async Task<IActionResult> AddToCart(int id, int qty, int size, string namesize)
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
            int quantity = qty;
            if (currentCart.Any(x => x.ProductId == id))
            {
                quantity = currentCart.First(x => x.ProductId == id).Quantity + qty;
            }
            var dem = 0;
            foreach (var cart in currentCart)
            {
                if (cart.ProductId == id)
                {
                    cart.Quantity = quantity;
                    dem++;
                    break;
                }
            }
            // get giá
            decimal gia = 0;
            foreach (var item in product.ResultObj.listPS)
            {
                if (item.SizeId == size)
                {
                    gia = item.Price;
                }
            }
            if (dem == 0)
            {
                var cartItem = new CartItemViewModel()
                {
                    ProductId = id,
                    Description = product.ResultObj.Description,
                    Name = product.ResultObj.Name,
                    Quantity = quantity,
                    Price = gia,
                    Image = images[0].URL,
                    SizeId = size,
                    NameSize = namesize
                };

                currentCart.Add(cartItem);
            }

            HttpContext.Session.SetString(SystemContants.CartSession, JsonConvert.SerializeObject(currentCart));
            return Ok(currentCart);
        }

        public IActionResult UpdateCart(int id, int quantity)
        {
            var session = HttpContext.Session.GetString(SystemContants.CartSession);
            List<CartItemViewModel> currentCart = new List<CartItemViewModel>();
            if (session != null)
            {
                currentCart = JsonConvert.DeserializeObject<List<CartItemViewModel>>(session);
            }
            foreach (var item in currentCart)
            {
                if (item.ProductId == id)
                {
                    if (quantity == 0)
                    {
                        currentCart.Remove(item);
                        break;
                    }
                    item.Quantity = quantity;
                }
            }

            HttpContext.Session.SetString(SystemContants.CartSession, JsonConvert.SerializeObject(currentCart));
            return Ok(currentCart);
        }

        private CheckoutViewModel GetCheckOutViewModel()
        {
            var identity = (ClaimsIdentity)User.Identity;
            var claimsPrincipal = new ClaimsPrincipal(identity);

            // Get the claims values
            var name = identity.Claims.Where(c => c.Type == ClaimTypes.Name)
                               .Select(c => c.Value).SingleOrDefault();
            var email = identity.Claims.Where(c => c.Type == ClaimTypes.Email)
                               .Select(c => c.Value).SingleOrDefault();
            var address = identity.Claims.Where(c => c.Type == ClaimTypes.StreetAddress)
                               .Select(c => c.Value).SingleOrDefault();
            var phone = identity.Claims.Where(c => c.Type == ClaimTypes.MobilePhone)
                               .Select(c => c.Value).SingleOrDefault();

            if (name == null && email == null && address == null && phone == null)
            {
                return null;
            }
            var session = HttpContext.Session.GetString(SystemContants.CartSession);
            List<CartItemViewModel> currentCart = new List<CartItemViewModel>();
            if (session != null)
            {
                currentCart = JsonConvert.DeserializeObject<List<CartItemViewModel>>(session);
            }
            var checkoutVM = new CheckoutViewModel()
            {
                CartItems = currentCart,
                CheckoutModel = new CheckoutRequest()
                {
                    Address = address,
                    Email = email,
                    Name = name,
                    PhoneNumber = phone
                }
            };
            return checkoutVM;
        }

        [HttpPost]
        public async Task<string> RatingProduct(int productID, int star, string comment)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            bool check = false;
            var identity = (ClaimsIdentity)User.Identity;
            var claimsPrincipal = new ClaimsPrincipal(identity);

            // Get the claims values
            var email = identity.Claims.Where(c => c.Type == ClaimTypes.Email)
                               .Select(c => c.Value).SingleOrDefault();

            if (email == null)
            {
                data.Add("mgs", check);
                return JsonConvert.SerializeObject(data);
            }
            if (comment == null)
            {
                comment = "Đã đánh giá";
            }
            var request = new CommentCreate()
            {
                Content = comment,
                ProductId = productID,
                Star = star,
                Email = email
            };
            var token = HttpContext.Session.GetString(SystemContants.AppSettings.Token);
            var result = await _commentApi.CreateComment(request, token);
            check = true;
            data.Add("mgs", check);
            return JsonConvert.SerializeObject(data);
        }
    }
}