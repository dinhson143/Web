using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.AdminApp.Models;
using Web.ServiceApi_Admin_User.Service.Congtys;
using Web.ServiceApi_Admin_User.Service.PhieuNhaps;
using Web.ServiceApi_Admin_User.Service.Products;
using Web.Utilities.Contants;
using Web.ViewModels.Catalog.Congtys;
using Web.ViewModels.Catalog.PhieuNhaps;
using Web.ViewModels.Catalog.Products;

namespace Web.AdminApp.Controllers
{
    public class PhieuNhapController : Controller
    {
        private readonly IPhieuNhapApi _phieuNhapApi;
        private readonly ICongtyApi _congtyApi;
        private readonly IProductApi _productApi;

        public PhieuNhapController(IPhieuNhapApi phieuNhapApi, ICongtyApi congtyApi, IProductApi productApi)
        {
            _phieuNhapApi = phieuNhapApi;
            _congtyApi = congtyApi;
            _productApi = productApi;
        }

        public async Task<IActionResult> Index()
        {
            HttpContext.Session.SetString("phieunhapID", "0");
            var languageId = HttpContext.Session.GetString(SystemContants.AppSettings.DefaultLanguageId);
            var token = HttpContext.Session.GetString(SystemContants.AppSettings.Token);
            var result = await _phieuNhapApi.GetAll(token);
            return View(result.ResultObj);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var token = HttpContext.Session.GetString(SystemContants.AppSettings.Token);
            var languageId = HttpContext.Session.GetString(SystemContants.AppSettings.DefaultLanguageId);
            var response = await _phieuNhapApi.GetPhieuNhapById(id, languageId, token);
            HttpContext.Session.SetString("phieunhapID", id.ToString());
            HttpContext.Session.SetString("ProductID", "0");
            return View(response);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var result = await _congtyApi.GetAll();

            if (result.IsSuccess == false)
            {
                return null;
            }
            var data = new PhieuNhapCreateViewModel();
            data.Congtys = result.ResultObj;
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> Create(int CurrentCongtyId)
        {
            var request = new PhieuNhapCreate();
            request.CongTyId = CurrentCongtyId;
            if (!ModelState.IsValid)
            {
                return View();
            }
            var token = HttpContext.Session.GetString(SystemContants.AppSettings.Token);
            var result = await _phieuNhapApi.CreatePhieuNhap(request, token);

            if (result.IsSuccess == false)
            {
                ModelState.AddModelError("", result.Message);
                return View(request);
            }
            TempData["Message"] = "Thêm phiếu nhập thành công";
            return RedirectToAction("Index", "PhieuNhap");
        }

        [HttpGet]
        public async Task<IActionResult> CreateCTPN()
        {
            var languageId = HttpContext.Session.GetString(SystemContants.AppSettings.DefaultLanguageId);
            var token = HttpContext.Session.GetString(SystemContants.AppSettings.Token);
            var data = new CTPhieuNhapCreateViewModel();

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
        public async Task<IActionResult> CreateCTPN(CTPhieuNhapCreateViewModel request)
        {
            var maPN = int.Parse(HttpContext.Session.GetString("phieunhapID"));
            var data = new CTPhieuNhapCreate()
            {
                Dongia = request.Dongia,
                Giaban = request.Giaban,
                PhieuNXId = maPN,
                ProductId = request.ProductId,
                SizeId = request.SizeId,
                Soluong = request.Soluong
            };
            var token = HttpContext.Session.GetString(SystemContants.AppSettings.Token);
            var languageId = HttpContext.Session.GetString(SystemContants.AppSettings.DefaultLanguageId);
            var result = await _phieuNhapApi.CreateCTPhieuNhap(data, token);

            if (result == false)
            {
                var list = new CTPhieuNhapCreateViewModel();

                var model = new GetManageProductPagingRequest()
                {
                    LanguageId = languageId,
                    BearerToken = token
                };
                var pageResult = await _productApi.GetAll(model);

                foreach (var product in pageResult.Items)
                {
                    var result2 = await _productApi.GetProductSize(product.Id, token);
                    var ps = new SizeofProductViewModel()
                    {
                        Id = product.Id,
                        Name = product.Name,
                        listPS = result2.ResultObj
                    };
                    list.listProduct.Add(ps);
                }
                TempData["Message"] = "Thêm chi tiết phiếu nhập thất bại. Đã tồn tại CTPN";
                return View(list);
            }
            TempData["Message"] = "Thêm chi tiết phiếu nhập thành công";

            return RedirectToAction("Index", "PhieuNhap");
        }

        [HttpPost]
        public async Task<IActionResult> GetSizeProduct(int ProductId)
        {
            HttpContext.Session.SetString("ProductID", ProductId.ToString());
            return RedirectToAction("CreateCTPN", "PhieuNhap");
        }
    }
}