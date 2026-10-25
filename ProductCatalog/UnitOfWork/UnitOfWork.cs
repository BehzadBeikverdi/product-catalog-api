namespace ProductCatalog.UnitOfWork;

using ProductCatalog.Data;

public class UnitOfWork(AppDbContext db) : IUnitOfWork
{
    public Task<int> SaveChangesAsync() =>
        db.SaveChangesAsync();
}