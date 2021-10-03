using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.ViewModels.Catalog.Categories;
using Web.ViewModels.Catalog.Common;
using Web.ViewModels.Catalog.Products;
using Web.ViewModels.Catalog.Sizes;

namespace Web.Application.Catalog.Products
{
    public interface IProductService
    {
        public Task<ResultApi<string>> CreateProduct(ProductCreate request);

        public Task<int> DeleteProduct(int productId);

        public Task<ResultApi<ProductViewModel>> GetProductById(int productId, string languageId);

        public Task UpdatePrice();

        public Task UpdateStock();

        public Task AddViewCount();

        public Task AddImage();

        public Task AddSize_Color(int productId, Size_Color request);

        public Task RemoveImage();

        public Task UpdateImage();

        public Task<int> Update(ProductUpdateRequest request);

        public Task GetImageId();

        public Task<ResultApi<List<ProductImagesModel>>> GetListImage(int productId);

        public Task<ResultApi<PageResult<ProductViewModel>>> GetAll(GetManageProductPagingRequest request);

        public Task<PageResult<ProductViewModel>> GetAllPaging(GetManageProductPagingRequest request);

        public Task<List<Size_Color>> GetSize_Color(int productId);

        Task<PageResult<ProductViewModel>> GetAllByCategoryId(GetPublicProductPagingRequest request);

        public Task<ResultApi<bool>> AssignCategory(int productId, CategoryAssignRequest request);

        public Task<ResultApi<bool>> AssignSize(int productId, SizeAssignRequest request);

        public Task<List<ProductViewModel>> GetFeaturedProducts(string languageId, int soluong);

        public Task<List<ProductViewModel>> GetLatestProducts(string languageId, int soluong);
    }
}