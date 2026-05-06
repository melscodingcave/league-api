namespace LeagueApi.DTOs
{
    public class StandingDTO
    {
        public int PlayerId { get; set; }
        public string PlayerName { get; set; } = string.Empty;
        public int Wins { get; set; }
        public int Losses { get; set; }
        public decimal WinPercentage { get; set; }
        public int CurrentStreak { get; set; }
        public string StreakDescription { get; set; } = string.Empty;
    }
}