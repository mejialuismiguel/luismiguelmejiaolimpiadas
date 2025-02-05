using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AthleteApi.Models;
using AthleteApi.Services;

namespace AthleteApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AthleteController : ControllerBase
    {
        private readonly IAthleteService _athleteService;
        private readonly ILogger<AthleteController> _logger;

        public AthleteController(IAthleteService athleteService, ILogger<AthleteController> logger)
        {
            _athleteService = athleteService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAthlete([FromBody] Athlete athlete)
        {
            try
            {
                await _athleteService.CreateAthlete(athlete);
                return Ok(new ApiResponse("Atleta creado satisfactoriamente", 0));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el atleta");
                return StatusCode(500, new ApiResponse($"Internal server error: {ex.Message}", 500));
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAthletes(
            [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? name = null)
        {
            try
            {
                var athletes = await _athleteService.GetAthletes(pageNumber, pageSize, name);
                return Ok(athletes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los atletas");
                return StatusCode(500, new ApiResponse($"Internal server error: {ex.Message}", 500));
            }
        }
    }
}