using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.ServiceApi_Admin_User.Service.Contacts;
using Web.Utilities.Contants;
using Web.ViewModels.Catalog.Contacts;

namespace Web.AdminApp.Controllers
{
    public class ContactController : Controller
    {
        private readonly IContactApi _contactApi;

        public ContactController(IContactApi contactApi)
        {
            _contactApi = contactApi;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _contactApi.GetAll();
            var contacts = result.ResultObj;
            return View(contacts);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ContactCreate request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var token = HttpContext.Session.GetString(SystemContants.AppSettings.Token);
            var result = await _contactApi.CreateContact(request, token);

            if (result.IsSuccess == false)
            {
                ModelState.AddModelError("", result.Message);
                return View(request);
            }
            TempData["Message"] = "Thêm Contacts thành công";
            return RedirectToAction("Index", "Contact");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var languageId = HttpContext.Session.GetString(SystemContants.AppSettings.DefaultLanguageId);
            var contact = await _contactApi.GetContactById(id);
            var editVm = new ContactViewModel()
            {
                Id = id,
                Name = contact.Name,
                Email = contact.Email,
                Message = contact.Message,
                PhoneNumber = contact.PhoneNumber
            };
            return View(editVm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] ContactViewModel request)
        {
            var token = HttpContext.Session.GetString(SystemContants.AppSettings.Token);
            var result = await _contactApi.Update(request, token);
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
            return RedirectToAction("Index", "Contact");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int Id)
        {
            var token = HttpContext.Session.GetString(SystemContants.AppSettings.Token);
            var result = await _contactApi.Delete(Id, token);
            string message = "";
            if (result == true)
            {
                message = "Xóathành công";
                TempData["Message"] = message;
            }
            else
            {
                message = "Xóa thất bại";
                TempData["Message"] = message;
            }
            return RedirectToAction("Index", "Contact");
        }
    }
}