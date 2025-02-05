using AthleteApi.Models;

namespace AthleteApi.Services
{
    public interface ITournamentService
    {
        Task CreateTournament(Tournament tournament);
        Task<IEnumerable<Tournament>> GetTournaments(int pageNumber, int pageSize, string? name = null);
    }
}