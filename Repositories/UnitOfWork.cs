using Microsoft.EntityFrameworkCore.Storage;
using MillionRealEstatecompany.API.Data;
using MillionRealEstatecompany.API.Interfaces;

namespace MillionRealEstatecompany.API.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IDbContextTransaction? _transaction;
    
    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        Owners = new OwnerRepository(_context);
        Properties = new PropertyRepository(_context);
        PropertyImages = new PropertyImageRepository(_context);
        PropertyTraces = new PropertyTraceRepository(_context);
    }

    public IOwnerRepository Owners { get; private set; }
    public IPropertyRepository Properties { get; private set; }
    public IPropertyImageRepository PropertyImages { get; private set; }
    public IPropertyTraceRepository PropertyTraces { get; private set; }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}