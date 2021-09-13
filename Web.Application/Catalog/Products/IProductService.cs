using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.ViewModels.Catalog.Common;
using Web.ViewModels.Catalog.Products;

namespace Web.Application.Catalog.Products
{
    public interface IProductService
    {
        public Task<int> CreateProduct(ProductCreate request);

        public Task UpdateProduct();

        public Task DeleteProduct();

        public Task GetProductById();

        public Task UpdatePrice();

        public Task UpdateStock();

        public Task AddViewCount();

        public Task AddImage();

        public Task AddSize_Color(int productId, Size_Color request);

        public Task RemoveImage();

        public Task UpdateImage();

        public Task GetImageId();

        public Task GetListImage();

        public Task<PageResult<ProductViewModel>> GetAll();

        public Task<PageResult<ProductViewModel>> GetAllPaging(GetManageProductPagingRequest request);

        public Task<List<Size_Color>> GetSize_Color(int productId);

        Task<PageResult<ProductViewModel>> GetAllByCategoryId(string languageId, GetPublicProductPagingRequest request);
    }
}