namespace MillionRealEstatecompany.API.Interfaces;

/// <summary>
/// Interface for Unit of Work pattern implementation
/// </summary>
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