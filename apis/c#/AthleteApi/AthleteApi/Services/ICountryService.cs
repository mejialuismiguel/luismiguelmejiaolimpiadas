using System.Collections.Generic;
using System.Threading.Tasks;
using AthleteApi.Models;


namespace AthleteApi.Services
{
    public interface ICountryService
    {
        Task<IEnumerable<Country>> GetCountries(int pageNumber, int pageSize, string? name = null);
    }
}