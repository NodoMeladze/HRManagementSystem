namespace HRManagement.Application.DTOs.Position
{
    public class PositionDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid? ParentId { get; set; }
        public string? ParentName { get; set; }
        public int Level { get; set; }
        public List<PositionDto> Children { get; set; } = [];
        public int EmployeeCount { get; set; }
    }
}
