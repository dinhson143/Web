using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Web.ViewModels.Catalog.Common;
using Web.ViewModels.Catalog.Users;
using Web.ViewModels.System.User;

namespace Web.ServiceApi_Admin_User.Service.Users
{
    public class UserAPi : IUserApi
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public UserAPi(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<ResultApi<int>> CheckMail(string email)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            var response = await client.GetAsync($"/api/Login/check-mail/{email}");

            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<ResultErrorApi<int>>(result);
        }

        public async Task<ResultApi<string>> DeleteUser(Guid IdUser, string BearerToken)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", BearerToken);

            var response = await client.DeleteAsync($"/api/Users/Delete/{IdUser}");

            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ResultSuccessApi<string>>(result);
            return JsonConvert.DeserializeObject<ResultErrorApi<string>>(result);
        }

        public async Task<ResultApi<string>> ForgetPassword(ForgetPassViewModel request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            var response = await client.PostAsync("/api/Login/Forget-Password", httpContent);

            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ResultSuccessApi<string>>(result);

            return JsonConvert.DeserializeObject<ResultErrorApi<string>>(result);
        }

        public async Task<PageResult<UserViewModel>> GetAllPaging(GetUserPagingRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", request.BearerToken);

            var response = await client.GetAsync($"/api/Users/paging?pageIndex=" +
                $"{request.pageIndex}&pageSize={request.pageSize}&Keyword={request.Keyword}");
            var body = await response.Content.ReadAsStringAsync();
            var list = JsonConvert.DeserializeObject<PageResult<UserViewModel>>(body);
            return list;
        }

        public async Task<UserViewModel> GetUserById(Guid IdUser, string BearerToken)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", BearerToken);

            var response = await client.GetAsync($"/api/Users/getUser/{IdUser}");

            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<UserViewModel>(result);
        }

        public async Task<string> Login(LoginRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            var response = await client.PostAsync("/api/Login/login", httpContent);

            var token = await response.Content.ReadAsStringAsync();
            return token;
        }

        public async Task<ResultApi<string>> Register(RegisterRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            var response = await client.PostAsync("/api/Login/Register", httpContent);

            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ResultSuccessApi<string>>(result);

            return JsonConvert.DeserializeObject<ResultErrorApi<string>>(result);
        }

        public async Task<ResultApi<string>> RoleAssign(Guid IdUser, RoleAssignRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", request.BearerToken);

            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/Users/{IdUser}/roles", httpContent);

            var result = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                return new ResultErrorApi<string>(result);

            return new ResultSuccessApi<string>(result);
        }

        public async Task<ResultApi<string>> Update(Guid IdUser, UpdateUserRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", request.BearerToken);

            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/Users/{IdUser}", httpContent);

            var result = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                return new ResultErrorApi<string>(result);

            return new ResultSuccessApi<string>(result);
        }
    }
}