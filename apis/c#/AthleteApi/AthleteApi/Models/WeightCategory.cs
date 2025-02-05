namespace AthleteApi.Models
{
    public class WeightCategory
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public double MinWeight { get; set; }
        public double MaxWeight { get; set; }
        public string Gender { get; set; } = string.Empty;
    }
}