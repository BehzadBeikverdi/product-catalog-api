namespace ProductCatalog.UnitOfWork;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync();
}