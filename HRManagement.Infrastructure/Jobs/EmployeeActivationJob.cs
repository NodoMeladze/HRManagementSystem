using HRManagement.Application.Interfaces;
using Microsoft.Extensions.Logging;
using Quartz;

namespace HRManagement.Infrastructure.Jobs
{
    [DisallowConcurrentExecution]
    public class EmployeeActivationJob(
        IEmployeeService employeeService,
        ILogger<EmployeeActivationJob> logger) : IJob
    {
        private readonly IEmployeeService _employeeService = employeeService;
        private readonly ILogger<EmployeeActivationJob> _logger = logger;

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("Employee activation job started at {Time}", DateTime.UtcNow);

            try
            {
                var result = await _employeeService.ActivateEmployeesAsync();

                if (result.Success)
                {
                    _logger.LogInformation("Employee activation job completed successfully: {Message}", result.Message);
                }
                else
                {
                    _logger.LogWarning("Employee activation job completed with warnings: {Message}", result.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing employee activation job");
                throw;
            }
        }
    }
}
