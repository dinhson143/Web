using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.ServiceApi_Admin_User.Service.Congtys;
using Web.ServiceApi_Admin_User.Service.PhieuXuats;
using Web.ServiceApi_Admin_User.Service.Products;

namespace Web.AdminApp.Controllers
{
    public class PhieuXuatController : Controller
    {
        private readonly IPhieuXuatApi _phieuXuatApi;
        private readonly ICongtyApi _congtyApi;
        private readonly IProductApi _productApi;

        public PhieuXuatController(IPhieuXuatApi phieuXuatApi, ICongtyApi congtyApi, IProductApi productApi)
        {
            _phieuXuatApi = phieuXuatApi;
            _congtyApi = congtyApi;
            _productApi = productApi;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}