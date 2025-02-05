namespace AthleteApi.Models
{
    public class CompetitionResult
    {
        public required string Pais { get; set; }
        public required string Nombre { get; set; }
        public double Arranque { get; set; }
        public double Envion { get; set; }
        public double TotalPeso { get; set; }
    }
}