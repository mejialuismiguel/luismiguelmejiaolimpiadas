using System.Data;
using Microsoft.Data.SqlClient;
using AthleteApi.Models;

namespace AthleteApi.Services
{
    public class TournamentService : ITournamentService
    {
        private readonly string _connectionString;

        public TournamentService(IConfiguration configuration)
        {
            _connectionString = configuration?.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task CreateTournament(Tournament tournament)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_createTournament", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Name", tournament.Name);
                    cmd.Parameters.AddWithValue("@Location", tournament.Location);
                    cmd.Parameters.AddWithValue("@StartDate", tournament.StartDate);
                    cmd.Parameters.AddWithValue("@EndDate", tournament.EndDate);

                    conn.Open();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<IEnumerable<Tournament>> GetTournaments(
            int pageNumber, int pageSize, string? name = null)
        {
            var tournaments = new List<Tournament>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_getTournaments", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
                    cmd.Parameters.AddWithValue("@PageSize", pageSize);
                    cmd.Parameters.AddWithValue("@Name", name ?? (object)DBNull.Value);

                    conn.Open();
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var tournament = new Tournament
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Location = reader.GetString(2),
                                StartDate = reader.GetDateTime(3),
                                EndDate = reader.GetDateTime(4)
                            };
                            tournaments.Add(tournament);
                        }
                    }
                }
            }

            return tournaments;
        }
    }
}