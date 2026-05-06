namespace LeagueApi.DTOs
{
    public class MatchDTO
    {
        public int Id { get; set; }
        public string PlayerOneName { get; set; } = string.Empty;
        public string PlayerTwoName { get; set; } = string.Empty;
        public string? WinnerName { get; set; }
        public int PlayerOneScore { get; set; }
        public int PlayerTwoScore { get; set; }
        public int PlayerOneRace { get; set; }
        public int PlayerTwoRace { get; set; }
        public string GameType { get; set; } = string.Empty;
        public DateTime DatePlayed { get; set; }
    }
}