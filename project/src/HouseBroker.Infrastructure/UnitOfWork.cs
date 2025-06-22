using HouseBroker.Domain.Interfaces;
using HouseBroker.Infrastructure.Data;
using HouseBroker.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace HouseBroker.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly HouseBrokerDbContext _context;
    private IDbContextTransaction? _transaction;
    private bool _disposed = false;

    public UnitOfWork(HouseBrokerDbContext context)
    {
        _context = context;
        Properties = new PropertyRepository(_context);
    }

    public IPropertyRepository Properties { get; private set; }

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
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
        _disposed = true;
    }
}