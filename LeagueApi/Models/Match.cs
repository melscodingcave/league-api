using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LeagueApi.Models
{
    public enum GameType
    {
        EightBall,
        NineBall,
        TenBall,
        OnePocket
    }

    public class Match
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PlayerOneId { get; set; }

        [Required]
        public int PlayerTwoId { get; set; }

        public int? WinnerId { get; set; }

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

        public DateTime DatePlayed { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [ForeignKey("PlayerOneId")]
        public Player? PlayerOne { get; set; }

        [ForeignKey("PlayerTwoId")]
        public Player? PlayerTwo { get; set; }

        [ForeignKey("WinnerId")]
        public Player? Winner { get; set; }
    }
}