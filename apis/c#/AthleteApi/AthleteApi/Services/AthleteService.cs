using System.Data;
using Microsoft.Data.SqlClient;
using AthleteApi.Models;

namespace AthleteApi.Services
{
    public class AthleteService : IAthleteService
    {
        private readonly string _connectionString;

        public AthleteService(IConfiguration configuration)
        {
            _connectionString = configuration?.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task CreateAthlete(Athlete athlete)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_createathlete", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_dni", athlete.Dni);
                    cmd.Parameters.AddWithValue("@p_first_name", athlete.FirstName);
                    cmd.Parameters.AddWithValue("@p_last_name", athlete.LastName);
                    cmd.Parameters.AddWithValue("@p_birth_date", athlete.BirthDate);
                    cmd.Parameters.AddWithValue("@p_gender", athlete.Gender);
                    cmd.Parameters.AddWithValue("@p_country_id", athlete.CountryId);
                    cmd.Parameters.AddWithValue("@p_weight_category_id", athlete.WeightCategoryId);

                    conn.Open();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<IEnumerable<Athlete>> GetAthletes(
            int pageNumber, int pageSize, string? name = null)
        {
            var athletes = new List<Athlete>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_getathletes", conn))
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
                            var athlete = new Athlete
                            {
                                Id = reader.GetInt32(0),
                                Dni = reader.GetString(1),
                                FirstName = reader.GetString(2),
                                LastName = reader.GetString(3),
                                BirthDate = reader.GetDateTime(4),
                                Gender = reader.GetString(5),
                                CountryId = reader.GetInt32(6),
                                WeightCategoryId = reader.GetInt32(7)
                            };
                            athletes.Add(athlete);
                        }
                    }
                }
            }

            return athletes;
        }
    }
}