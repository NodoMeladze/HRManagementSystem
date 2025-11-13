using AutoMapper;
using HRManagement.Application.DTOs.Common;
using HRManagement.Application.DTOs.Employee;
using HRManagement.Application.Exceptions;
using HRManagement.Application.Interfaces;
using HRManagement.Domain.Entities;
using HRManagement.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace HRManagement.Application.Services
{
    public class EmployeeService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<EmployeeService> logger) : IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<EmployeeService> _logger = logger;

        public async Task<ResultDto<IEnumerable<EmployeeDto>>> GetAllAsync(string? searchTerm = null)
        {
            try
            {
                IEnumerable<Employee> employees = string.IsNullOrWhiteSpace(searchTerm)
                    ? await _unitOfWork.Employees.GetAllWithPositionAsync()
                    : await _unitOfWork.Employees.SearchWithPositionAsync(searchTerm);

                var employeeDtos = _mapper.Map<IEnumerable<EmployeeDto>>(employees);

                return ResultDto<IEnumerable<EmployeeDto>>.SuccessResult(employeeDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting employees");
                return ResultDto<IEnumerable<EmployeeDto>>.FailureResult(
                    "თანამშრომლების ჩატვირთვისას მოხდა შეცდომა",
                    [ex.Message]);
            }
        }

        public async Task<ResultDto<EmployeeDto>> GetByIdAsync(Guid id)
        {
            try
            {
                var employee = await _unitOfWork.Employees.GetByIdWithPositionAsync(id);

                if (employee == null)
                {
                    return ResultDto<EmployeeDto>.FailureResult("თანამშრომელი ვერ მოიძებნა");
                }

                var employeeDto = _mapper.Map<EmployeeDto>(employee);
                return ResultDto<EmployeeDto>.SuccessResult(employeeDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting employee by id: {Id}", id);
                return ResultDto<EmployeeDto>.FailureResult(
                    "თანამშრომლის ჩატვირთვისას მოხდა შეცდომა",
                    [ex.Message]);
            }
        }

        public async Task<ResultDto<EmployeeDto>> CreateAsync(CreateEmployeeDto createDto)
        {
            try
            {
                var employeeDto = await _unitOfWork.ExecuteInTransactionAsync(async () =>
                {
                    var existingByPersonalNumber = await _unitOfWork.Employees
                        .FirstOrDefaultAsync(e => e.PersonalNumber == createDto.PersonalNumber);

                    if (existingByPersonalNumber != null)
                    {
                        throw new BusinessException(
                            "ამ პირადი ნომრით თანამშრომელი უკვე არსებობს");
                    }

                    if (!string.IsNullOrEmpty(createDto.Email))
                    {
                        var existingByEmail = await _unitOfWork.Employees
                            .FirstOrDefaultAsync(e => e.Email == createDto.Email);

                        if (existingByEmail != null)
                        {
                            throw new BusinessException(
                                "ამ ელ.ფოსტით თანამშრომელი უკვე არსებობს");
                        }
                    }

                    var position = await _unitOfWork.Positions.GetByIdAsync(createDto.PositionId)
                        ?? throw new BusinessException("მითითებული თანამდებობა ვერ მოიძებნა");

                    var employee = _mapper.Map<Employee>(createDto);
                    employee.IsActive = false;
                    employee.ActivationScheduledAt = DateTime.UtcNow.AddHours(1);

                    await _unitOfWork.Employees.AddAsync(employee);
                    await _unitOfWork.SaveChangesAsync();

                    _logger.LogInformation(
                        "Employee created: {Id}, scheduled for activation at {Time}",
                        employee.Id,
                        employee.ActivationScheduledAt);

                    var createdEmployee = await _unitOfWork.Employees
                        .GetByIdWithPositionAsync(employee.Id);

                    return _mapper.Map<EmployeeDto>(createdEmployee);
                });

                return ResultDto<EmployeeDto>.SuccessResult(
                    employeeDto,
                    "თანამშრომელი წარმატებით დაემატა");
            }
            catch (BusinessException ex)
            {
                return ResultDto<EmployeeDto>.FailureResult(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating employee");
                return ResultDto<EmployeeDto>.FailureResult(
                    "თანამშრომლის დამატებისას მოხდა შეცდომა",
                    [ex.Message]);
            }
        }

        public async Task<ResultDto<EmployeeDto>> UpdateAsync(UpdateEmployeeDto updateDto)
        {
            try
            {
                var employeeDto = await _unitOfWork.ExecuteInTransactionAsync(async () =>
                {
                    var employee = await _unitOfWork.Employees
                        .GetByIdWithPositionAsync(updateDto.Id)
                        ?? throw new BusinessException("თანამშრომელი ვერ მოიძებნა");

                    var existingByPersonalNumber = await _unitOfWork.Employees
                        .FirstOrDefaultAsync(e => e.PersonalNumber == updateDto.PersonalNumber &&
                                                 e.Id != updateDto.Id);

                    if (existingByPersonalNumber != null)
                    {
                        throw new BusinessException(
                            "ამ პირადი ნომრით თანამშრომელი უკვე არსებობს");
                    }

                    if (!string.IsNullOrEmpty(updateDto.Email))
                    {
                        var existingByEmail = await _unitOfWork.Employees
                            .FirstOrDefaultAsync(e => e.Email == updateDto.Email &&
                                                     e.Id != updateDto.Id);

                        if (existingByEmail != null)
                        {
                            throw new BusinessException(
                                "ამ ელ.ფოსტით თანამშრომელი უკვე არსებობს");
                        }
                    }

                    var position = await _unitOfWork.Positions
                        .GetByIdAsync(updateDto.PositionId)
                        ?? throw new BusinessException("მითითებული თანამდებობა ვერ მოიძებნა");

                    _mapper.Map(updateDto, employee);
                    employee.UpdatedAt = DateTime.UtcNow;

                    _unitOfWork.Employees.Update(employee);
                    await _unitOfWork.SaveChangesAsync();

                    _logger.LogInformation("Employee updated: {Id}", employee.Id);

                    var updatedEmployee = await _unitOfWork.Employees
                        .GetByIdWithPositionAsync(updateDto.Id);

                    return _mapper.Map<EmployeeDto>(updatedEmployee);
                });

                return ResultDto<EmployeeDto>.SuccessResult(
                    employeeDto,
                    "თანამშრომელი წარმატებით განახლდა");
            }
            catch (BusinessException ex)
            {
                return ResultDto<EmployeeDto>.FailureResult(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating employee: {Id}", updateDto.Id);
                return ResultDto<EmployeeDto>.FailureResult(
                    "თანამშრომლის განახლებისას მოხდა შეცდომა",
                    [ex.Message]);
            }
        }

        public async Task<ResultDto<bool>> DeleteAsync(Guid id)
        {
            try
            {
                await _unitOfWork.ExecuteInTransactionAsync(async () =>
                {
                    var employee = await _unitOfWork.Employees
                        .GetByIdAsync(id)
                        ?? throw new BusinessException("თანამშრომელი ვერ მოიძებნა");

                    employee.DeletedAt = DateTime.UtcNow;
                    _unitOfWork.Employees.Update(employee);
                    await _unitOfWork.SaveChangesAsync();

                    _logger.LogInformation("Employee deleted (soft): {Id}", id);

                    return true;
                });

                return ResultDto<bool>.SuccessResult(true, "თანამშრომელი წარმატებით წაიშალა");
            }
            catch (BusinessException ex)
            {
                return ResultDto<bool>.FailureResult(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting employee: {Id}", id);
                return ResultDto<bool>.FailureResult(
                    "თანამშრომლის წაშლისას მოხდა შეცდომა",
                    [ex.Message]);
            }
        }

        public async Task<ResultDto<bool>> ActivateEmployeesAsync()
        {
            try
            {
                var count = await _unitOfWork.ExecuteInTransactionAsync(async () =>
                {
                    var now = DateTime.UtcNow;
                    var employeesToActivate = await _unitOfWork.Employees
                        .GetEmployeesScheduledForActivationAsync(now);

                    var activatedCount = 0;
                    foreach (var employee in employeesToActivate)
                    {
                        employee.IsActive = true;
                        _unitOfWork.Employees.Update(employee);
                        activatedCount++;
                    }

                    if (activatedCount > 0)
                    {
                        await _unitOfWork.SaveChangesAsync();
                        _logger.LogInformation("Activated {Count} employees", activatedCount);
                    }

                    return activatedCount;
                });

                return ResultDto<bool>.SuccessResult(
                    true,
                    $"{count} თანამშრომელი გააქტიურდა");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error activating employees");
                return ResultDto<bool>.FailureResult(
                    "თანამშრომლების გააქტიურებისას მოხდა შეცდომა",
                    [ex.Message]);
            }
        }
    }
}