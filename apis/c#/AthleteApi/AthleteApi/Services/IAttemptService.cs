using System.Collections.Generic;
using System.Threading.Tasks;
using AthleteApi.Models;

namespace AthleteApi.Services
{
    public interface IAttemptService
    {
        Task AddAttempt(Attempt attempt);
        Task<IEnumerable<Attempt>> GetAttemptsByTournament(int? tournamentId, string tournamentName, int pageNumber, int pageSize);
    }
}