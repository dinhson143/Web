using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Web.ServiceApi_Admin_User.Service.Roles;

namespace Web.AdminApp.Controllers
{
    public class RoleController : Controller
    {
        private readonly IRoleApi _roleApi;
        private readonly IConfiguration _config;

        public RoleController(IRoleApi roleApi, IConfiguration config)
        {
            _roleApi = roleApi;
            _config = config;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}