using Microsoft.EntityFrameworkCore;
using ProductCatalog.Data;
using ProductCatalog.Models;

namespace ProductCatalog.Repositories;

public class ProductRepository(AppDbContext db)
    : IProductRepository
{
    public Task<Product?> GetByIdAsync(Guid id) =>
        db.Products.FirstOrDefaultAsync(p => p.Id == id);

    public Task<List<Product>> GetAllAsync() =>
        db.Products.AsNoTracking().ToListAsync();

    public async Task AddAsync(Product product) =>
        await db.Products.AddAsync(product);

    public void Update(Product product) =>
        db.Products.Update(product);

    public void Delete(Product product) =>
        db.Products.Remove(product);
}