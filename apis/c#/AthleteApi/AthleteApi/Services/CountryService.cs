using System.Data;
using Microsoft.Data.SqlClient;
using AthleteApi.Models;


namespace AthleteApi.Services
{
    public class CountryService : ICountryService
    {
        private readonly string _connectionString;

        public CountryService(IConfiguration configuration)
        {
            _connectionString = configuration?.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<IEnumerable<Country>> GetCountries(
            int pageNumber, int pageSize, string? name = null)
        {
            var countries = new List<Country>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_getCountries", conn))
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
                            var country = new Country
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Code = reader.GetString(2)
                            };
                            countries.Add(country);
                        }
                    }
                }
            }

            return countries;
        }
    }
}