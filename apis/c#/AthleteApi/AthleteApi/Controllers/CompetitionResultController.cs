using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AthleteApi.Models;
using AthleteApi.Services;

namespace AthleteApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CompetitionResultController : ControllerBase
    {
        private readonly ICompetitionResultService _competitionResultService;
        private readonly ILogger<CompetitionResultController> _logger;

        public CompetitionResultController(ICompetitionResultService competitionResultService, ILogger<CompetitionResultController> logger)
        {
            _competitionResultService = competitionResultService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetCompetitionResults(
            [FromQuery] int? tournamentId = null, [FromQuery] string? tournamentName = null, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            if (!tournamentId.HasValue && string.IsNullOrEmpty(tournamentName))
            {
                return BadRequest(new ApiResponse("Se tiene que proveer obligatoriamente uno de estos valores: tournamentID o tournamentName.", 400));
            }

            try
            {
                var results = await _competitionResultService.GetCompetitionResults(tournamentId, tournamentName, pageNumber, pageSize);
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los resultados de la competencia");
                return StatusCode(500, new ApiResponse($"Internal server error: {ex.Message}", 500));
            }
        }
    }
}