using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Web.ViewModels.Catalog.Common;
using Web.ViewModels.Catalog.Sliders;

namespace Web.ServiceApi_Admin_User.Service.Sliders
{
    public class SliderApi : ISliderApi
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public SliderApi(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<ResultApi<string>> CreateProduct(SliderCreate request, string BearerToken)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", BearerToken);

            var requestContent = new MultipartFormDataContent();

            List<ByteArrayContent> bytes = new List<ByteArrayContent>();
            byte[] data;
            using (var br = new BinaryReader(request.Image.OpenReadStream()))
            {
                data = br.ReadBytes((int)request.Image.OpenReadStream().Length);
            }
            ByteArrayContent bytesData = new ByteArrayContent(data);
            requestContent.Add(bytesData, "image", request.Image.FileName);
            bytes.Add(bytesData);

            requestContent.Add(new StringContent(request.Url.ToString()), "url");
            requestContent.Add(new StringContent(request.Name.ToString()), "name");
            requestContent.Add(new StringContent(request.Description.ToString()), "description");

            var response = await client.PostAsync("/api/Sliders", requestContent);
            var result = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                return new ResultErrorApi<string>(result);

            return new ResultSuccessApi<string>(result);
        }

        public async Task<bool> Delete(int sliderId, string BearerToken)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", BearerToken);
            var response = await client.DeleteAsync($"/api/Sliders/Delete/{sliderId}");
            return response.IsSuccessStatusCode;
        }

        public async Task<ResultApi<List<SliderViewModel>>> GetAll()
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            //client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", BearerToken);

            var response = await client.GetAsync("/api/Sliders/danh-sach");
            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<List<SliderViewModel>>(body);
                return new ResultSuccessApi<List<SliderViewModel>>(list);
            }
            return new ResultErrorApi<List<SliderViewModel>>("Không thể lấy danh sách Sliders");
        }

        public async Task<ResultApi<SliderViewModel>> GetSliderById(int sliderId, string BearerToken)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", BearerToken);

            var response = await client.GetAsync($"/api/Sliders/slider_detail/{sliderId}");
            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<ResultApi<SliderViewModel>>(body);
                return list;
            }
            return null;
        }

        public async Task<bool> UpdateSlider(SliderUpdateRequest request, string BearerToken)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", BearerToken);

            var requestContent = new MultipartFormDataContent();

            if (request.Image != null)
            {
                List<ByteArrayContent> bytes = new List<ByteArrayContent>();
                byte[] data;
                using (var br = new BinaryReader(request.Image.OpenReadStream()))
                {
                    data = br.ReadBytes((int)request.Image.OpenReadStream().Length);
                }
                ByteArrayContent bytesData = new ByteArrayContent(data);
                requestContent.Add(bytesData, "image", request.Image.FileName);
                bytes.Add(bytesData);
            }

            requestContent.Add(new StringContent(request.Url.ToString()), "url");
            requestContent.Add(new StringContent(request.Name.ToString()), "name");
            requestContent.Add(new StringContent(request.Description.ToString()), "description");

            var response = await client.PutAsync("/api/Sliders/" + request.Id, requestContent);
            return response.IsSuccessStatusCode;
        }
    }
}