using AutoMapper;
using HRManagement.Web.Constants;
using HRManagement.Web.Filters;
using HRManagement.Web.Models.API;
using HRManagement.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace HRManagement.Web.Controllers
{
    [AuthorizeSession]
    public class PositionController(
        ApiClient apiClient,
        IMapper mapper,
        ILogger<PositionController> logger) : BaseController
    {
        private readonly ApiClient _apiClient = apiClient;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<PositionController> _logger = logger;

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var response = await _apiClient.GetAsync<List<PositionViewModel>>(ApiEndpoints.PositionTree);

            if (response.Success && response.Data != null)
            {
                _logger.LogInformation("Retrieved position tree with {Count} root positions", response.Data.Count);
                return View(response.Data);
            }

            if (!string.IsNullOrEmpty(response.Message))
            {
                SetErrorMessage(response.Message);
            }

            return View(new List<PositionViewModel>());
        }

        [HttpGet]
        public async Task<IActionResult> Create(Guid? parentId)
        {
            var model = new PositionViewModel();

            if (parentId.HasValue)
            {
                model.ParentId = parentId.Value;
                await LoadParentNameAsync(parentId.Value);
            }

            await LoadAllPositionsAsync();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PositionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                if (model.ParentId.HasValue)
                {
                    await LoadParentNameAsync(model.ParentId.Value);
                }
                await LoadAllPositionsAsync();
                return View(model);
            }

            var createDto = _mapper.Map<object>(model);
            var response = await _apiClient.PostAsync<PositionViewModel>(ApiEndpoints.Positions, createDto);

            if (response.Success)
            {
                _logger.LogInformation("Position created: {Name}, ParentId: {ParentId}",
                    model.Name, model.ParentId?.ToString() ?? "null");
                SetSuccessMessage(ValidationMessages.PositionCreated);
                return RedirectToAction(nameof(Index));
            }

            AddModelErrors(response);

            if (model.ParentId.HasValue)
            {
                await LoadParentNameAsync(model.ParentId.Value);
            }
            await LoadAllPositionsAsync();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _apiClient.DeleteAsync(
                string.Format(ApiEndpoints.PositionById, id));

            if (response.Success)
            {
                _logger.LogInformation("Position deleted: {Id}", id);
                SetSuccessMessage(ValidationMessages.PositionDeleted);
            }
            else
            {
                _logger.LogWarning("Failed to delete position {Id}: {Message}", id, response.Message);
                SetErrorMessage(response.Message ?? ValidationMessages.PositionDeleteFailed);
            }

            return RedirectToAction(nameof(Index));
        }

        #region Helper Methods

        private async Task LoadAllPositionsAsync()
        {
            var response = await _apiClient.GetAsync<List<PositionViewModel>>(ApiEndpoints.Positions);
            ViewBag.AllPositions = response.Success && response.Data != null
                ? response.Data
                : [];
        }

        private async Task LoadParentNameAsync(Guid parentId)
        {
            var parentResponse = await _apiClient.GetAsync<PositionViewModel>(
                string.Format(ApiEndpoints.PositionById, parentId));

            if (parentResponse.Success && parentResponse.Data != null)
            {
                ViewBag.ParentName = parentResponse.Data.Name;
                _logger.LogDebug("Creating child position for parent: {ParentName}", parentResponse.Data.Name);
            }
        }

        #endregion
    }
}