using System.ComponentModel.DataAnnotations;
using LeagueApi.Models;

namespace LeagueApi.DTOs
{
    public class CreateMatchDTO
    {
        [Required]
        public int PlayerOneId { get; set; }

        [Required]
        public int PlayerTwoId { get; set; }

        [Required]
        public int WinnerId { get; set; }

        [Required]
        [Range(0, 20)]
        public int PlayerOneScore { get; set; }

        [Required]
        [Range(0, 20)]
        public int PlayerTwoScore { get; set; }

        [Required]
        [Range(1, 20)]
        public int PlayerOneRace { get; set; }

        [Required]
        [Range(1, 20)]
        public int PlayerTwoRace { get; set; }

        [Required]
        public GameType GameType { get; set; }

        public int? ForfeitingPlayerId { get; set; }
    }
}