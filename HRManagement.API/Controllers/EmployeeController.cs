using FluentValidation;
using HRManagement.Application.DTOs.Employee;
using HRManagement.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EmployeeController(
        IEmployeeService employeeService,
        IValidator<CreateEmployeeDto> createValidator,
        IValidator<UpdateEmployeeDto> updateValidator,
        ILogger<EmployeeController> logger) : ControllerBase
    {
        private readonly IEmployeeService _employeeService = employeeService;
        private readonly IValidator<CreateEmployeeDto> _createValidator = createValidator;
        private readonly IValidator<UpdateEmployeeDto> _updateValidator = updateValidator;
        private readonly ILogger<EmployeeController> _logger = logger;

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? searchTerm = null)
        {
            var result = await _employeeService.GetAllAsync(searchTerm);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _employeeService.GetByIdAsync(id);

            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateEmployeeDto createDto)
        {
            var validationResult = await _createValidator.ValidateAsync(createDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "ვალიდაციის შეცდომა",
                    errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList()
                });
            }

            var result = await _employeeService.CreateAsync(createDto);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return CreatedAtAction(nameof(GetById), new { id = result.Data?.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateEmployeeDto updateDto)
        {
            if (id != updateDto.Id)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "ID-ები არ ემთხვევა",
                    errors = new[] { "Route ID and body ID do not match" }
                });
            }

            var validationResult = await _updateValidator.ValidateAsync(updateDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "ვალიდაციის შეცდომა",
                    errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList()
                });
            }

            var result = await _employeeService.UpdateAsync(updateDto);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _employeeService.DeleteAsync(id);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
