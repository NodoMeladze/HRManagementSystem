using HRManagement.Domain.Entities;

namespace HRManagement.Domain.Interfaces
{
    public interface IPositionRepository : IRepository<Position>
    {
        Task<IEnumerable<Position>> GetAllWithEmployeeCountAsync();
        Task<bool> HasChildrenAsync(Guid id);
        Task<bool> HasEmployeesAsync(Guid id);
    }
}
