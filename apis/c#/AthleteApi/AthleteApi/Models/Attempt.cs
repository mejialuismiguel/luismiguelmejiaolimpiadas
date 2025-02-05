namespace AthleteApi.Models
{
    public class Attempt
    {
        public int Id { get; set; }
        public int ParticipationId { get; set; }
        public int AttemptNumber { get; set; }
        public string? Type { get; set; }
        public double WeightLifted { get; set; }
        public bool Success { get; set; }
        public string? TournamentName { get; set; } 
        public int TournamentId { get; set; }
    }
}