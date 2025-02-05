using System.Collections.Generic;
using System.Threading.Tasks;
using AthleteApi.Models;

namespace AthleteApi.Services
{
    public interface IAthleteAttemptSummaryService
    {
        Task<IEnumerable<AthleteAttemptSummary>> GetAthleteAttemptSummary(int? tournamentId, int? athleteId, string? athleteDni, string? athleteName, int pageNumber, int pageSize);
    }
}