using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Application.Catalog.Languages;
using Web.ViewModels.Catalog.Languages;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LanguagesController : ControllerBase
    {
        private readonly ILanguageService _languageService;

        public LanguagesController(ILanguageService languageService)
        {
            _languageService = languageService;
        }

        [HttpGet("languages")]
        [AllowAnonymous]
        public async Task<List<LanguageViewModel>> GetAll()
        {
            var result = await _languageService.GetAll();
            return result;
        }

        [HttpDelete("Delete/{languageId}")]
        public async Task<IActionResult> Delete(string languageId)
        {
            var result = await _languageService.Delete(languageId);
            return Ok(result);
        }

        [HttpPut("{languageId}")]
        public async Task<IActionResult> Update(string languageId, [FromBody] LanguageViewModel request)
        {
            request.Id = languageId;
            var result = await _languageService.UpdateLanguage(request);
            return Ok(result);
        }

        [HttpGet("language_detail/{languageId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetLanguageById(string languageId)
        {
            var congty = await _languageService.GetLanguagetById(languageId);
            return Ok(congty);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LanguageCreate request)
        {
            var result = await _languageService.CreateLanguage(request);
            return Ok(result);
        }
    }
}