using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Web.ViewModels.Catalog.Comments;
using Web.ViewModels.Catalog.Common;

namespace Web.ServiceApi_Admin_User.Service.Comments
{
    public class CommentApi : ICommentApi
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public CommentApi(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<ResultApi<string>> CreateComment(CommentCreate request, string BearerToken)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", BearerToken);

            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/api/Comments", httpContent);
            var result = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                return new ResultErrorApi<string>(result);

            return new ResultSuccessApi<string>(result);
        }

        public async Task<bool> Delete(int commentId, string BearerToken)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", BearerToken);
            var response = await client.DeleteAsync($"/api/Comments/Delete/{commentId}");
            return response.IsSuccessStatusCode;
        }

        public async Task<ResultApi<List<CommentViewModel>>> GetAllAdmin(string languageId, string BearerToken)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", BearerToken);

            var response = await client.GetAsync("/api/Comments/danh-sach-admin?languageId=" + languageId);
            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<List<CommentViewModel>>(body);
                return new ResultSuccessApi<List<CommentViewModel>>(list);
            }
            return new ResultErrorApi<List<CommentViewModel>>("Không thể lấy danh sách bình luận");
        }

        public async Task<ResultApi<List<CommentViewModel>>> GetAllNow(string languageId, string BearerToken)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", BearerToken);

            var response = await client.GetAsync("/api/Comments/danh-sach-now?languageId=" + languageId);
            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<List<CommentViewModel>>(body);
                return new ResultSuccessApi<List<CommentViewModel>>(list);
            }
            return new ResultErrorApi<List<CommentViewModel>>("Không thể lấy danh sách bình luận");
        }

        public async Task<ResultApi<List<CommentViewModel>>> GetAllWeb(int productID, string languageId)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var response = await client.GetAsync($"/api/Comments/danh-sach-web?languageId=" +
                $"{languageId}" +
                $"&productID={productID}");
            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<List<CommentViewModel>>(body);
                return new ResultSuccessApi<List<CommentViewModel>>(list);
            }
            return new ResultErrorApi<List<CommentViewModel>>("Không thể lấy danh sách bình luận");
        }
    }
}