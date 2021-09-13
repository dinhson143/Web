﻿using Microsoft.Extensions.Configuration;
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

namespace Web.AdminApp.Service
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

        public Task<bool> Register(RegisterRequest request)
        {
            throw new NotImplementedException();
        }
    }
}