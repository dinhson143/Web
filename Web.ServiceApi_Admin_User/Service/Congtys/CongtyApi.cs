using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Web.ViewModels.Catalog.Common;
using Web.ViewModels.Catalog.Congtys;

namespace Web.ServiceApi_Admin_User.Service.Congtys
{
    public class CongtyApi : ICongtyApi
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public CongtyApi(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<ResultApi<string>> CreateCongty(CongtyCreate request, string BearerToken)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", BearerToken);

            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/api/Congtys", httpContent);
            var result = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                return new ResultErrorApi<string>(result);

            return new ResultSuccessApi<string>(result);
        }

        public async Task<bool> Delete(int congtyId, string BearerToken)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", BearerToken);
            var response = await client.DeleteAsync($"/api/Congtys/Delete/{congtyId}");
            return response.IsSuccessStatusCode;
        }

        public async Task<ResultApi<List<CongtyViewModel>>> GetAll()
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var response = await client.GetAsync("/api/Congtys/danh-sach");
            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<List<CongtyViewModel>>(body);
                return new ResultSuccessApi<List<CongtyViewModel>>(list);
            }
            return new ResultErrorApi<List<CongtyViewModel>>("Không thể lấy danh sách Contacts");
        }

        public async Task<CongtyViewModel> GetCongtyById(int id)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var response = await client.GetAsync($"/api/Congtys/congty_detail/{id}");
            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<CongtyViewModel>(body);
                return list;
            }
            return null;
        }

        public async Task<bool> Update(CongtyViewModel request, string BearerToken)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", BearerToken);

            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PutAsync("/api/Congtys/" + request.Id, httpContent);
            return response.IsSuccessStatusCode;
        }
    }
}