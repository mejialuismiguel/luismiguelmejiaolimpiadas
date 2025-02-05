using System.Data;
using Microsoft.Data.SqlClient;
using AthleteApi.Models;

namespace AthleteApi.Services
{
    public class CompetitionResultService : ICompetitionResultService
    {
        private readonly string _connectionString;

        public CompetitionResultService(IConfiguration configuration)
        {
             _connectionString = configuration?.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<IEnumerable<CompetitionResult>> GetCompetitionResults(int? tournamentId, string? tournamentName, int pageNumber, int pageSize)
        {
            var results = new List<CompetitionResult>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_getCompetitionResults", conn))
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
                            var result = new CompetitionResult
                            {
                                Pais = reader.GetString(0),
                                Nombre = reader.GetString(1),
                                Arranque = reader.GetDouble(2),
                                Envion = reader.GetDouble(3),
                                TotalPeso = reader.GetDouble(4)
                            };
                            results.Add(result);
                        }
                    }
                }
            }

            return results;
        }
    }
}