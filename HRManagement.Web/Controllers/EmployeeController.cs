using AutoMapper;
using HRManagement.Web.Constants;
using HRManagement.Web.Filters;
using HRManagement.Web.Models.API;
using HRManagement.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace HRManagement.Web.Controllers
{
    [AuthorizeSession]
    public class EmployeeController(
        ApiClient apiClient,
        IMapper mapper,
        ILogger<EmployeeController> logger) : BaseController
    {
        private readonly ApiClient _apiClient = apiClient;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<EmployeeController> _logger = logger;

        [HttpGet]
        public async Task<IActionResult> Index(string? searchTerm)
        {
            ViewBag.SearchTerm = searchTerm;

            var endpoint = string.IsNullOrWhiteSpace(searchTerm)
                ? ApiEndpoints.Employees
                : $"{ApiEndpoints.Employees}?searchTerm={Uri.EscapeDataString(searchTerm)}";

            var response = await _apiClient.GetAsync<List<EmployeeViewModel>>(endpoint);

            if (response.Success && response.Data != null)
            {
                _logger.LogInformation("Retrieved {Count} employees", response.Data.Count);
                return View(response.Data);
            }

            if (!string.IsNullOrEmpty(response.Message))
            {
                SetErrorMessage(response.Message);
            }

            return View(new List<EmployeeViewModel>());
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadPositionsAsync();
            return View(new EmployeeViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadPositionsAsync();
                return View(model);
            }

            var createDto = _mapper.Map<object>(model);
            var response = await _apiClient.PostAsync<EmployeeViewModel>(ApiEndpoints.Employees, createDto);

            if (response.Success)
            {
                _logger.LogInformation("Employee created: {PersonalNumber}", model.PersonalNumber);
                SetSuccessMessage(ValidationMessages.EmployeeCreated);
                return RedirectToAction(nameof(Index));
            }

            AddModelErrors(response);
            await LoadPositionsAsync();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var response = await _apiClient.GetAsync<EmployeeViewModel>(
                string.Format(ApiEndpoints.EmployeeById, id));

            if (!response.Success || response.Data == null)
            {
                SetErrorMessage(response.Message ?? ValidationMessages.EmployeeNotFound);
                return RedirectToAction(nameof(Index));
            }

            await LoadPositionsAsync();
            return View(response.Data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, EmployeeViewModel model)
        {
            if (id != model.Id)
            {
                _logger.LogWarning("ID mismatch in Edit: route={RouteId}, model={ModelId}", id, model.Id);
                ModelState.AddModelError(string.Empty, ValidationMessages.IdMismatch);
                await LoadPositionsAsync();
                return View(model);
            }

            if (!ModelState.IsValid)
            {
                await LoadPositionsAsync();
                return View(model);
            }

            var updateDto = _mapper.Map<object>(model);
            var response = await _apiClient.PutAsync<EmployeeViewModel>(
                string.Format(ApiEndpoints.EmployeeById, id), updateDto);

            if (response.Success)
            {
                _logger.LogInformation("Employee updated: {Id}", id);
                SetSuccessMessage(ValidationMessages.EmployeeUpdated);
                return RedirectToAction(nameof(Index));
            }

            AddModelErrors(response);
            await LoadPositionsAsync();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _apiClient.DeleteAsync(
                string.Format(ApiEndpoints.EmployeeById, id));

            if (response.Success)
            {
                _logger.LogInformation("Employee deleted: {Id}", id);
                SetSuccessMessage(ValidationMessages.EmployeeDeleted);
            }
            else
            {
                SetErrorMessage(response.Message ?? ValidationMessages.EmployeeDeleteFailed);
            }

            return RedirectToAction(nameof(Index));
        }

        #region Helper Methods

        private async Task LoadPositionsAsync()
        {
            var response = await _apiClient.GetAsync<List<PositionViewModel>>(ApiEndpoints.Positions);
            ViewBag.Positions = response.Success && response.Data != null
                ? response.Data
                : [];
        }

        #endregion
    }
}