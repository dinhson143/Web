using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Application.Catalog.Contacts;
using Web.ViewModels.Catalog.Contacts;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ContactsController : ControllerBase
    {
        private readonly IContactService _contactService;

        public ContactsController(IContactService contactService)
        {
            _contactService = contactService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ContactCreate request)
        {
            var result = await _contactService.CreateContact(request);
            return Ok(result);
        }

        [HttpGet("danh-sach")]
        [AllowAnonymous]
        public async Task<List<ContactViewModel>> GetAll()
        {
            var result = await _contactService.GetAll();
            return result;
        }

        [HttpDelete("Delete/{contactId}")]
        public async Task<IActionResult> Delete(int contactId)
        {
            var result = await _contactService.Delete(contactId);
            return Ok(result);
        }

        [HttpPut("{contactId}")]
        public async Task<IActionResult> Update(int contactId, [FromBody] ContactViewModel request)
        {
            request.Id = contactId;
            var result = await _contactService.UpdateContact(request);
            return Ok(result);
        }

        [HttpGet("contact_detail/{contactId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetContactById(int contactId)
        {
            var Category = await _contactService.GetContactById(contactId);
            return Ok(Category);
        }
    }
}