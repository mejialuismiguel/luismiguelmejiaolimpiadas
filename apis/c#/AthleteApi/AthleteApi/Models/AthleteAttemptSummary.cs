namespace AthleteApi.Models
{
    public class AthleteAttemptSummary
    {
        public required string AthleteName { get; set; }
        public required string AthleteDni { get; set; }
        public required string TournamentName { get; set; }
        public int TournamentId { get; set; } 
        public int TotalAttempts { get; set; }
    }
}