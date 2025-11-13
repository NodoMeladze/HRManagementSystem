using HRManagement.Domain.Enums;

namespace HRManagement.Application.DTOs.Employee
{
    public class CreateEmployeeDto
    {
        public string PersonalNumber { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public Gender Gender { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string? Email { get; set; }
        public Guid PositionId { get; set; }
        public EmployeeStatus Status { get; set; }
        public DateOnly? ReleaseDate { get; set; }
    }
}
