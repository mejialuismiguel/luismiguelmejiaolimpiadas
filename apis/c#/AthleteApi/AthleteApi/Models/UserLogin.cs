namespace AthleteApi.Models
{
    public class UserLogin
    {
        public required string Username { get; set; } = string.Empty;
        public required string Password { get; set; } = string.Empty;
    }
}