using System.Collections.Generic;
using System.Threading.Tasks;
using AthleteApi.Models;

namespace AthleteApi.Services
{
    public interface IWeightCategoryService
    {
        Task<IEnumerable<WeightCategory>> GetWeightCategories(int pageNumber, int pageSize);
    }
}