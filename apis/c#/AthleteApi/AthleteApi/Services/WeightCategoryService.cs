using System.Data;
using Microsoft.Data.SqlClient;
using AthleteApi.Models;

namespace AthleteApi.Services
{
    public class WeightCategoryService : IWeightCategoryService
    {
        private readonly string _connectionString;

        public WeightCategoryService(IConfiguration configuration)
        {
            _connectionString = configuration?.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException(nameof(configuration), "Configuration cannot be null");
        }

        public async Task<IEnumerable<WeightCategory>> GetWeightCategories(int pageNumber, int pageSize)
        {
            var weightCategories = new List<WeightCategory>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_getweightcategories", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
                    cmd.Parameters.AddWithValue("@PageSize", pageSize);

                    conn.Open();
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var weightCategory = new WeightCategory
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                MinWeight = reader.GetDouble(2),
                                MaxWeight = reader.GetDouble(3),
                                Gender = reader.GetString(4)
                            };
                            weightCategories.Add(weightCategory);
                        }
                    }
                }
            }

            return weightCategories;
        }
    }
}