using System.Collections.Generic;
using System.Threading.Tasks;
using AthleteApi.Models;

namespace AthleteApi.Services
{
    public interface ITournamentParticipationService
    {
        Task AddParticipant(TournamentParticipation participation);
        Task<IEnumerable<TournamentParticipation>> GetParticipants(int pageNumber, int pageSize);
    }
}