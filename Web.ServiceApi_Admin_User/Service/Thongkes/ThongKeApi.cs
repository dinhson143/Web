using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Web.ViewModels.Catalog.Common;
using Web.ViewModels.Catalog.Orders;
using Web.ViewModels.Catalog.Products;

namespace Web.ServiceApi_Admin_User.Service.Thongkes
{
    public class ThongKeApi : IThongKeApi
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public ThongKeApi(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<ResultApi<List<ProductViewModel>>> ProductLovest(string languageId, string BearerToken)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", BearerToken);

            var response = await client.GetAsync($"/api/ThongKes/danh-sach-yeu-thich/{languageId}");
            var body = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<ResultApi<List<ProductViewModel>>>(body);
            return data;
        }

        public async Task<ResultApi<List<ProductViewModel>>> ProductSavest(string from, string to, string languageId, string BearerToken)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", BearerToken);

            var response = await client.GetAsync($"/api/ThongKes/danh-sach-ban-chay/{languageId}/{from}/{to}");
            var body = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<ResultApi<List<ProductViewModel>>>(body);
            return data;
        }

        public async Task<ResultApi<List<OrderViewModel>>> Doanhthu(string from, string to, string languageId, string BearerToken)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", BearerToken);

            var response = await client.GetAsync($"/api/ThongKes/doanh-thu/{languageId}/{from}/{to}");
            var body = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<ResultApi<List<OrderViewModel>>>(body);
            return data;
        }
    }
}