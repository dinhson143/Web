using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PayPal.Core;
using PayPal.Payments;
using System;
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
using Transaction = PayPal.Payments.Transaction;

namespace Web.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductApi _productApi;
        private readonly IOrderApi _orderApi;
        private readonly ICommentApi _commentApi;
        private readonly string _clientId;
        private readonly string _secretKey;

        public decimal TyGiaUSD = 23300;

        public CartController(IProductApi productApi, IOrderApi orderApi, ICommentApi commentApi, IConfiguration configuration)
        {
            _productApi = productApi;
            _orderApi = orderApi;
            _commentApi = commentApi;
            _clientId = configuration["PayPal:ClientId"];
            _secretKey = configuration["PayPal:SecretKey"];
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
                decimal percent = (item.DiscountPercent * (decimal)(0.01)) ?? 0;
                decimal amount = item.DiscountAmount ?? 0;
                orderDetails.Add(new OrderDetailVM()
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price - (item.Price * percent) - amount,
                    SizeId = item.SizeId
                });
            }

            var checkoutRequest = new CheckoutRequest()
            {
                Address = request.CheckoutModel.Address,
                Email = request.CheckoutModel.Email,
                Name = request.CheckoutModel.Name,
                PhoneNumber = request.CheckoutModel.PhoneNumber,
                OrderDetails = orderDetails,
                ThanhToan = "Normal"
            };
            string token = HttpContext.Session.GetString(SystemContants.AppSettings.Token);
            var result = await _orderApi.CreateOrder(checkoutRequest, token);
            if (result.IsSuccess == true)
            {
                TempData["msg"] = "Đặt hàng thành công";
                HttpContext.Session.Remove(SystemContants.CartSession);
            }
            else
            {
                TempData["msg"] = null;
            }
            return View(data);
        }

        public async Task<IActionResult> PaypalCheckout(CheckoutViewModel checkout)
        {
            var culture = CultureInfo.CurrentCulture.Name;
            var session = HttpContext.Session.GetString(SystemContants.CartSession);
            List<CartItemViewModel> currentCart = new List<CartItemViewModel>();
            if (session != null)
            {
                currentCart = JsonConvert.DeserializeObject<List<CartItemViewModel>>(session);
                checkout.CartItems = currentCart;
            }
            if (currentCart.Count == 0)
            {
                return RedirectToAction("Index", "Cart");
            }
            HttpContext.Session.SetString(SystemContants.CheckoutVMD, JsonConvert.SerializeObject(checkout));
            decimal tempTotalAmount = 0;
            foreach (var item in currentCart)
            {
                tempTotalAmount += (item.Quantity * item.Price);
            }
            var environment = new SandboxEnvironment(_clientId, _secretKey);
            var client = new PayPalHttpClient(environment);

            var total = Math.Round(tempTotalAmount / TyGiaUSD, 2);
            var paypalOrderId = DateTime.Now.Ticks;
            var hostname = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
            var payment = new Payment()
            {
                Intent = "sale",
                Transactions = new List<Transaction>()
                {
                    new Transaction()
                    {
                        Amount = new Amount()
                        {
                            Total = total.ToString(CultureInfo.InvariantCulture),
                            Currency = "USD",
                            Details = new AmountDetails
                            {
                                Tax = "0",
                                Shipping = "0",
                                Subtotal = total.ToString(CultureInfo.InvariantCulture)
                            }
                        },
						//ItemList = itemList,
						Description = $"Invoice #{paypalOrderId}",
                        InvoiceNumber = paypalOrderId.ToString()
                    }
                },
                RedirectUrls = new RedirectUrls()
                {
                    CancelUrl = $"{hostname}/{culture}/Cart/CheckoutFail",
                    ReturnUrl = $"{hostname}/{culture}/Cart/CheckoutSuccess"
                },
                Payer = new Payer()
                {
                    PaymentMethod = "paypal"
                }
            };
            PaymentCreateRequest request = new PaymentCreateRequest();
            request.RequestBody(payment);

            try
            {
                var response = await client.Execute(request);
                var statusCode = response.StatusCode;
                Payment result = response.Result<Payment>();

                var links = result.Links.GetEnumerator();
                string paypalRedirectUrl = null;
                while (links.MoveNext())
                {
                    LinkDescriptionObject lnk = links.Current;
                    if (lnk.Rel.ToLower().Trim().Equals("approval_url"))
                    {
                        //saving the payapalredirect URL to which user will be redirected for payment
                        paypalRedirectUrl = lnk.Href;
                    }
                }

                return Redirect(paypalRedirectUrl);
            }
            //catch (Exception ex)
            //{
            //    var message = ex.Message;

            //    //Process when Checkout with Paypal fails
            //    return Redirect("/Cart/CheckoutFail");
            //}
            catch (BraintreeHttp.HttpException httpException)
            {
                var statusCode = httpException.StatusCode;
                var debugId = httpException.Headers.GetValues("PayPal-Debug-Id").FirstOrDefault();

                //Process when Checkout with Paypal fails
                return Redirect($"{culture}/Cart/CheckoutFail");
            }
        }

        public IActionResult CheckoutFail()
        {
            //Tạo đơn hàng trong database với trạng thái thanh toán là "Chưa thanh toán"
            //Xóa session
            TempData["err"] = null;
            return RedirectToAction("Checkout");
        }

        public async Task<IActionResult> CheckoutSuccess()
        {
            var session = HttpContext.Session.GetString(SystemContants.CheckoutVMD);
            var data = JsonConvert.DeserializeObject<CheckoutViewModel>(session);
            //Tạo đơn hàng trong database với trạng thái thanh toán là "Paypal" và thành công
            var orderDetails = new List<OrderDetailVM>();
            foreach (var item in data.CartItems)
            {
                decimal percent = (item.DiscountPercent * (decimal)(0.01)) ?? 0;
                decimal amount = item.DiscountAmount ?? 0;
                orderDetails.Add(new OrderDetailVM()
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price - (item.Price * percent) - amount,
                    SizeId = item.SizeId
                });
            }

            var checkoutRequest = new CheckoutRequest()
            {
                Address = data.CheckoutModel.Address,
                Email = data.CheckoutModel.Email,
                Name = data.CheckoutModel.Name,
                PhoneNumber = data.CheckoutModel.PhoneNumber,
                OrderDetails = orderDetails,
                ThanhToan = "PayPal"
            };
            string token = HttpContext.Session.GetString(SystemContants.AppSettings.Token);
            var result = await _orderApi.CreateOrder(checkoutRequest, token);
            if (result.IsSuccess == true)
            {
                TempData["msg"] = "Đặt hàng thành công";
                HttpContext.Session.Remove(SystemContants.CartSession);
            }
            else
            {
                TempData["msg"] = null;
            }
            //Xóa session
            HttpContext.Session.Remove(SystemContants.CheckoutVMD);
            return RedirectToAction("Checkout");
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
                    NameSize = namesize,
                    DiscountAmount = product.ResultObj.DiscountAmount,
                    DiscountPercent = product.ResultObj.DiscountPercent
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