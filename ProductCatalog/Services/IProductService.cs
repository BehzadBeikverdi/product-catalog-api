using ProductCatalog.Models;
using ProductCatalog.Models.Requests;
using ProductCatalog.Models.Responses;

namespace ProductCatalog.Services;

public interface IProductService
{
    Task<BaseResponse<List<ProductResponseModel>>> GetAllAsync();

    Task<BaseResponse<Product>> GetByIdAsync(Guid id);

    Task<BaseResponse<ProductResponseModel>> CreateAsync(
        ProductRequestModel request);

    Task<BaseResponse<object>> UpdateAsync(Guid id, Product product);

    Task<BaseResponse<object>> DeleteAsync(Guid id);
}