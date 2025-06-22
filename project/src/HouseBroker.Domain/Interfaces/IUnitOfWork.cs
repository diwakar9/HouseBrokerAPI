namespace HouseBroker.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IPropertyRepository Properties { get; }
    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}