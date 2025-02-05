using System;

namespace AthleteApi.Models
{
    public class Tournament
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Location { get; set; }
        public required DateTime StartDate { get; set; }
        public required DateTime EndDate { get; set; }
    }
}