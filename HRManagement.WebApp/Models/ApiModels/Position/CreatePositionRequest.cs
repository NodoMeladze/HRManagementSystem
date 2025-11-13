namespace HRManagement.WebApp.Models.ApiModels.Position
{
    public class CreatePositionRequest
    {
        public string Name { get; set; } = string.Empty;
        public Guid? ParentId { get; set; }
    }
}