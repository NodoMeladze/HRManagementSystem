namespace HRManagement.WebApp.Models.ApiModels.Employee
{
    public class UpdateEmployeeRequest
    {
        public Guid Id { get; set; }
        public string PersonalNumber { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int Gender { get; set; }
        public string DateOfBirth { get; set; } = string.Empty;
        public string? Email { get; set; }
        public Guid PositionId { get; set; }
        public int Status { get; set; }
        public string? ReleaseDate { get; set; }
    }
}