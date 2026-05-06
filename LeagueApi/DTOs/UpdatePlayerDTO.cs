using System.ComponentModel.DataAnnotations;

namespace LeagueApi.DTOs
{
    public class UpdatePlayerDTO
    {
        [MaxLength(50)]
        public string? FirstName { get; set; }

        [MaxLength(50)]
        public string? LastName { get; set; }

        [EmailAddress]
        [MaxLength(100)]
        public string? Email { get; set; }

        [Phone]
        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        public int? FargoRate { get; set; }

        public bool? IsActive { get; set; }
    }
}