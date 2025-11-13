using HRManagement.Application.DTOs.Common;
using HRManagement.Application.DTOs.Position;

namespace HRManagement.Application.Interfaces
{
    public interface IPositionService
    {
        Task<ResultDto<IEnumerable<PositionDto>>> GetAllAsync();
        Task<ResultDto<IEnumerable<PositionDto>>> GetTreeAsync();
        Task<ResultDto<PositionDto>> GetByIdAsync(Guid id);
        Task<ResultDto<PositionDto>> CreateAsync(CreatePositionDto createDto);
        Task<ResultDto<bool>> DeleteAsync(Guid id);
    }
}
