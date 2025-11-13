using HRManagement.WebApp.Attributes;
using HRManagement.WebApp.Models.ApiModels.Position;
using HRManagement.WebApp.Models.ViewModels.Position;
using HRManagement.WebApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HRManagement.WebApp.Controllers
{
    [Authorize]
    public class PositionController : BaseController
    {
        private readonly IPositionService _positionService;
        private readonly ILogger<PositionController> _logger;

        public PositionController(
            IPositionService positionService,
            ILogger<PositionController> logger)
        {
            _positionService = positionService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var response = await _positionService.GetTreeAsync();

            if (response?.Success == true && response.Data != null)
            {
                var viewModel = new PositionListViewModel
                {
                    Positions = response.Data.ToList()
                };

                return View(viewModel);
            }

            SetErrorMessage(response?.Message ?? "თანამდებობების ჩატვირთვა ვერ მოხერხდა");
            return View(new PositionListViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> Create(Guid? parentId)
        {
            var viewModel = new CreatePositionViewModel
            {
                ParentId = parentId
            };

            await LoadPositionsAsync(viewModel);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePositionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadPositionsAsync(model);
                return View(model);
            }

            var request = new CreatePositionRequest
            {
                Name = model.Name,
                ParentId = model.ParentId
            };

            var response = await _positionService.CreateAsync(request);

            if (response?.Success == true)
            {
                SetSuccessMessage(response.Message);
                return RedirectToAction(nameof(Index));
            }

            // Handle validation errors from API
            if (response?.Errors != null && response.Errors.Any())
            {
                foreach (var error in response.Errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, response?.Message ?? "თანამდებობის დამატება ვერ მოხერხდა");
            }

            await LoadPositionsAsync(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _positionService.DeleteAsync(id);

            if (response?.Success == true)
            {
                SetSuccessMessage(response.Message);
            }
            else
            {
                SetErrorMessage(response?.Message ?? "თანამდებობის წაშლა ვერ მოხერხდა");
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task LoadPositionsAsync(CreatePositionViewModel model)
        {
            var positionsResponse = await _positionService.GetAllAsync();

            if (positionsResponse?.Success == true && positionsResponse.Data != null)
            {
                model.Positions = positionsResponse.Data
                    .Select(p => new SelectListItem
                    {
                        Value = p.Id.ToString(),
                        Text = new string('-', p.Level * 2) + " " + p.Name
                    })
                    .Prepend(new SelectListItem { Value = "", Text = "--- ძირითადი თანამდებობა ---" })
                    .ToList();
            }
            else
            {
                model.Positions = new List<SelectListItem>
                {
                    new SelectListItem { Value = "", Text = "--- ძირითადი თანამდებობა ---" }
                };
            }
        }
    }
}