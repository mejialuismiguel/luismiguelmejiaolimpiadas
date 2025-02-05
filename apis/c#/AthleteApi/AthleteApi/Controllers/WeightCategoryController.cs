using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AthleteApi.Models;
using AthleteApi.Services;

namespace AthleteApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class WeightCategoryController : ControllerBase
    {
        private readonly IWeightCategoryService _weightCategoryService;
        private readonly ILogger<WeightCategoryController> _logger;

        public WeightCategoryController(IWeightCategoryService weightCategoryService, ILogger<WeightCategoryController> logger)
        {
            _weightCategoryService = weightCategoryService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetWeightCategories([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var weightCategories = await _weightCategoryService.GetWeightCategories(pageNumber, pageSize);
                return Ok(weightCategories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las categorías de peso");
                return StatusCode(500, new ApiResponse($"Internal server error: {ex.Message}", 500));
            }
        }
    }
}