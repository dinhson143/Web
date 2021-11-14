using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.ViewModels.Catalog.Categories;
using Web.ViewModels.Catalog.Common;
using Web.ViewModels.Catalog.PhieuNhaps;
using Web.ViewModels.Catalog.Products;
using Web.ViewModels.Catalog.Sizes;

namespace Web.Application.Catalog.Products
{
    public interface IProductService
    {
        public Task<ResultApi<string>> CreateProduct(ProductCreate request);

        public Task<ResultApi<string>> CreateProductFavorite(ProductFVCreate request);

        public Task<int> DeleteProduct(int productId);

        public Task<int> DeleteProductFV(Guid userId, int productId);

        public Task<ResultApi<ProductViewModel>> GetProductById(int productId, string languageId);
        public Task<string> GetProductByName(string productName, string languageId);

        public Task<ResultApi<List<ProductViewModel>>> GetProductLQ(int productId, string languageId);

        public Task<ResultApi<string>> UpdatePrice(UpdatePriceRequest request);

        public Task UpdateStock();

        public Task<int> AddViewCount(int productId);

        public Task<ResultApi<int>> AddImage(ProductUpdateRequest request);

        public Task AddSize_Color(int productId, ProductSizeViewModel request);

        public Task<int> RemoveImage(int id);

        public Task UpdateImage();

        public Task<int> Update(ProductUpdateRequest request);

        public Task GetImageId();

        public Task<ResultApi<List<ProductImagesModel>>> GetListImage(int productId);

        public Task<ResultApi<PageResult<ProductViewModel>>> GetAll(GetManageProductPagingRequest request);

        public Task<PageResult<ProductViewModel>> GetAllPaging(GetManageProductPagingRequest request);

        public List<ProductSizeViewModel> GetProductSize(int ProductId);

        public Task<List<ProductFavoriteViewModel>> GetProductFavorite(ProductFVrequest request);

        Task<PageResult<ProductViewModel>> GetAllByCategoryId(GetPublicProductPagingRequest request);

        public Task<ResultApi<bool>> AssignCategory(int productId, CategoryAssignRequest request);

        public Task<ResultApi<bool>> AssignSize(int productId, SizeAssignRequest request);

        public Task<List<ProductViewModel>> GetFeaturedProducts(string languageId, int soluong);

        public Task<List<ProductViewModel>> GetLatestProducts(string languageId, int soluong);

        public Task<List<ProductViewModel>> GetProductsOrderMax(string languageId, int soluong);
    }
}