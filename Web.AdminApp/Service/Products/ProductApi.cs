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
using Web.ViewModels.Catalog.Products;

namespace Web.AdminApp.Service.Products
{
    public class ProductApi : IProductApi
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public ProductApi(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<ResultApi<string>> CreateProduct(ProductCreate request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", request.BearerToken);

            var requestContent = new MultipartFormDataContent();
            if (request.ImageURL.Length > 0)
            {
                List<ByteArrayContent> bytes = new List<ByteArrayContent>();
                for (var i = 0; i < request.ImageURL.Length; i++)
                {
                    byte[] data;
                    using (var br = new BinaryReader(request.ImageURL[i].OpenReadStream()))
                    {
                        data = br.ReadBytes((int)request.ImageURL[i].OpenReadStream().Length);
                    }
                    ByteArrayContent bytesData = new ByteArrayContent(data);
                    requestContent.Add(bytesData, "imageURL", request.ImageURL[i].FileName);
                    bytes.Add(bytesData);
                }
            }
            requestContent.Add(new StringContent(request.Price.ToString()), "price");
            requestContent.Add(new StringContent(request.OriginalPrice.ToString()), "originalPrice");
            requestContent.Add(new StringContent(request.Name.ToString()), "name");
            requestContent.Add(new StringContent(request.Description.ToString()), "description");

            requestContent.Add(new StringContent(request.Details.ToString()), "details");
            requestContent.Add(new StringContent(request.SeoDescription.ToString()), "seoDescription");
            requestContent.Add(new StringContent(request.SeoTitle.ToString()), "seoTitle");
            requestContent.Add(new StringContent(request.SeoAlias.ToString()), "seoAlias");
            requestContent.Add(new StringContent(request.LanguageId.ToString()), "languageId");

            var response = await client.PostAsync("/api/Products", requestContent);
            var result = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                return new ResultErrorApi<string>(result);

            return new ResultSuccessApi<string>(result);
        }

        public async Task<PageResult<ProductViewModel>> GetAll(GetManageProductPagingRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", request.BearerToken);

            var response = await client.GetAsync($"/api/Products?pageIndex=" +
                $"{request.pageIndex}&pageSize={request.pageSize}&Keyword={request.Keyword}&LanguageId={request.LanguageId}");
            var body = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<PageResult<ProductViewModel>>(body);
            return data;
        }
    }
}