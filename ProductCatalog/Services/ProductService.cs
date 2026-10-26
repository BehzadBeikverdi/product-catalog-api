
using ProductCatalog.Models;
using ProductCatalog.Models.Requests;
using ProductCatalog.Models.Responses;
using ProductCatalog.Repositories;
using ProductCatalog.UnitOfWork;

namespace ProductCatalog.Services;

public class ProductService(
    IProductRepository repository,
    IUnitOfWork unitOfWork) : IProductService
{
    public async Task<BaseResponse<List<ProductResponseModel>>> GetAllAsync()
    {
        var products = await repository.GetAllAsync();

        var productResponses = products.Select(product => new ProductResponseModel
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Stock = product.StockQuantity,
            CreatedAtUtc = product.CreatedAtUtc,
            UpdatedAtUtc = product.UpdatedAtUtc
        }).ToList();

        return BaseResponse<List<ProductResponseModel>>.SuccessResponse(
            "Products retrieved successfully",
            productResponses);
    }

    public async Task<BaseResponse<ProductResponseModel>> GetByIdAsync(Guid id)
    {
        var product = await repository.GetByIdAsync(id);

        if (product is null)
        {
            return BaseResponse<ProductResponseModel>.ErrorResponse(
                "Product not found",
                new object[] { $"Product with ID '{id}' was not found." });
        }
        
        var response = new ProductResponseModel
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Stock = product.StockQuantity,
            CreatedAtUtc = product.CreatedAtUtc,
            UpdatedAtUtc = product.UpdatedAtUtc
        };

        return BaseResponse<ProductResponseModel>.SuccessResponse(
            "Product retrieved successfully",
            response);
    }

    public async Task<BaseResponse<ProductResponseModel>> CreateAsync(
        ProductRequestModel request)
    {
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            StockQuantity = request.Stock,
            CreatedAtUtc = DateTime.UtcNow
        };

        await repository.AddAsync(product);
        await unitOfWork.SaveChangesAsync();

        var response = new ProductResponseModel
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Stock = product.StockQuantity,
            CreatedAtUtc = product.CreatedAtUtc,
            UpdatedAtUtc = product.UpdatedAtUtc
        };

        return BaseResponse<ProductResponseModel>.SuccessResponse(
            "Product created successfully",
            response);
    }

    public async Task<BaseResponse<object>> UpdateAsync(
        Guid id,
        Product product)
    {
        var existing = await repository.GetByIdAsync(id);

        if (existing is null)
        {
            return BaseResponse<object>.ErrorResponse(
                "Product not found",
                new object[] { $"Product with ID '{id}' was not found." });
        }

        existing.Name = product.Name;
        existing.Description = product.Description;
        existing.Price = product.Price;
        existing.StockQuantity = product.StockQuantity;

        repository.Update(existing);
        await unitOfWork.SaveChangesAsync();

        return BaseResponse<object>.SuccessResponse(
            "Product updated successfully",
            null);
    }

    public async Task<BaseResponse<object>> DeleteAsync(Guid id)
    {
        var product = await repository.GetByIdAsync(id);

        if (product is null)
        {
            return BaseResponse<object>.ErrorResponse(
                "Product not found",
                new object[] { $"Product with ID '{id}' was not found." });
        }

        repository.Delete(product);
        await unitOfWork.SaveChangesAsync();

        return BaseResponse<object>.SuccessResponse(
            "Product deleted successfully",
            null);
    }
}
