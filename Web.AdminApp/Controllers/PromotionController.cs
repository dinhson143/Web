using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.AdminApp.Models;
using Web.ServiceApi_Admin_User.Service.Categories;
using Web.ServiceApi_Admin_User.Service.Products;
using Web.ServiceApi_Admin_User.Service.Promotions;
using Web.Utilities.Contants;
using Web.ViewModels.Catalog.Categories;
using Web.ViewModels.Catalog.Common;
using Web.ViewModels.Catalog.Products;
using Web.ViewModels.Catalog.Promotions;

namespace Web.AdminApp.Controllers
{
    public class PromotionController : Controller
    {
        private readonly IProductApi _productApi;
        private readonly ICategoryApi _categoryApi;
        private readonly IPromotionApi _promotionApi;

        public PromotionController(IProductApi productApi, ICategoryApi categoryApi, IPromotionApi promotionApi)
        {
            _productApi = productApi;
            _categoryApi = categoryApi;
            _promotionApi = promotionApi;
        }

        public async Task<IActionResult> Index()
        {
            var languageId = HttpContext.Session.GetString(SystemContants.AppSettings.DefaultLanguageId);
            var token = HttpContext.Session.GetString(SystemContants.AppSettings.Token);
            var promotions = await _promotionApi.GetAll();
            foreach (var item in promotions.ResultObj)
            {
                string[] product = item.ProductIDs.Split(',');
                foreach (var pId in product)
                {
                    if (pId != "")
                    {
                        var productId = Int32.Parse(pId);
                        var dataPro = await _productApi.GetProductById(productId, token, languageId);
                        var imagePro = await _productApi.GetListImage(productId);
                        dataPro.ResultObj.Images = imagePro.ResultObj;
                        item.listPro.Add(dataPro.ResultObj);
                    }
                }
            }
            foreach (var item in promotions.ResultObj)
            {
                string[] category = item.ProductCategoryIds.Split(',');
                foreach (var cId in category)
                {
                    if (cId != "")
                    {
                        var cateId = Int32.Parse(cId);
                        var dataCate = await _categoryApi.GetCategoryById(cateId, languageId);
                        item.listCate.Add(dataCate);
                    }
                }
            }
            return View(promotions.ResultObj);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var languageId = HttpContext.Session.GetString(SystemContants.AppSettings.DefaultLanguageId);
            var token = HttpContext.Session.GetString(SystemContants.AppSettings.Token);
            var model = new GetManageProductPagingRequest()
            {
                LanguageId = languageId,
                BearerToken = token,
                CategoryId = null
            };
            var pageResult = await _productApi.GetAll(model);
            var result = await _categoryApi.GetAll(languageId, token);
            var categories = new List<CategoryViewModel>();
            foreach (var item in result.ResultObj)
            {
                if (item.ParentId != null)
                {
                    categories.Add(item);
                }
            }

            var listCate = new List<SelectItems>();
            var listPro = new List<SelectItems>();
            foreach (var item in categories)
            {
                var x = new SelectItems()
                {
                    Id = item.Id.ToString(),
                    Name = item.Name,
                    Selected = false
                };
                listCate.Add(x);
            }
            foreach (var item in pageResult.Items)
            {
                var x = new SelectItems()
                {
                    Id = item.Id.ToString(),
                    Name = item.Name,
                    Selected = false
                };
                listPro.Add(x);
            }
            var data = new PromotionCreateModel()
            {
                listCate = listCate,
                listPro = listPro
            };
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PromotionCreateModel request)
        {
            var languageId = HttpContext.Session.GetString(SystemContants.AppSettings.DefaultLanguageId);
            var token = HttpContext.Session.GetString(SystemContants.AppSettings.Token);
            if (!ModelState.IsValid)
            {
                var model = new GetManageProductPagingRequest()
                {
                    LanguageId = languageId,
                    BearerToken = token,
                    CategoryId = null
                };
                var pageResult = await _productApi.GetAll(model);
                var result = await _categoryApi.GetAll(languageId, token);
                var categories = new List<CategoryViewModel>();
                foreach (var item in result.ResultObj)
                {
                    if (item.ParentId != null)
                    {
                        categories.Add(item);
                    }
                }

                var listCate = new List<SelectItems>();
                var listPro = new List<SelectItems>();
                foreach (var item in categories)
                {
                    var x = new SelectItems()
                    {
                        Id = item.Id.ToString(),
                        Name = item.Name,
                        Selected = false
                    };
                    listCate.Add(x);
                }
                foreach (var item in pageResult.Items)
                {
                    var x = new SelectItems()
                    {
                        Id = item.Id.ToString(),
                        Name = item.Name,
                        Selected = false
                    };
                    listPro.Add(x);
                }
                var dulieu = new PromotionCreateModel()
                {
                    listCate = listCate,
                    listPro = listPro
                };
                return View(dulieu);
            }

            string productIds = "";
            string CategoryIds = "";
            foreach (var item in request.listPro)
            {
                if (item.Selected)
                {
                    productIds += item.Id + ",";
                }
            }
            foreach (var item in request.listCate)
            {
                if (item.Selected)
                {
                    CategoryIds += item.Id + ",";
                }
            }
            var data = new PromotionCreate()
            {
                ApplyAll = request.ApplyAll,
                DiscountAmount = request.DiscountAmount,
                DiscountPercent = request.DiscountPercent,
                FromDate = request.FromDate,
                ToDate = request.ToDate,
                Name = request.Name,
                ProductCategoryIds = CategoryIds,
                ProductIDs = productIds
            };

            var kiemtra = await _promotionApi.CreatePromotion(data, token);

            if (kiemtra.IsSuccess == false)
            {
                ModelState.AddModelError("", kiemtra.Message);
                return View(request);
            }
            TempData["Message"] = "Thêm khuyến mãi thành công";
            return RedirectToAction("Index", "Promotion");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var languageId = HttpContext.Session.GetString(SystemContants.AppSettings.DefaultLanguageId);
            var token = HttpContext.Session.GetString(SystemContants.AppSettings.Token);
            var promotions = await _promotionApi.GetAll();
            var listData = new PromotionViewModel();
            foreach (var item in promotions.ResultObj)
            {
                if (item.Id == id)
                {
                    listData = item;
                    break;
                }
            }

            string[] product = listData.ProductIDs.Split(',');
            foreach (var pId in product)
            {
                if (pId != "")
                {
                    var productId = Int32.Parse(pId);
                    var dataPro = await _productApi.GetProductById(productId, token, languageId);
                    var imagePro = await _productApi.GetListImage(productId);
                    dataPro.ResultObj.Images = imagePro.ResultObj;
                    listData.listPro.Add(dataPro.ResultObj);
                }
            }

            string[] category = listData.ProductCategoryIds.Split(',');
            foreach (var cId in category)
            {
                if (cId != "")
                {
                    var cateId = Int32.Parse(cId);
                    var dataCate = await _categoryApi.GetCategoryById(cateId, languageId);
                    listData.listCate.Add(dataCate);
                }
            }
            return View(listData);
        }

        [HttpGet]
        public async Task<IActionResult> Block(int id)
        {
            var token = HttpContext.Session.GetString(SystemContants.AppSettings.Token);
            var result = await _promotionApi.Block(id, token);
            string message = "";
            if (result == true)
            {
                message = "Cập nhật trạng thái sự kiện thành công";
                TempData["Message"] = message;
            }
            else
            {
                message = "Cập nhật trạng thái sự kiện thất bại";
                TempData["Message"] = message;
            }
            return RedirectToAction("Index", "Promotion");
        }
    }
}