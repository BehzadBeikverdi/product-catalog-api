using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Models;
using ProductCatalog.Models.Requests;
using ProductCatalog.Services;

namespace ProductCatalog.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IProductService productService)
    : ControllerBase
{
    // GET: api/products
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await productService.GetAllAsync();
        return Ok(products);
    }

    // GET: api/products/{id}
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var product = await productService.GetByIdAsync(id);
        return Ok(product);
    }

    // POST: api/products
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] ProductRequestModel product)
    {
        var createdProduct = await productService.CreateAsync(product);

        return CreatedAtAction(
            nameof(GetById),
            new { id = createdProduct.Payload.Id },
            createdProduct);
    }

    // PUT: api/products/{id}
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] Product product)
    {
        await productService.UpdateAsync(id, product);
        return NoContent();
    }

    // DELETE: api/products/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await productService.DeleteAsync(id);
        return NoContent();
    }
}