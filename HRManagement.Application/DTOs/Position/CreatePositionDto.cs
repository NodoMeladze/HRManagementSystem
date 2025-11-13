namespace HRManagement.Application.DTOs.Position
{
    public class CreatePositionDto
    {
        public string Name { get; set; } = string.Empty;
        public Guid? ParentId { get; set; }
    }
}
