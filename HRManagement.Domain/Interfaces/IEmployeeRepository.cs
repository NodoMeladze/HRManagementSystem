using HRManagement.Domain.Entities;

namespace HRManagement.Domain.Interfaces
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        Task<IEnumerable<Employee>> GetAllWithPositionAsync();
        Task<Employee?> GetByIdWithPositionAsync(Guid id);
        Task<IEnumerable<Employee>> SearchWithPositionAsync(string searchTerm);
        Task<IEnumerable<Employee>> GetEmployeesScheduledForActivationAsync(DateTime currentTime);
    }
}
