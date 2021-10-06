using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.ViewModels.Catalog.Common;
using Web.ViewModels.Catalog.Languages;

namespace Web.Application.Catalog.Languages
{
    public interface ILanguageService
    {
        public Task<List<LanguageViewModel>> GetAll();

        public Task<ResultApi<string>> CreateLanguage(LanguageCreate request);

        public Task<int> UpdateLanguage(LanguageViewModel request);

        public Task<int> Delete(string id);

        public Task<LanguageViewModel> GetLanguagetById(string id);
    }
}