namespace MillionRealEstatecompany.API.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IOwnerRepository Owners { get; }
    IPropertyRepository Properties { get; }
    IPropertyImageRepository PropertyImages { get; }
    IPropertyTraceRepository PropertyTraces { get; }
    
    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}