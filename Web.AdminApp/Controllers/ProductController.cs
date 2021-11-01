using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.AdminApp.Models;
using Web.ServiceApi_Admin_User.Service.Categories;
using Web.ServiceApi_Admin_User.Service.Orders;
using Web.ServiceApi_Admin_User.Service.PhieuNhaps;
using Web.ServiceApi_Admin_User.Service.PhieuXuats;
using Web.ServiceApi_Admin_User.Service.Products;
using Web.ServiceApi_Admin_User.Service.Sizes;
using Web.Utilities.Contants;
using Web.ViewModels.Catalog.Categories;
using Web.ViewModels.Catalog.Common;
using Web.ViewModels.Catalog.PhieuNhaps;
using Web.ViewModels.Catalog.Products;
using Web.ViewModels.Catalog.Sizes;

namespace Web.AdminApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductApi _productApi;
        private readonly IConfiguration _config;
        private readonly ICategoryApi _categoryApi;
        private readonly ISizeApi _sizeApi;
        private readonly IPhieuNhapApi _phieuNhapApi;
        private readonly IOrderApi _orderApi;
        private readonly IPhieuXuatApi _phieuXuatApi;

        public ProductController(IProductApi productApi, IPhieuXuatApi phieuXuatApi, IOrderApi orderApi, IPhieuNhapApi phieuNhapApi, IConfiguration config, ICategoryApi categoryApi, ISizeApi sizeApi)
        {
            _productApi = productApi;
            _config = config;
            _categoryApi = categoryApi;
            _sizeApi = sizeApi;
            _phieuNhapApi = phieuNhapApi;
            _orderApi = orderApi;
            _phieuXuatApi = phieuXuatApi;
        }

        public async Task<IActionResult> Index(int? categoryId)
        {
            var languageId = HttpContext.Session.GetString(SystemContants.AppSettings.DefaultLanguageId);
            var token = HttpContext.Session.GetString(SystemContants.AppSettings.Token);

            var result = await _categoryApi.GetAll(languageId, token);
            var categories = new List<CategoryViewModel>();
            foreach (var item in result.ResultObj)
            {
                if (item.ParentId != null)
                {
                    categories.Add(item);
                }
            }
            ViewBag.Categories = categories.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString(),
                Selected = categoryId.HasValue && categoryId == x.Id
            });
            var model = new GetManageProductPagingRequest()
            {
                LanguageId = languageId,
                BearerToken = token,
                CategoryId = categoryId
            };
            var pageResult = await _productApi.GetAll(model);

            return View(pageResult.Items);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> Create([FromForm] ProductCreate request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            request.BearerToken = HttpContext.Session.GetString(SystemContants.AppSettings.Token);
            request.LanguageId = HttpContext.Session.GetString(SystemContants.AppSettings.DefaultLanguageId);
            var result = await _productApi.CreateProduct(request);
            string message = "";
            if (result.IsSuccess == true)
            {
                message = "Thêm mới thành công";
                TempData["Message"] = message;
            }
            else
            {
                ModelState.AddModelError("", result.Message);
                return View(request);
            }
            return RedirectToAction("Index", "Product");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var token = HttpContext.Session.GetString(SystemContants.AppSettings.Token);
            var languageId = HttpContext.Session.GetString(SystemContants.AppSettings.DefaultLanguageId);
            var response = await _productApi.GetProductById(id, token, languageId);
            var images = await _productApi.GetListImage(id);

            response.ResultObj.Images = images.ResultObj;
            if (response.IsSuccess != false)
            {
                return View(response.ResultObj);
            }
            return RedirectToAction("Error", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> AssignCategory(int id)
        {
            var response = await GetCategoryAssignRequets(id);
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> AssignCategory(CategoryAssignRequest request)
        {
            var session = HttpContext.Session.GetString("Token");
            request.BearerToken = session;
            var response = await _productApi.AssignCategory(request.Id, request);
            if (response.IsSuccess)
            {
                TempData["Message"] = "Gán danh mục thành công";
                return RedirectToAction("Index", "Product");
            }
            var roles = await GetCategoryAssignRequets(request.Id);
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> AssignSize(int id)
        {
            var response = await GetSizeAssignRequets(id);
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> AssignSize(SizeAssignRequest request)
        {
            var session = HttpContext.Session.GetString("Token");
            request.BearerToken = session;
            var response = await _productApi.AssignSize(request.Id, request);
            if (response.IsSuccess)
            {
                TempData["Message"] = "Gán size thành công";
                return RedirectToAction("Index", "Product");
            }
            var sizes = await GetSizeAssignRequets(request.Id);
            return View();
        }

        private async Task<CategoryAssignRequest> GetCategoryAssignRequets(int productId)
        {
            var languageId = HttpContext.Session.GetString(SystemContants.AppSettings.DefaultLanguageId);
            var token = HttpContext.Session.GetString(SystemContants.AppSettings.Token);
            var result = await _categoryApi.GetAll(languageId, token);

            if (result.IsSuccess == false)
            {
                return null;
            }

            var categories = new List<CategoryViewModel>();
            foreach (var item in result.ResultObj)
            {
                if (item.ParentId != null)
                {
                    categories.Add(item);
                }
            }
            var data = await _productApi.GetProductById(productId, token, languageId);
            var product = data.ResultObj;

            var CategoryAssignRequest = new CategoryAssignRequest();
            foreach (var category in categories)
            {
                CategoryAssignRequest.Categories.Add(new SelectItems()
                {
                    Id = category.Id.ToString(),
                    Name = category.Name,
                    Selected = product.Categories.Contains(category.Name)
                });
            }
            return CategoryAssignRequest;
        }

        private async Task<SizeAssignRequest> GetSizeAssignRequets(int productId)
        {
            var languageId = HttpContext.Session.GetString(SystemContants.AppSettings.DefaultLanguageId);
            var token = HttpContext.Session.GetString(SystemContants.AppSettings.Token);
            var result = await _sizeApi.GetAll(token);

            if (result.IsSuccess == false)
            {
                return null;
            }

            var sizes = new List<SizeViewModel>();
            foreach (var item in result.ResultObj)
            {
                sizes.Add(item);
            }
            var data = await _productApi.GetProductById(productId, token, languageId);
            var product = data.ResultObj;

            var SizeAssignRequest = new SizeAssignRequest();
            foreach (var size in sizes)
            {
                SizeAssignRequest.Sizes.Add(new SelectItems()
                {
                    Id = size.Id.ToString(),
                    Name = size.Name,
                    Selected = product.Sizes.Contains(size.Name)
                });
            }
            return SizeAssignRequest;
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var languageId = HttpContext.Session.GetString(SystemContants.AppSettings.DefaultLanguageId);
            var token = HttpContext.Session.GetString(SystemContants.AppSettings.Token);
            var result = await _productApi.GetProductById(id, token, languageId);
            var product = result.ResultObj;
            bool kiemtraFeatured;
            if (product.IsFeatured == null)
            {
                kiemtraFeatured = false;
            }
            else
            {
                kiemtraFeatured = (bool)product.IsFeatured;
            }
            var editVm = new ProductUpdateRequest()
            {
                Id = product.Id,
                Description = product.Description,
                Details = product.Details,
                Name = product.Name,
                SeoAlias = product.SeoAlias,
                SeoDescription = product.SeoDescription,
                SeoTitle = product.SeoTitle,
                IsFeatured = kiemtraFeatured
            };
            return View(editVm);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> Edit([FromForm] ProductUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            request.BearerToken = HttpContext.Session.GetString(SystemContants.AppSettings.Token);
            request.LanguageId = HttpContext.Session.GetString(SystemContants.AppSettings.DefaultLanguageId);
            var result = await _productApi.Update(request);
            string message = "";
            if (result == true)
            {
                message = "Chình sửa thành công";
                TempData["Message"] = message;
            }
            else
            {
                return View(request);
            }
            return RedirectToAction("Index", "Product");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int Id)
        {
            var token = HttpContext.Session.GetString(SystemContants.AppSettings.Token);
            var response = await _productApi.DeleteProduct(Id, token);
            if (response == true)
            {
                TempData["Message"] = "Xóa product thành công";
                return RedirectToAction("Index", "Product");
            }
            else
            {
                TempData["Message"] = "Xóa product thất bại";
                return RedirectToAction("Index", "Product");
            }
        }

        [HttpGet]
        public async Task<IActionResult> UpdatePrice()
        {
            var languageId = HttpContext.Session.GetString(SystemContants.AppSettings.DefaultLanguageId);
            var token = HttpContext.Session.GetString(SystemContants.AppSettings.Token);
            var data = new UpdatePriceViewModel();

            var model = new GetManageProductPagingRequest()
            {
                LanguageId = languageId,
                BearerToken = token
            };
            var pageResult = await _productApi.GetAll(model);

            foreach (var product in pageResult.Items)
            {
                var result = await _productApi.GetProductSize(product.Id, token);
                var ps = new SizeofProductViewModel()
                {
                    Id = product.Id,
                    Name = product.Name,
                    listPS = result.ResultObj
                };
                data.listProduct.Add(ps);
            }
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePrice(UpdatePriceViewModel request)
        {
            var data = new UpdatePriceRequest()
            {
                Price = request.Price,
                ProductId = request.ProductId,
                SizeId = request.SizeId,
            };
            var token = HttpContext.Session.GetString(SystemContants.AppSettings.Token);
            var result = await _productApi.UpdatePrice(data, token);

            if (result == false)
            {
                ModelState.AddModelError("", "Cập nhật giá thất bại");
                return View(request);
            }
            TempData["Message"] = "Cập nhật giá thành công";
            return RedirectToAction("Index", "Product");
        }

        public async Task<IActionResult> Nhap_Xuat_TonKho()
        {
            var languageId = HttpContext.Session.GetString(SystemContants.AppSettings.DefaultLanguageId);
            var token = HttpContext.Session.GetString(SystemContants.AppSettings.Token);

            HttpContext.Session.SetString("phieunhapID", "0");
            var pn = await _phieuNhapApi.GetAll(token);
            var px = await _phieuXuatApi.GetAll(token);
            //
            var model = new GetManageProductPagingRequest()
            {
                LanguageId = languageId,
                BearerToken = token,
                CategoryId = 0
            };
            var products = await _productApi.GetAll(model);

            //
            var data = new Nhap_Xuat_TonKho_ViewModel();
            data.PhieuNhap = pn.ResultObj;
            data.PhieuXuat = px.ResultObj;
            data.Products = products.Items;
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> Nhap_Xuat_TonKhoDetail(int Id)
        {
            var token = HttpContext.Session.GetString(SystemContants.AppSettings.Token);
            var languageId = HttpContext.Session.GetString(SystemContants.AppSettings.DefaultLanguageId);
            var response = await _phieuNhapApi.GetPhieuNhapById(Id, languageId, token);
            HttpContext.Session.SetString("phieunhapID", Id.ToString());
            HttpContext.Session.SetString("ProductID", "0");
            return View(response);
        }

        [HttpGet]
        public async Task<IActionResult> ProductStock(int id)
        {
            var token = HttpContext.Session.GetString(SystemContants.AppSettings.Token);
            var languageId = HttpContext.Session.GetString(SystemContants.AppSettings.DefaultLanguageId);
            var response = await _productApi.GetProductById(id, token, languageId);
            if (response.IsSuccess != false)
            {
                return View(response.ResultObj);
            }
            return RedirectToAction("Error", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> EditImageProduct(int productImageId)
        {
            var images = await _productApi.GetListImage(productImageId);
            HttpContext.Session.SetString("productIDImage", productImageId.ToString());
            return View(images.ResultObj);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteImage(int id)
        {
            var token = HttpContext.Session.GetString(SystemContants.AppSettings.Token);
            var response = await _productApi.DeleteImageProduct(id, token);
            var productImageId = HttpContext.Session.GetString("productIDImage");
            HttpContext.Session.Remove("productIDImage");
            if (response == true)
            {
                TempData["Message"] = "Xóa Image thành công";
                return RedirectToAction("EditImageProduct", "Product", new { productImageId });
            }
            else
            {
                TempData["Message"] = "Xóa Image thất bại";
                return RedirectToAction("EditImageProduct", "Product", new { productImageId });
            }
        }

        [HttpGet]
        public IActionResult AddImage()
        {
            return View();
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> AddImage([FromForm] ProductUpdateRequest request)
        {
            var productImageId = HttpContext.Session.GetString("productIDImage");
            var token = HttpContext.Session.GetString(SystemContants.AppSettings.Token);
            request.Id = int.Parse(productImageId);
            request.BearerToken = token;
            request.LanguageId = HttpContext.Session.GetString(SystemContants.AppSettings.DefaultLanguageId);
            var result = await _productApi.AddImage(request);
            string message = "";
            if (result == true)
            {
                message = "Thêm ảnh thành công";
                TempData["Message"] = message;
            }
            else
            {
                return View(request);
            }
            return RedirectToAction("EditImageProduct", "Product", new { productImageId });
        }
    }
}