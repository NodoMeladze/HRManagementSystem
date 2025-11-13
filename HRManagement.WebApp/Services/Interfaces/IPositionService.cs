using HRManagement.WebApp.Models.ApiModels;
using HRManagement.WebApp.Models.ApiModels.Position;

namespace HRManagement.WebApp.Services.Interfaces
{
    public interface IPositionService
    {
        Task<ApiResponse<IEnumerable<PositionDto>>?> GetAllAsync();
        Task<ApiResponse<IEnumerable<PositionDto>>?> GetTreeAsync();
        Task<ApiResponse<PositionDto>?> GetByIdAsync(Guid id);
        Task<ApiResponse<PositionDto>?> CreateAsync(CreatePositionRequest request);
        Task<ApiResponse<bool>?> DeleteAsync(Guid id);
    }
}