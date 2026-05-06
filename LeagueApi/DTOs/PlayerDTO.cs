namespace LeagueApi.DTOs
{
    public class PlayerDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public DateTime DateJoined { get; set; }
        public int? FargoRate { get; set; }
        public bool IsActive { get; set; }
    }
}