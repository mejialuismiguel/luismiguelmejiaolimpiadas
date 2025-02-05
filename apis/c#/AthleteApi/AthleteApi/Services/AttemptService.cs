using System.Data;
using Microsoft.Data.SqlClient;
using AthleteApi.Models;

namespace AthleteApi.Services
{
    public class AttemptService : IAttemptService
    {
        private readonly string _connectionString;

        public AttemptService(IConfiguration configuration)
        {
            _connectionString = configuration?.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException(nameof(configuration), "Configuration cannot be null");
        }

        public async Task AddAttempt(Attempt attempt)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_addattempt", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ParticipationId", attempt.ParticipationId);
                    cmd.Parameters.AddWithValue("@AttemptNumber", attempt.AttemptNumber);
                    cmd.Parameters.AddWithValue("@Type", attempt.Type);
                    cmd.Parameters.AddWithValue("@WeightLifted", attempt.WeightLifted);
                    cmd.Parameters.AddWithValue("@Success", attempt.Success);

                    conn.Open();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<IEnumerable<Attempt>> GetAttemptsByTournament(int? tournamentId, string tournamentName, int pageNumber, int pageSize)
        {
            var attempts = new List<Attempt>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_getattemptsbytournament", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@TournamentId", tournamentId.HasValue ? (object)tournamentId.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@TournamentName", tournamentName ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
                    cmd.Parameters.AddWithValue("@PageSize", pageSize);

                    conn.Open();
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var attempt = new Attempt
                            {
                                Id = reader.GetInt32(0),
                                ParticipationId = reader.GetInt32(1),
                                AttemptNumber = reader.GetInt32(2),
                                Type = reader.GetString(3),
                                WeightLifted = reader.GetDouble(4),
                                Success = reader.GetBoolean(5) ? 1 : 0,
                                TournamentName = reader.GetString(6),
                                TournamentId = reader.GetInt32(7) 
                            };
                            attempts.Add(attempt);
                        }
                    }
                }
            }

            return attempts;
        }
    }
}