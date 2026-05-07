using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LeagueApi.Data;
using LeagueApi.DTOs;
using LeagueApi.Models;

namespace LeagueApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StandingsController : ControllerBase
    {
        private readonly LeagueDbContext _context;

        public StandingsController(LeagueDbContext context)
        {
            _context = context;
        }

        // GET: api/standings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StandingDTO>>> GetStandings()
        {
            var players = await _context.Players
                .Where(p => p.IsActive)
                .ToListAsync();

            var matches = await _context.Matches
                .ToListAsync();

            var standings = players.Select(player =>
            {
                var playerMatches = matches
                    .Where(m => m.PlayerOneId == player.Id ||
                                m.PlayerTwoId == player.Id)
                    .OrderByDescending(m => m.DatePlayed)
                    .ToList();

                var wins = playerMatches.Count(m => m.WinnerId == player.Id);
                var losses = playerMatches.Count(m => m.WinnerId != player.Id);
                var total = wins + losses;

                var winPercentage = total > 0
                    ? Math.Round((decimal)wins / total * 100, 2)
                    : 0;

                // Calculate current streak
                var streak = 0;
                var isWinStreak = true;

                if (playerMatches.Any())
                {
                    isWinStreak = playerMatches.First().WinnerId == player.Id;

                    foreach (var match in playerMatches)
                    {
                        bool matchWon = match.WinnerId == player.Id;

                        if (matchWon == isWinStreak)
                            streak++;
                        else
                            break;
                    }
                }

                var streakDescription = total == 0
                    ? "N/A"
                    : $"{(isWinStreak ? "W" : "L")}{streak}";

                return new StandingDTO
                {
                    PlayerId = player.Id,
                    PlayerName = player.FirstName + " " + player.LastName,
                    Wins = wins,
                    Losses = losses,
                    WinPercentage = winPercentage,
                    CurrentStreak = streak,
                    StreakDescription = streakDescription,
                    LastName = player.LastName,
                };
            })
            .OrderByDescending(s => s.WinPercentage)
            .ThenByDescending(s => s.Wins)
            .ThenBy(s => s.LastName)
            .ToList();

            return Ok(standings);
        }
    }
}