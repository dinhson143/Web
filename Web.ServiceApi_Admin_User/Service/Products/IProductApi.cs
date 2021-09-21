using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.ViewModels.Catalog.Categories;
using Web.ViewModels.Catalog.Common;
using Web.ViewModels.Catalog.Products;

namespace Web.ServiceApi_Admin_User.Service.Products
{
    public interface IProductApi
    {
        public Task<PageResult<ProductViewModel>> GetAll(GetManageProductPagingRequest request);

        public Task<ResultApi<string>> CreateProduct(ProductCreate request);

        public Task<ResultApi<string>> AssignCategory(int productId, CategoryAssignRequest request);

        public Task<ResultApi<ProductViewModel>> GetProductById(int productId, string BearerToken, string languageId);

        public Task<List<ProductViewModel>> GetFeaturedProducts(string languageId, int soluong);

        public Task<List<ProductViewModel>> GetLatestProducts(string languageId, int soluong);

        public Task<PageResult<ProductViewModel>> GetAllByCategoryId(GetPublicProductPagingRequest request);

        public Task<ResultApi<List<ProductImagesModel>>> GetListImage(int productId);
    }
}