using AthleteApi.Models;

namespace AthleteApi.Services
{
    public interface IAthleteService
    {
        Task CreateAthlete(Athlete athlete);
        Task<IEnumerable<Athlete>> GetAthletes(
            int pageNumber, int pageSize, string? name = null);
    }
}