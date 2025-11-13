using HRManagement.WebApp.Attributes;
using HRManagement.WebApp.Helpers;
using HRManagement.WebApp.Models.ApiModels.Employee;
using HRManagement.WebApp.Models.ViewModels.Employee;

using HRManagement.WebApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HRManagement.WebApp.Controllers
{
    [Authorize]
    public class EmployeeController : BaseController
    {
        private readonly IEmployeeService _employeeService;
        private readonly IPositionService _positionService;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(
            IEmployeeService employeeService,
            IPositionService positionService,
            ILogger<EmployeeController> logger)
        {
            _employeeService = employeeService;
            _positionService = positionService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? searchTerm)
        {
            var response = await _employeeService.GetAllAsync(searchTerm);

            if (response?.Success == true && response.Data != null)
            {
                var viewModel = new EmployeeListViewModel
                {
                    Employees = response.Data.ToList(),
                    SearchTerm = searchTerm
                };

                return View(viewModel);
            }

            SetErrorMessage(response?.Message ?? "თანამშრომლების ჩატვირთვა ვერ მოხერხდა");
            return View(new EmployeeListViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var response = await _employeeService.GetByIdAsync(id);

            if (response?.Success == true && response.Data != null)
            {
                var viewModel = new EmployeeDetailsViewModel
                {
                    Employee = response.Data
                };

                return View(viewModel);
            }

            SetErrorMessage(response?.Message ?? "თანამშრომელი ვერ მოიძებნა");
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var viewModel = new CreateEmployeeViewModel
            {
                DateOfBirth = DateTime.Today.AddYears(-25),
                Genders = SelectListHelper.GetGenderList(),
                Statuses = SelectListHelper.GetEmployeeStatusList()
            };

            await LoadPositionsAsync(viewModel);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEmployeeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Genders = SelectListHelper.GetGenderList();
                model.Statuses = SelectListHelper.GetEmployeeStatusList();
                await LoadPositionsAsync(model);
                return View(model);
            }

            // Validate ReleaseDate if Status is Released
            if (model.Status == 3 && !model.ReleaseDate.HasValue)
            {
                ModelState.AddModelError(nameof(model.ReleaseDate),
                    "გათავისუფლების თარიღი სავალდებულოა როდესაც სტატუსია 'გათავისუფლებული'");

                model.Genders = SelectListHelper.GetGenderList();
                model.Statuses = SelectListHelper.GetEmployeeStatusList();
                await LoadPositionsAsync(model);
                return View(model);
            }

            var request = new CreateEmployeeRequest
            {
                PersonalNumber = model.PersonalNumber,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Gender = model.Gender,
                DateOfBirth = DateOnly.FromDateTime(model.DateOfBirth).ToString("yyyy-MM-dd"),
                Email = model.Email,
                PositionId = model.PositionId,
                Status = model.Status,
                ReleaseDate = model.ReleaseDate.HasValue
                    ? DateOnly.FromDateTime(model.ReleaseDate.Value).ToString("yyyy-MM-dd")
                    : null
            };

            var response = await _employeeService.CreateAsync(request);

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
                ModelState.AddModelError(string.Empty, response?.Message ?? "თანამშრომლის დამატება ვერ მოხერხდა");
            }

            model.Genders = SelectListHelper.GetGenderList();
            model.Statuses = SelectListHelper.GetEmployeeStatusList();
            await LoadPositionsAsync(model);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var response = await _employeeService.GetByIdAsync(id);

            if (response?.Success != true || response.Data == null)
            {
                SetErrorMessage(response?.Message ?? "თანამშრომელი ვერ მოიძებნა");
                return RedirectToAction(nameof(Index));
            }

            var employee = response.Data;

            var viewModel = new UpdateEmployeeViewModel
            {
                Id = employee.Id,
                PersonalNumber = employee.PersonalNumber,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Gender = employee.Gender,
                DateOfBirth = DateTime.Parse(employee.DateOfBirth),
                Email = employee.Email,
                PositionId = employee.PositionId,
                Status = employee.Status,
                ReleaseDate = !string.IsNullOrEmpty(employee.ReleaseDate)
                    ? DateTime.Parse(employee.ReleaseDate)
                    : null,
                Genders = SelectListHelper.GetGenderList(),
                Statuses = SelectListHelper.GetEmployeeStatusList()
            };

            await LoadPositionsAsync(viewModel);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateEmployeeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Genders = SelectListHelper.GetGenderList();
                model.Statuses = SelectListHelper.GetEmployeeStatusList();
                await LoadPositionsAsync(model);
                return View(model);
            }

            // Validate ReleaseDate if Status is Released
            if (model.Status == 3 && !model.ReleaseDate.HasValue)
            {
                ModelState.AddModelError(nameof(model.ReleaseDate),
                    "გათავისუფლების თარიღი სავალდებულოა როდესაც სტატუსია 'გათავისუფლებული'");

                model.Genders = SelectListHelper.GetGenderList();
                model.Statuses = SelectListHelper.GetEmployeeStatusList();
                await LoadPositionsAsync(model);
                return View(model);
            }

            var request = new UpdateEmployeeRequest
            {
                Id = model.Id,
                PersonalNumber = model.PersonalNumber,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Gender = model.Gender,
                DateOfBirth = DateOnly.FromDateTime(model.DateOfBirth).ToString("yyyy-MM-dd"),
                Email = model.Email,
                PositionId = model.PositionId,
                Status = model.Status,
                ReleaseDate = model.ReleaseDate.HasValue
                    ? DateOnly.FromDateTime(model.ReleaseDate.Value).ToString("yyyy-MM-dd")
                    : null
            };

            var response = await _employeeService.UpdateAsync(request);

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
                ModelState.AddModelError(string.Empty, response?.Message ?? "თანამშრომლის განახლება ვერ მოხერხდა");
            }

            model.Genders = SelectListHelper.GetGenderList();
            model.Statuses = SelectListHelper.GetEmployeeStatusList();
            await LoadPositionsAsync(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _employeeService.DeleteAsync(id);

            if (response?.Success == true)
            {
                SetSuccessMessage(response.Message);
            }
            else
            {
                SetErrorMessage(response?.Message ?? "თანამშრომლის წაშლა ვერ მოხერხდა");
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task LoadPositionsAsync(CreateEmployeeViewModel model)
        {
            var positionsResponse = await _positionService.GetAllAsync();

            if (positionsResponse?.Success == true && positionsResponse.Data != null)
            {
                model.Positions = positionsResponse.Data
                    .Select(p => new SelectListItem
                    {
                        Value = p.Id.ToString(),
                        Text = p.Name
                    })
                    .Prepend(new SelectListItem { Value = "", Text = "აირჩიეთ თანამდებობა" })
                    .ToList();
            }
            else
            {
                model.Positions = new List<SelectListItem>
                {
                    new SelectListItem { Value = "", Text = "აირჩიეთ თანამდებობა" }
                };
            }
        }

        private async Task LoadPositionsAsync(UpdateEmployeeViewModel model)
        {
            var positionsResponse = await _positionService.GetAllAsync();

            if (positionsResponse?.Success == true && positionsResponse.Data != null)
            {
                model.Positions = positionsResponse.Data
                    .Select(p => new SelectListItem
                    {
                        Value = p.Id.ToString(),
                        Text = p.Name
                    })
                    .Prepend(new SelectListItem { Value = "", Text = "აირჩიეთ თანამდებობა" })
                    .ToList();
            }
            else
            {
                model.Positions = new List<SelectListItem>
                {
                    new SelectListItem { Value = "", Text = "აირჩიეთ თანამდებობა" }
                };
            }
        }
    }
}