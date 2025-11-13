using HRManagement.Domain.Entities;
using HRManagement.Domain.Interfaces;
using HRManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.Infrastructure.Repositories
{
    public class PositionRepository(ApplicationDbContext context) : Repository<Position>(context), IPositionRepository
    {
        public async Task<IEnumerable<Position>> GetAllWithEmployeeCountAsync()
        {
            return await _dbSet
                .Include(p => p.Employees.Where(e => e.DeletedAt == null))
                .Where(p => p.DeletedAt == null)
                .ToListAsync();
        }
        public async Task<bool> HasChildrenAsync(Guid id)
        {
            return await _dbSet
                .AnyAsync(p => p.ParentId == id);
        }

        public async Task<bool> HasEmployeesAsync(Guid id)
        {
            return await _context.Employees
                .AnyAsync(e => e.PositionId == id);
        }
    }
}
