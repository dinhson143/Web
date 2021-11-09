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
using Web.ViewModels.Catalog.Sales;

namespace Web.ServiceApi_Admin_User.Service.Orders
{
    public class OrderApi : IOrderApi
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public OrderApi(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<ResultApi<string>> CreateOrder(CheckoutRequest request, string BearerToken)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", BearerToken);

            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/api/Orders", httpContent);
            var result = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                return new ResultErrorApi<string>(result);

            return new ResultSuccessApi<string>(result);
        }

        public async Task<ResultApi<List<OrderViewModel>>> GetOrderUser(Guid userId, string languageID, string BearerToken)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", BearerToken);

            var response = await client.GetAsync($"/api/Orders/danh-sach-order/{languageID}/{userId}");
            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<List<OrderViewModel>>(body);
                return new ResultSuccessApi<List<OrderViewModel>>(list);
            }
            return new ResultErrorApi<List<OrderViewModel>>("Không thể lấy danh sách Orders");
        }

        public async Task<ResultApi<List<OrderViewModel>>> GetOrderUserHistory(Guid userId, string languageID, string BearerToken)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", BearerToken);

            var response = await client.GetAsync($"/api/Orders/danh-sach-order-history/{languageID}/{userId}");
            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<List<OrderViewModel>>(body);
                return new ResultSuccessApi<List<OrderViewModel>>(list);
            }
            return new ResultErrorApi<List<OrderViewModel>>("Không thể lấy danh sách Orders");
        }

        public async Task<ResultApi<List<OrderViewModel>>> GetAllOrder(string languageID, string BearerToken)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", BearerToken);

            var response = await client.GetAsync($"/api/Orders/get-all-order/{languageID}");
            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<List<OrderViewModel>>(body);
                return new ResultSuccessApi<List<OrderViewModel>>(list);
            }
            return new ResultErrorApi<List<OrderViewModel>>("Không thể lấy danh sách Orders");
        }

        public async Task<ResultApi<List<OrderViewModel>>> GetallOrderSuccess(string languageID, string BearerToken)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", BearerToken);

            var response = await client.GetAsync($"/api/Orders/get-all-order-success/{languageID}");
            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<List<OrderViewModel>>(body);
                return new ResultSuccessApi<List<OrderViewModel>>(list);
            }
            return new ResultErrorApi<List<OrderViewModel>>("Không thể lấy danh sách Orders");
        }

        public async Task<bool> CancelOrder(Guid userId, int orderId, string BearerToken)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", BearerToken);

            var response = await client.GetAsync($"/api/Orders/cancel-order/{userId}/{orderId}");

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ConfirmOrder(int orderId, string BearerToken)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", BearerToken);

            var response = await client.GetAsync($"/api/Orders/confirm-order/{orderId}");

            return response.IsSuccessStatusCode;
        }

        public async Task<ResultApi<OrderViewModel>> GetOrderByID(int orderID, string languageID, string BearerToken)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", BearerToken);

            var response = await client.GetAsync($"/api/Orders/order-id/{orderID}/{languageID}");
            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                var order = JsonConvert.DeserializeObject<OrderViewModel>(body);
                return new ResultSuccessApi<OrderViewModel>(order);
            }
            return new ResultErrorApi<OrderViewModel>("Không thể lấy danh sách Orders");
        }

        public async Task<ResultApi<List<OrderViewModel>>> GetallOrderConfirm(string languageID, string BearerToken)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", BearerToken);

            var response = await client.GetAsync($"/api/Orders/get-all-order-confirm/{languageID}");
            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<List<OrderViewModel>>(body);
                return new ResultSuccessApi<List<OrderViewModel>>(list);
            }
            return new ResultErrorApi<List<OrderViewModel>>("Không thể lấy danh sách Orders");
        }

        public async Task<bool> SuccessOrder(int orderId, string BearerToken)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", BearerToken);

            var response = await client.GetAsync($"/api/Orders/success-order/{orderId}");

            return response.IsSuccessStatusCode;
        }

        public async Task<ResultApi<List<OrderViewModel>>> GetallOrderInProgress(string languageID, string BearerToken)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", BearerToken);

            var response = await client.GetAsync($"/api/Orders/get-all-order-inProgress/{languageID}");
            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<List<OrderViewModel>>(body);
                return new ResultSuccessApi<List<OrderViewModel>>(list);
            }
            return new ResultErrorApi<List<OrderViewModel>>("Không thể lấy danh sách Orders");
        }
    }
}