using HRManagement.Domain.Enums;

namespace HRManagement.Domain.Entities
{
    public class Employee : BaseEntity
    {
        public string PersonalNumber { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public Gender Gender { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string? Email { get; set; }

        public Guid PositionId { get; set; }
        public Position Position { get; set; } = null!;

        public EmployeeStatus Status { get; set; }
        public DateOnly? ReleaseDate { get; set; }
        public bool IsActive { get; set; } = false;
        public DateTime? ActivationScheduledAt { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
