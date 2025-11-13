using HRManagement.WebApp.Models.ApiModels.Employee;

namespace HRManagement.WebApp.Models.ViewModels.Employee
{
    public class EmployeeListViewModel
    {
        public List<EmployeeDto> Employees { get; set; } = new();
        public string? SearchTerm { get; set; }
        public int TotalCount => Employees.Count;
    }
}