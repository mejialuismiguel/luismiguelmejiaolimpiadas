using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AthleteApi.Models;
using AthleteApi.Services;

namespace AthleteApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TournamentController : ControllerBase
    {
        private readonly ITournamentService _tournamentService;
        private readonly ILogger<TournamentController> _logger;

        public TournamentController(ITournamentService tournamentService, ILogger<TournamentController> logger)
        {
            _tournamentService = tournamentService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTournament([FromBody] Tournament tournament)
        {
            try
            {
                await _tournamentService.CreateTournament(tournament);
                return Ok(new ApiResponse("Torneo creado satisfactoriamente", 0));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el torneo");
                return StatusCode(500, new ApiResponse($"Internal server error: {ex.Message}", 500));
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetTournaments(
            [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? name = null)
        {
            try
            {
                var tournaments = await _tournamentService.GetTournaments(pageNumber, pageSize, name);
                return Ok(tournaments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los torneos");
                return StatusCode(500, new ApiResponse($"Internal server error: {ex.Message}", 500));
            }
        }
    }
}