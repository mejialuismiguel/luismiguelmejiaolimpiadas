using System.Data;
using Microsoft.Data.SqlClient;
using AthleteApi.Models;

namespace AthleteApi.Services
{
    public class TournamentParticipationService : ITournamentParticipationService
    {
        private readonly string _connectionString;

        public TournamentParticipationService(IConfiguration configuration)
        {
            _connectionString = configuration?.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException(nameof(configuration), "Configuration cannot be null");
        }

        public async Task AddParticipant(TournamentParticipation participation)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_addParticipant", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@AthleteId", participation.AthleteId);
                    cmd.Parameters.AddWithValue("@TournamentId", participation.TournamentId);
                    cmd.Parameters.AddWithValue("@WeightCategoryId", participation.WeightCategoryId);

                    conn.Open();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<IEnumerable<TournamentParticipation>> GetParticipants(int pageNumber, int pageSize)
        {
            var participants = new List<TournamentParticipation>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_getParticipants", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
                    cmd.Parameters.AddWithValue("@PageSize", pageSize);

                    conn.Open();
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var participation = new TournamentParticipation
                            {
                                Id = reader.GetInt32(0),
                                AthleteId = reader.GetInt32(1),
                                TournamentId = reader.GetInt32(2),
                                WeightCategoryId = reader.GetInt32(3)
                            };
                            participants.Add(participation);
                        }
                    }
                }
            }

            return participants;
        }
    }
}