using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Models;
using ProductCatalog.Models.Requests;
using ProductCatalog.Models.Responses;
using ProductCatalog.Services;

namespace ProductCatalog.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IProductService productService)
    : ControllerBase
{
    // GET: api/products
    [HttpGet]
    public async Task<BaseResponse<List<ProductResponseModel>>> GetAll()
    {
        var products = await productService.GetAllAsync();

        return products;
        
        // return Ok(products);
    }

    // GET: api/products/{id}
    [HttpGet("{id:guid}")]
    public async Task<BaseResponse<ProductResponseModel>> GetById(Guid id)
    {
        var product = await productService.GetByIdAsync(id);

        return product;
        
        // return Ok(product);
    }

    // POST: api/products
    [HttpPost]
    public async Task<BaseResponse<ProductResponseModel>> Create(
        [FromBody] ProductRequestModel product)
    {
        var createdProduct = await productService.CreateAsync(product);

        return createdProduct;
        
        // return CreatedAtAction(
        //     nameof(GetById),
        //     new { id = createdProduct.Payload.Id },
        //     createdProduct);
    }

    // PUT: api/products/{id}
    [HttpPut("{id:guid}")]
    public async Task<BaseResponse<Object>> Update(
        Guid id,
        [FromBody] Product product)
    {
        return await productService.UpdateAsync(id, product);
        // return NoContent();
    }

    // DELETE: api/products/{id}
    [HttpDelete("{id:guid}")]
    public async Task<BaseResponse<Object>> Delete(Guid id)
    {
        return await productService.DeleteAsync(id);
        // return NoContent();
    }
}