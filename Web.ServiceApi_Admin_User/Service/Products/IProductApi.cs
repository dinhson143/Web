using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.ViewModels.Catalog.Categories;
using Web.ViewModels.Catalog.Common;
using Web.ViewModels.Catalog.PhieuNhaps;
using Web.ViewModels.Catalog.Products;
using Web.ViewModels.Catalog.Sizes;

namespace Web.ServiceApi_Admin_User.Service.Products
{
    public interface IProductApi
    {
        public Task<PageResult<ProductViewModel>> GetAll(GetManageProductPagingRequest request);

        public Task<ResultApi<string>> CreateProduct(ProductCreate request);

        public Task<ResultApi<string>> AssignCategory(int productId, CategoryAssignRequest request);

        public Task<ResultApi<string>> AssignSize(int productId, SizeAssignRequest request);

        public Task<ResultApi<ProductViewModel>> GetProductById(int productId, string BearerToken, string languageId);

        public Task<List<ProductViewModel>> GetFeaturedProducts(string languageId, int soluong);

        public Task<List<ProductViewModel>> GetLatestProducts(string languageId, int soluong);

        public Task<ResultApi<List<ProductSizeViewModel>>> GetProductSize(int productId, string BearerToken);

        public Task<PageResult<ProductViewModel>> GetAllByCategoryId(GetPublicProductPagingRequest request);

        public Task<ResultApi<List<ProductImagesModel>>> GetListImage(int productId);

        public Task<bool> Update(ProductUpdateRequest request);

        public Task<bool> DeleteProduct(int productId, string BearerToken);

        public Task<bool> UpdatePrice(UpdatePriceRequest request, string BearerToken);
    }
}