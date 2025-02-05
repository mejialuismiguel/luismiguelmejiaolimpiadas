using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AthleteApi.Models;
using AthleteApi.Services;

namespace AthleteApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AttemptController : ControllerBase
    {
        private readonly IAttemptService _attemptService;
        private readonly ILogger<AttemptController> _logger;

        public AttemptController(IAttemptService attemptService, ILogger<AttemptController> logger)
        {
            _attemptService = attemptService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> AddAttempt([FromBody] Attempt attempt)
        {
            try
            {
                await _attemptService.AddAttempt(attempt);
                return Ok(new ApiResponse("Intento agregado satisfactoriamente", 0));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar el intento");
                return StatusCode(500, new ApiResponse($"Internal server error: {ex.Message}", 500));
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAttemptsByTournament(
            [FromQuery] int? tournamentId = null, [FromQuery] string? tournamentName = null, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var attempts = await _attemptService.GetAttemptsByTournament(tournamentId, tournamentName ?? string.Empty, pageNumber, pageSize);
                return Ok(attempts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los intentos");
                return StatusCode(500, new ApiResponse($"Internal server error: {ex.Message}", 500));
            }
        }
    }
}