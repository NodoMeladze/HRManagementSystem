using HRManagement.WebApp.Models.ApiModels;
using HRManagement.WebApp.Models.ApiModels.Position;
using HRManagement.WebApp.Services.Interfaces;

namespace HRManagement.WebApp.Services
{
    public class PositionService : IPositionService
    {
        private readonly IApiService _apiService;
        private readonly ILogger<PositionService> _logger;

        public PositionService(
            IApiService apiService,
            ILogger<PositionService> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        public async Task<ApiResponse<IEnumerable<PositionDto>>?> GetAllAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all positions");
                return await _apiService.GetAsync<ApiResponse<IEnumerable<PositionDto>>>("position");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching positions");
                return new ApiResponse<IEnumerable<PositionDto>>
                {
                    Success = false,
                    Message = "თანამდებობების ჩატვირთვისას მოხდა შეცდომა"
                };
            }
        }

        public async Task<ApiResponse<IEnumerable<PositionDto>>?> GetTreeAsync()
        {
            try
            {
                _logger.LogInformation("Fetching position tree");
                return await _apiService.GetAsync<ApiResponse<IEnumerable<PositionDto>>>("position/tree");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching position tree");
                return new ApiResponse<IEnumerable<PositionDto>>
                {
                    Success = false,
                    Message = "თანამდებობების ხის ჩატვირთვისას მოხდა შეცდომა"
                };
            }
        }

        public async Task<ApiResponse<PositionDto>?> GetByIdAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("Fetching position: {PositionId}", id);
                return await _apiService.GetAsync<ApiResponse<PositionDto>>($"position/{id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching position: {PositionId}", id);
                return new ApiResponse<PositionDto>
                {
                    Success = false,
                    Message = "თანამდებობის ჩატვირთვისას მოხდა შეცდომა"
                };
            }
        }

        public async Task<ApiResponse<PositionDto>?> CreateAsync(CreatePositionRequest request)
        {
            try
            {
                _logger.LogInformation("Creating position: {Name}", request.Name);

                return await _apiService.PostAsync<CreatePositionRequest, ApiResponse<PositionDto>>(
                    "position",
                    request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating position");
                return new ApiResponse<PositionDto>
                {
                    Success = false,
                    Message = "თანამდებობის დამატებისას მოხდა შეცდომა"
                };
            }
        }

        public async Task<ApiResponse<bool>?> DeleteAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("Deleting position: {PositionId}", id);
                return await _apiService.DeleteAsync<ApiResponse<bool>>($"position/{id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting position: {PositionId}", id);
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "თანამდებობის წაშლისას მოხდა შეცდომა"
                };
            }
        }
    }
}