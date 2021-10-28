using LazZiya.ExpressLocalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Models;
using Web.ServiceApi_Admin_User.Service.Contacts;
using Web.ViewModels.Catalog.Contacts;

namespace Web.Controllers
{
    public class ContactController : Controller
    {
        private readonly ILogger<ContactController> _logger;
        private readonly ISharedCultureLocalizer _loc;
        private readonly IContactApi _contactApi;

        public ContactController(ILogger<ContactController> logger, ISharedCultureLocalizer loc, IContactApi contactApi)
        {
            _logger = logger;
            _loc = loc;
            _contactApi = contactApi;
        }

        public async Task<IActionResult> Index()
        {
            var contacts = await _contactApi.GetAll();
            var data = new ContactModel()
            {
                listCT = contacts.ResultObj
            };
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(ContactModel request)
        {
            var data = new ContactSendMail()
            {
                Email = request.Email,
                Message = request.Message,
                Name = request.Name,
                Subject = request.Subject
            };
            return View();
        }
    }
}