using System.Data;
using Microsoft.Data.SqlClient;
using AthleteApi.Models;

namespace AthleteApi.Services
{
    public class AthleteAttemptSummaryService : IAthleteAttemptSummaryService
    {
        private readonly string _connectionString;

        public AthleteAttemptSummaryService(IConfiguration configuration)
        {
            _connectionString = configuration?.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<IEnumerable<AthleteAttemptSummary>> GetAthleteAttemptSummary(int? tournamentId, int? athleteId, string? athleteDni, string? athleteName, int pageNumber, int pageSize)
        {
            var summaries = new List<AthleteAttemptSummary>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_getAthleteAttemptSummary", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@TournamentId", tournamentId.HasValue ? (object)tournamentId.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@AthleteId", athleteId.HasValue ? (object)athleteId.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@AthleteDni", athleteDni ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@AthleteName", athleteName ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
                    cmd.Parameters.AddWithValue("@PageSize", pageSize);

                    conn.Open();
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var summary = new AthleteAttemptSummary
                            {
                                AthleteName = reader.GetString(0),
                                AthleteDni = reader.GetString(1), 
                                TournamentName = reader.GetString(2),
                                TournamentId = reader.GetInt32(3), 
                                TotalAttempts = reader.GetInt32(4)
                            };
                            summaries.Add(summary);
                        }
                    }
                }
            }

            return summaries;
        }
    }
}