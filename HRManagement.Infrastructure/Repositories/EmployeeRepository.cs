using HRManagement.Domain.Entities;
using HRManagement.Domain.Interfaces;
using HRManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.Infrastructure.Repositories
{
    public class EmployeeRepository(ApplicationDbContext context) : Repository<Employee>(context), IEmployeeRepository
    {
        public async Task<IEnumerable<Employee>> GetAllWithPositionAsync()
        {
            return await _dbSet
                .Include(e => e.Position)
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .ToListAsync();
        }

        public async Task<Employee?> GetByIdWithPositionAsync(Guid id)
        {
            return await _dbSet
                .Include(e => e.Position)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<Employee>> SearchWithPositionAsync(string searchTerm)
        {
            var cleanSearchTerm = searchTerm.Trim();

            return await _dbSet
                .Include(e => e.Position)
                .Where(e => (e.FirstName.Contains(cleanSearchTerm) ||
                            e.LastName.Contains(cleanSearchTerm) ||
                            (e.FirstName + " " + e.LastName).Contains(cleanSearchTerm)))
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetEmployeesScheduledForActivationAsync(DateTime currentTime)
        {
            return await _dbSet
                .Include(e => e.Position)
                .Where(e => !e.IsActive &&
                           e.ActivationScheduledAt.HasValue &&
                           e.ActivationScheduledAt.Value <= currentTime)
                .ToListAsync();
        }
    }
}
