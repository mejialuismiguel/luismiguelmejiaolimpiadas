using System.Collections.Generic;
using System.Threading.Tasks;
using AthleteApi.Models;

namespace AthleteApi.Services
{
    public interface ICompetitionResultService
    {
        Task<IEnumerable<CompetitionResult>> GetCompetitionResults(int? tournamentId, string? tournamentName, int pageNumber, int pageSize);
    }
}