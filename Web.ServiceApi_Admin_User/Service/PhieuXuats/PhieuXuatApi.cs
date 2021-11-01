using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Web.ViewModels.Catalog.Common;
using Web.ViewModels.Catalog.PhieuNhaps;
using Web.ViewModels.Catalog.PhieuXuats;

namespace Web.ServiceApi_Admin_User.Service.PhieuXuats
{
    public class PhieuXuatApi : IPhieuXuatApi
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public PhieuXuatApi(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<bool> CreateCTPhieuXuat(CTPhieuXuatCreate request, string BearerToken)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", BearerToken);

            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/api/PhieuXuats/chi-tiet-phieu-xuat", httpContent);
            var result = await response.Content.ReadAsStringAsync();

            return Convert.ToBoolean(result);
        }

        public async Task<ResultApi<string>> CreatePhieuXuat(PhieuXuatCreate request, string BearerToken)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", BearerToken);

            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/api/PhieuXuats", httpContent);
            var result = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                return new ResultErrorApi<string>(result);

            return new ResultSuccessApi<string>(result);
        }

        public async Task<ResultApi<List<PhieuXuatViewModel>>> GetAll(string BearerToken)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", BearerToken);

            var response = await client.GetAsync("/api/PhieuXuats/danh-sach");
            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<List<PhieuXuatViewModel>>(body);
                return new ResultSuccessApi<List<PhieuXuatViewModel>>(list);
            }
            return new ResultErrorApi<List<PhieuXuatViewModel>>("Không thể lấy danh sách Phiếu xuất");
        }

        public async Task<List<PhieuNXchitietViewModel>> GetPhieuXuatById(int id, string languageId, string BearerToken)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", BearerToken);

            var response = await client.GetAsync($"/api/PhieuXuats/phieunhap_detail/{id}/{languageId}");
            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<List<PhieuNXchitietViewModel>>(body);
                return list;
            }
            return null;
        }
    }
}