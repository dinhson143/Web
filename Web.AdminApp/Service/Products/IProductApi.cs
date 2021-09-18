using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.ViewModels.Catalog.Categories;
using Web.ViewModels.Catalog.Common;
using Web.ViewModels.Catalog.Products;

namespace Web.AdminApp.Service.Products
{
    public interface IProductApi
    {
        public Task<PageResult<ProductViewModel>> GetAll(GetManageProductPagingRequest request);

        public Task<ResultApi<string>> CreateProduct(ProductCreate request);

        public Task<ResultApi<string>> AssignCategory(int productId, CategoryAssignRequest request);

        public Task<ResultApi<ProductViewModel>> GetProductById(int productId, string BearerToken, string languageId);
    }
}