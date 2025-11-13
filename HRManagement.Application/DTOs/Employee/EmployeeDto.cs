using HRManagement.Domain.Enums;

namespace HRManagement.Application.DTOs.Employee
{
    public class EmployeeDto
    {
        public Guid Id { get; set; }
        public string PersonalNumber { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName => $"{FirstName} {LastName}";
        public Gender Gender { get; set; }
        public string GenderDisplay => Gender switch
        {
            Gender.Male => "მამრობითი",
            Gender.Female => "მდედრობითი",
            _ => "არ არის მითითებული"
        };
        public DateOnly DateOfBirth { get; set; }
        public string? Email { get; set; }
        public Guid PositionId { get; set; }
        public string PositionName { get; set; } = string.Empty;
        public EmployeeStatus Status { get; set; }
        public string StatusDisplay => Status switch
        {
            EmployeeStatus.Inactive => "არააქტიური",
            EmployeeStatus.InStaff => "შტატში",
            EmployeeStatus.OutOfStaff => "შტატგარეშე",
            EmployeeStatus.Released => "გათავისუფლებული",
            _ => "არ არის მითითებული"
        };
        public DateOnly? ReleaseDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
