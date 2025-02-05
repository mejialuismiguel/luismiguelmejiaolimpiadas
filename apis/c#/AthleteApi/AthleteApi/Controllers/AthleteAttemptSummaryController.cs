using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AthleteApi.Models;
using AthleteApi.Services;

namespace AthleteApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AthleteAttemptSummaryController : ControllerBase
    {
        private readonly IAthleteAttemptSummaryService _athleteAttemptSummaryService;
        private readonly ILogger<AthleteAttemptSummaryController> _logger;

        public AthleteAttemptSummaryController(IAthleteAttemptSummaryService athleteAttemptSummaryService, ILogger<AthleteAttemptSummaryController> logger)
        {
            _athleteAttemptSummaryService = athleteAttemptSummaryService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAthleteAttemptSummary([FromQuery] int? tournamentId = null, [FromQuery] int? athleteId = null, [FromQuery] string? athleteDni = null, [FromQuery] string? athleteName = null, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var summaries = await _athleteAttemptSummaryService.GetAthleteAttemptSummary(tournamentId, athleteId, athleteDni, athleteName, pageNumber, pageSize);
                return Ok(summaries);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el resumen de intentos de los deportistas");
                return StatusCode(500, new ApiResponse($"Internal server error: {ex.Message}", 500));
            }
        }
    }
}