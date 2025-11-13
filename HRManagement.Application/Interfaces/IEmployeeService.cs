using HRManagement.Application.DTOs.Common;
using HRManagement.Application.DTOs.Employee;

namespace HRManagement.Application.Interfaces
{
    public interface IEmployeeService
    {
        Task<ResultDto<IEnumerable<EmployeeDto>>> GetAllAsync(string? searchTerm = null);
        Task<ResultDto<EmployeeDto>> GetByIdAsync(Guid id);
        Task<ResultDto<EmployeeDto>> CreateAsync(CreateEmployeeDto createDto);
        Task<ResultDto<EmployeeDto>> UpdateAsync(UpdateEmployeeDto updateDto);
        Task<ResultDto<bool>> DeleteAsync(Guid id);
        Task<ResultDto<bool>> ActivateEmployeesAsync();
    }
}
