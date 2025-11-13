namespace HRManagement.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IEmployeeRepository Employees { get; }
        IPositionRepository Positions { get; }

        Task<int> SaveChangesAsync();
        //Task BeginTransactionAsync();
        //Task CommitTransactionAsync();
        //Task RollbackTransactionAsync();
        Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> operation);
    }
}
