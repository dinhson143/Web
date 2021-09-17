using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Web.ViewModels.Catalog.Categories;
using Web.ViewModels.Catalog.Common;

namespace Web.AdminApp.Service.Categories
{
    public class CategoryApi : ICategoryApi
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public CategoryApi(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<ResultApi<List<CategoryViewModel>>> GetAll(string languageId, string BearerToken)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", BearerToken);

            var response = await client.GetAsync("/api/Categories/categories?languageId=" + languageId);
            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<List<CategoryViewModel>>(body);
                return new ResultSuccessApi<List<CategoryViewModel>>(list);
            }
            return new ResultErrorApi<List<CategoryViewModel>>("Không thể lấy danh sách Categories");
        }
    }
}