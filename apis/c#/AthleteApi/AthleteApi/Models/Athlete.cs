namespace AthleteApi.Models
{
    public class Athlete
    {
        public int Id { get; set; } = 0;
        public string Dni { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; } = DateTime.MinValue;
        public string Gender { get; set; } = string.Empty;
        public int CountryId { get; set; } = 0;
        public int WeightCategoryId { get; set; } = 0;
    }
}