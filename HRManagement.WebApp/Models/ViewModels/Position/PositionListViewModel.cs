using HRManagement.WebApp.Models.ApiModels.Position;

namespace HRManagement.WebApp.Models.ViewModels.Position
{
    public class PositionListViewModel
    {
        public List<PositionDto> Positions { get; set; } = new();
    }
}