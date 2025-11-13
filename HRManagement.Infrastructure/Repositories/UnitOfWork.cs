using HRManagement.Domain.Interfaces;
using HRManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IUserRepository Users { get; }
        public IEmployeeRepository Employees { get; }
        public IPositionRepository Positions { get; }

        public UnitOfWork(
            ApplicationDbContext context,
            IUserRepository users,
            IEmployeeRepository employees,
            IPositionRepository positions)
        {
            _context = context;
            Users = users;
            Employees = employees;
            Positions = positions;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> operation)
        {
            var strategy = _context.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                _context.Database.SetCommandTimeout(30);

                await using var transaction = await _context.Database.BeginTransactionAsync();

                try
                {
                    var result = await operation();
                    await transaction.CommitAsync();
                    return result;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            });
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}