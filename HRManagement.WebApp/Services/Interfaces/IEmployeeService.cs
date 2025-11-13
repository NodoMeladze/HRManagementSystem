using HRManagement.WebApp.Models.ApiModels;
using HRManagement.WebApp.Models.ApiModels.Employee;

namespace HRManagement.WebApp.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<ApiResponse<IEnumerable<EmployeeDto>>?> GetAllAsync(string? searchTerm = null);
        Task<ApiResponse<EmployeeDto>?> GetByIdAsync(Guid id);
        Task<ApiResponse<EmployeeDto>?> CreateAsync(CreateEmployeeRequest request);
        Task<ApiResponse<EmployeeDto>?> UpdateAsync(UpdateEmployeeRequest request);
        Task<ApiResponse<bool>?> DeleteAsync(Guid id);
    }
}