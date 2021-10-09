using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Web.ViewModels.Catalog.Categories;
using Web.ViewModels.Catalog.Common;
using Web.ViewModels.Catalog.PhieuNhaps;
using Web.ViewModels.Catalog.Products;
using Web.ViewModels.Catalog.Sizes;

namespace Web.ServiceApi_Admin_User.Service.Products
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
                $"{request.pageIndex}&pageSize={request.pageSize}&" +
                $"Keyword={request.Keyword}" +
                $"&LanguageId={request.LanguageId}" +
                $"&categoryId={request.CategoryId}");
            var body = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<PageResult<ProductViewModel>>(body);
            return data;
        }

        public async Task<ResultApi<string>> AssignCategory(int productId, CategoryAssignRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", request.BearerToken);

            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/Products/{productId}/categories", httpContent);

            var result = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                return new ResultErrorApi<string>(result);

            return new ResultSuccessApi<string>("");
        }

        public async Task<ResultApi<ProductViewModel>> GetProductById(int productId, string BearerToken, string languageId)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            //client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", BearerToken);

            var response = await client.GetAsync($"/api/Products/product_detail/{productId}/{languageId}");

            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<ResultApi<ProductViewModel>>(result);
        }

        public async Task<List<ProductViewModel>> GetFeaturedProducts(string languageId, int soluong)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            var response = await client.GetAsync($"/api/Products/featured-product/{languageId}/{soluong}");
            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<ProductViewModel>>(result);
        }

        public async Task<List<ProductViewModel>> GetLatestProducts(string languageId, int soluong)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            var response = await client.GetAsync($"/api/Products/latest-product/{languageId}/{soluong}");
            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<ProductViewModel>>(result);
        }

        public async Task<PageResult<ProductViewModel>> GetAllByCategoryId(GetPublicProductPagingRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var response = await client.GetAsync($"/api/Products/products-category?pageIndex=" +
                $"{request.pageIndex}&pageSize={request.pageSize}&" +
                $"&LanguageId={request.LanguageId}" +
                $"&categoryId={request.CategoryId}");
            var body = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<PageResult<ProductViewModel>>(body);
            return data;
        }

        public async Task<ResultApi<List<ProductImagesModel>>> GetListImage(int productId)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            var response = await client.GetAsync($"/api/Products/product_images/{productId}");

            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<ResultApi<List<ProductImagesModel>>>(result);
        }

        public async Task<bool> Update(ProductUpdateRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", request.BearerToken);

            var requestContent = new MultipartFormDataContent();
            if (request.ImageURL != null && request.ImageURL.Length > 0)
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
            requestContent.Add(new StringContent(request.Name.ToString()), "name");
            requestContent.Add(new StringContent(request.Description.ToString()), "description");

            requestContent.Add(new StringContent(request.Details.ToString()), "details");
            requestContent.Add(new StringContent(request.SeoDescription.ToString()), "seoDescription");
            requestContent.Add(new StringContent(request.SeoTitle.ToString()), "seoTitle");
            requestContent.Add(new StringContent(request.SeoAlias.ToString()), "seoAlias");
            requestContent.Add(new StringContent(request.LanguageId.ToString()), "languageId");
            requestContent.Add(new StringContent(request.IsFeatured.ToString()), "isFeatured");
            requestContent.Add(new StringContent(request.BearerToken.ToString()), "bearerToken");

            var response = await client.PutAsync("/api/Products/" + request.Id, requestContent);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteProduct(int productId, string BearerToken)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", BearerToken);

            var response = await client.DeleteAsync($"/api/Products/Delete/{productId}");

            return response.IsSuccessStatusCode;
        }

        public async Task<ResultApi<string>> AssignSize(int productId, SizeAssignRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", request.BearerToken);

            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/Products/{productId}/sizes", httpContent);

            var result = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                return new ResultErrorApi<string>(result);

            return new ResultSuccessApi<string>("");
        }

        public async Task<ResultApi<List<ProductSizeViewModel>>> GetProductSize(int ProductId, string BearerToken)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", BearerToken);

            var response = await client.GetAsync($"/api/Products/danh-sach-product-size/{ProductId}");
            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<List<ProductSizeViewModel>>(body);
                return new ResultSuccessApi<List<ProductSizeViewModel>>(list);
            }
            return new ResultErrorApi<List<ProductSizeViewModel>>("Không thể lấy danh sách size của sản phẩm");
        }

        public async Task<bool> UpdatePrice(UpdatePriceRequest request, string BearerToken)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", BearerToken);

            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PutAsync("/api/Products/Update-price", httpContent);
            return response.IsSuccessStatusCode;
        }
    }
}