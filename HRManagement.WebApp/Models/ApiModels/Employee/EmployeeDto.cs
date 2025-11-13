namespace HRManagement.WebApp.Models.ApiModels.Employee
{
    public class EmployeeDto
    {
        public Guid Id { get; set; }
        public string PersonalNumber { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public int Gender { get; set; }
        public string GenderDisplay { get; set; } = string.Empty;
        public string DateOfBirth { get; set; } = string.Empty;
        public string? Email { get; set; }
        public Guid PositionId { get; set; }
        public string PositionName { get; set; } = string.Empty;
        public int Status { get; set; }
        public string StatusDisplay { get; set; } = string.Empty;
        public string? ReleaseDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}