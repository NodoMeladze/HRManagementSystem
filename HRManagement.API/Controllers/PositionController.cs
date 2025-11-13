using FluentValidation;
using HRManagement.Application.DTOs.Position;
using HRManagement.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PositionController(
        IPositionService positionService,
        IValidator<CreatePositionDto> createValidator,
        ILogger<PositionController> logger) : ControllerBase
    {
        private readonly IPositionService _positionService = positionService;
        private readonly IValidator<CreatePositionDto> _createValidator = createValidator;
        private readonly ILogger<PositionController> _logger = logger;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _positionService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("tree")]
        public async Task<IActionResult> GetTree()
        {
            var result = await _positionService.GetTreeAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _positionService.GetByIdAsync(id);

            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePositionDto createDto)
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

            var result = await _positionService.CreateAsync(createDto);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return CreatedAtAction(nameof(GetById), new { id = result.Data?.Id }, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _positionService.DeleteAsync(id);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
