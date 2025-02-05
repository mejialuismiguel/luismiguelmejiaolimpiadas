namespace AthleteApi.Models
{
    public class TournamentParticipation
    {
        public int Id { get; set; }
        public int AthleteId { get; set; }
        public int TournamentId { get; set; }
        public int WeightCategoryId { get; set; }
    }
}