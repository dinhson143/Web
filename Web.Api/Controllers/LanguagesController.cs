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
    public class LanguagesController : ControllerBase
    {
        private readonly ILanguageService _languageService;

        public LanguagesController(ILanguageService languageService)
        {
            _languageService = languageService;
        }

        [HttpGet("languages")]
        public async Task<List<LanguageViewModel>> GetAll()
        {
            var result = await _languageService.GetAll();
            return result;
        }
    }
}