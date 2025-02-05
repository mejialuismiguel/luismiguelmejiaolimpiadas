using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AthleteApi.Models;
using AthleteApi.Services;

namespace AthleteApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TournamentParticipationController : ControllerBase
    {
        private readonly ITournamentParticipationService _tournamentParticipationService;
        private readonly ILogger<TournamentParticipationController> _logger;

        public TournamentParticipationController(ITournamentParticipationService tournamentParticipationService, ILogger<TournamentParticipationController> logger)
        {
            _tournamentParticipationService = tournamentParticipationService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> AddParticipant([FromBody] TournamentParticipation participation)
        {
            try
            {
                await _tournamentParticipationService.AddParticipant(participation);
                return Ok(new ApiResponse("Participante agregado satisfactoriamente", 0));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar el participante");
                return StatusCode(500, new ApiResponse($"Internal server error: {ex.Message}", 500));
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetTournamentParticipations(
            [FromQuery] int? tournamentId = null, [FromQuery] string? athleteName = null, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var participations = await _tournamentParticipationService.GetParticipants(tournamentId, athleteName, pageNumber, pageSize);
                return Ok(participations);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las participaciones en el torneo");
                return StatusCode(500, new ApiResponse($"Internal server error: {ex.Message}", 500));
            }
        }
    }
}