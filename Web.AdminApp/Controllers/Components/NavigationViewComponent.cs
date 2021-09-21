using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Web.AdminApp.Models;
using Web.ServiceApi_Admin_User.Service.Languages;
using Web.Utilities.Contants;

namespace Web.AdminApp.Controllers.Components
{
    public class NavigationViewComponent : ViewComponent
    {
        private readonly ILanguageApi _languageApi;
        private readonly IConfiguration _config;

        public NavigationViewComponent(ILanguageApi languageApi, IConfiguration config)
        {
            _languageApi = languageApi;
            _config = config;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var laguages = await _languageApi.GetAll(HttpContext.Session.GetString(SystemContants.AppSettings.Token));
            var navigationVM = new NavigationViewModel()
            {
                CurrentLanguageId = HttpContext.Session.GetString(SystemContants.AppSettings.DefaultLanguageId),
                Languages = laguages.ResultObj
            };

            return View("Default", navigationVM);
        }
    }
}