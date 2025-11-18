using Dsw2025Tpi.Application.Dtos;

namespace Dsw2025Tpi.Application.Interfaces
{
    public interface IProductsManagementService
    {
        Task<ProductModel.Response?> GetProductById(Guid id);
        Task<ProductModel.ResponsePagination> GetAllProducts(ProductModel.FilterProduct request);
        Task<ProductModel.Response> AddProduct(ProductModel.Request request);
        Task<ProductModel.Response> UpdateProduct(Guid id, ProductModel.Request request);
        Task<ProductModel.Response?> DeactivateProduct(Guid id);
    }
}
