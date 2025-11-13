using AutoMapper;
using HRManagement.Application.DTOs.Common;
using HRManagement.Application.DTOs.Position;
using HRManagement.Application.Exceptions;
using HRManagement.Application.Interfaces;
using HRManagement.Domain.Entities;
using HRManagement.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace HRManagement.Application.Services
{
    public class PositionService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<PositionService> logger) : IPositionService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<PositionService> _logger = logger;

        public async Task<ResultDto<IEnumerable<PositionDto>>> GetAllAsync()
        {
            try
            {
                var positions = await _unitOfWork.Positions.GetAllAsync();
                var positionDtos = _mapper.Map<IEnumerable<PositionDto>>(positions);

                return ResultDto<IEnumerable<PositionDto>>.SuccessResult(positionDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting positions");
                return ResultDto<IEnumerable<PositionDto>>.FailureResult(
                    "თანამდებობების ჩატვირთვისას მოხდა შეცდომა",
                    [ex.Message]);
            }
        }

        public async Task<ResultDto<IEnumerable<PositionDto>>> GetTreeAsync()
        {
            try
            {
                var positions = await _unitOfWork.Positions.GetAllWithEmployeeCountAsync();
                var positionList = positions.ToList();

                var positionDtos = _mapper.Map<List<PositionDto>>(positionList);
                var tree = BuildTree(positionDtos);

                return ResultDto<IEnumerable<PositionDto>>.SuccessResult(tree);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting position tree");
                return ResultDto<IEnumerable<PositionDto>>.FailureResult(
                    "თანამდებობების ხის ჩატვირთვისას მოხდა შეცდომა",
                    [ex.Message]);
            }
        }

        public async Task<ResultDto<PositionDto>> GetByIdAsync(Guid id)
        {
            try
            {
                var position = await _unitOfWork.Positions.GetByIdAsync(id);

                if (position == null)
                {
                    return ResultDto<PositionDto>.FailureResult("თანამდებობა ვერ მოიძებნა");
                }

                var positionDto = _mapper.Map<PositionDto>(position);
                return ResultDto<PositionDto>.SuccessResult(positionDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting position by id: {Id}", id);
                return ResultDto<PositionDto>.FailureResult(
                    "თანამდებობის ჩატვირთვისას მოხდა შეცდომა",
                    [ex.Message]);
            }
        }

        public async Task<ResultDto<PositionDto>> CreateAsync(CreatePositionDto createDto)
        {
            try
            {
                var positionDto = await _unitOfWork.ExecuteInTransactionAsync(async () =>
                {
                    int level = 0;

                    if (createDto.ParentId.HasValue)
                    {
                        var parent = await _unitOfWork.Positions
                            .GetByIdAsync(createDto.ParentId.Value)
                            ?? throw new BusinessException("მშობელი თანამდებობა ვერ მოიძებნა");

                        level = parent.Level + 1;
                    }

                    var position = new Position
                    {
                        Name = createDto.Name,
                        ParentId = createDto.ParentId,
                        Level = level
                    };

                    await _unitOfWork.Positions.AddAsync(position);
                    await _unitOfWork.SaveChangesAsync();

                    _logger.LogInformation(
                        "Position created: {Id}, Level: {Level}",
                        position.Id,
                        position.Level);

                    return _mapper.Map<PositionDto>(position);
                });

                return ResultDto<PositionDto>.SuccessResult(
                    positionDto,
                    "თანამდებობა წარმატებით დაემატა");
            }
            catch (BusinessException ex)
            {
                return ResultDto<PositionDto>.FailureResult(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating position");
                return ResultDto<PositionDto>.FailureResult(
                    "თანამდებობის დამატებისას მოხდა შეცდომა",
                    [ex.Message]);
            }
        }

        public async Task<ResultDto<bool>> DeleteAsync(Guid id)
        {
            try
            {
                await _unitOfWork.ExecuteInTransactionAsync(async () =>
                {
                    var position = await _unitOfWork.Positions
                        .GetByIdAsync(id)
                        ?? throw new BusinessException("თანამდებობა ვერ მოიძებნა");

                    if (await _unitOfWork.Positions.HasChildrenAsync(id))
                    {
                        throw new BusinessException(
                            "თანამდებობის წაშლა შეუძლებელია, რადგან მას აქვს შვილობილი თანამდებობები");
                    }

                    if (await _unitOfWork.Positions.HasEmployeesAsync(id))
                    {
                        throw new BusinessException(
                            "თანამდებობის წაშლა შეუძლებელია, რადგან მას მიეკუთვნება თანამშრომლები");
                    }

                    position.DeletedAt = DateTime.UtcNow;
                    _unitOfWork.Positions.Update(position);
                    await _unitOfWork.SaveChangesAsync();

                    _logger.LogInformation("Position deleted (soft): {Id}", id);

                    return true;
                });

                return ResultDto<bool>.SuccessResult(true, "თანამდებობა წარმატებით წაიშალა");
            }
            catch (BusinessException ex)
            {
                return ResultDto<bool>.FailureResult(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting position: {Id}", id);
                return ResultDto<bool>.FailureResult(
                    "თანამდებობის წაშლისას მოხდა შეცდომა",
                    [ex.Message]);
            }
        }

        private static List<PositionDto> BuildTree(List<PositionDto> allPositions)
        {
            var lookup = allPositions.ToLookup(p => p.ParentId);

            foreach (var position in allPositions)
            {
                position.Children = [.. lookup[position.Id]];
            }

            return [.. lookup[null]];
        }
    }
}