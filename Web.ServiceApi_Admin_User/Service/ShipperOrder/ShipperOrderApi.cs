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
using Web.ViewModels.Catalog.ShipperOrders;

namespace Web.ServiceApi_Admin_User.Service.ShipperOrder
{
    public class ShipperOrderApi : IShipperOrderApi
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public ShipperOrderApi(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<ResultApi<string>> CreateShipperOrder(ShipperOrderCreate request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/api/ShipperOrders", httpContent);
            var result = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                return new ResultErrorApi<string>(result);

            return new ResultSuccessApi<string>(result);
        }

        public async Task<List<OrderViewModel>> GetAll(Guid shipperId)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            //client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", BearerToken);

            var response = await client.GetAsync($"/api/ShipperOrders/danh-sach-order-shipper/{shipperId}");
            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<List<OrderViewModel>>(body);
                return new List<OrderViewModel>(list);
            }
            return new List<OrderViewModel>(null);
        }

        public async Task<List<OrderViewModel>> GetAll_HistorySP(Guid shipperId)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            //client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", BearerToken);

            var response = await client.GetAsync($"/api/ShipperOrders/lich-su-order-shipper/{shipperId}");
            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<List<OrderViewModel>>(body);
                return new List<OrderViewModel>(list);
            }
            return new List<OrderViewModel>(null);
        }
    }
}