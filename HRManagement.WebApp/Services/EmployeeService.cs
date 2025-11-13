using HRManagement.WebApp.Models.ApiModels;
using HRManagement.WebApp.Models.ApiModels.Employee;
using HRManagement.WebApp.Services.Interfaces;

namespace HRManagement.WebApp.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IApiService _apiService;
        private readonly ILogger<EmployeeService> _logger;

        public EmployeeService(
            IApiService apiService,
            ILogger<EmployeeService> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        public async Task<ApiResponse<IEnumerable<EmployeeDto>>?> GetAllAsync(string? searchTerm = null)
        {
            try
            {
                var endpoint = string.IsNullOrWhiteSpace(searchTerm)
                    ? "employee"
                    : $"employee?searchTerm={Uri.EscapeDataString(searchTerm)}";

                _logger.LogInformation("Fetching employees with search term: {SearchTerm}", searchTerm);
                return await _apiService.GetAsync<ApiResponse<IEnumerable<EmployeeDto>>>(endpoint);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching employees");
                return new ApiResponse<IEnumerable<EmployeeDto>>
                {
                    Success = false,
                    Message = "თანამშრომლების ჩატვირთვისას მოხდა შეცდომა"
                };
            }
        }

        public async Task<ApiResponse<EmployeeDto>?> GetByIdAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("Fetching employee: {EmployeeId}", id);
                return await _apiService.GetAsync<ApiResponse<EmployeeDto>>($"employee/{id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching employee: {EmployeeId}", id);
                return new ApiResponse<EmployeeDto>
                {
                    Success = false,
                    Message = "თანამშრომლის ჩატვირთვისას მოხდა შეცდომა"
                };
            }
        }

        public async Task<ApiResponse<EmployeeDto>?> CreateAsync(CreateEmployeeRequest request)
        {
            try
            {
                _logger.LogInformation("Creating employee: {FirstName} {LastName}",
                    request.FirstName, request.LastName);

                return await _apiService.PostAsync<CreateEmployeeRequest, ApiResponse<EmployeeDto>>(
                    "employee",
                    request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating employee");
                return new ApiResponse<EmployeeDto>
                {
                    Success = false,
                    Message = "თანამშრომლის დამატებისას მოხდა შეცდომა"
                };
            }
        }

        public async Task<ApiResponse<EmployeeDto>?> UpdateAsync(UpdateEmployeeRequest request)
        {
            try
            {
                _logger.LogInformation("Updating employee: {EmployeeId}", request.Id);

                return await _apiService.PutAsync<UpdateEmployeeRequest, ApiResponse<EmployeeDto>>(
                    $"employee/{request.Id}",
                    request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating employee: {EmployeeId}", request.Id);
                return new ApiResponse<EmployeeDto>
                {
                    Success = false,
                    Message = "თანამშრომლის განახლებისას მოხდა შეცდომა"
                };
            }
        }

        public async Task<ApiResponse<bool>?> DeleteAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("Deleting employee: {EmployeeId}", id);
                return await _apiService.DeleteAsync<ApiResponse<bool>>($"employee/{id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting employee: {EmployeeId}", id);
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "თანამშრომლის წაშლისას მოხდა შეცდომა"
                };
            }
        }
    }
}