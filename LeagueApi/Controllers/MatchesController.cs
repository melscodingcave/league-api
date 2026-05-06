using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LeagueApi.Data;
using LeagueApi.DTOs;
using LeagueApi.Models;

namespace LeagueApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatchesController : ControllerBase
    {
        private readonly LeagueDbContext _context;

        public MatchesController(LeagueDbContext context)
        {
            _context = context;
        }

        // GET: api/matches
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MatchDTO>>> GetMatches()
        {
            var matches = await _context.Matches
                .Include(m => m.PlayerOne)
                .Include(m => m.PlayerTwo)
                .Include(m => m.Winner)
                .Select(m => new MatchDTO
                {
                    Id = m.Id,
                    PlayerOneName = m.PlayerOne!.FirstName + " " + m.PlayerOne.LastName,
                    PlayerTwoName = m.PlayerTwo!.FirstName + " " + m.PlayerTwo.LastName,
                    WinnerName = m.Winner != null
                        ? m.Winner.FirstName + " " + m.Winner.LastName
                        : null,
                    PlayerOneScore = m.PlayerOneScore,
                    PlayerTwoScore = m.PlayerTwoScore,
                    PlayerOneRace = m.PlayerOneRace,
                    PlayerTwoRace = m.PlayerTwoRace,
                    GameType = m.GameType.ToString(),
                    DatePlayed = m.DatePlayed
                })
                .ToListAsync();

            return Ok(matches);
        }

        // GET: api/matches/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<MatchDTO>> GetMatch(int id)
        {
            var match = await _context.Matches
                .Include(m => m.PlayerOne)
                .Include(m => m.PlayerTwo)
                .Include(m => m.Winner)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (match == null)
                return NotFound($"Match with ID {id} not found.");

            return Ok(new MatchDTO
            {
                Id = match.Id,
                PlayerOneName = match.PlayerOne!.FirstName + " " + match.PlayerOne.LastName,
                PlayerTwoName = match.PlayerTwo!.FirstName + " " + match.PlayerTwo.LastName,
                WinnerName = match.Winner != null
                    ? match.Winner.FirstName + " " + match.Winner.LastName
                    : null,
                PlayerOneScore = match.PlayerOneScore,
                PlayerTwoScore = match.PlayerTwoScore,
                PlayerOneRace = match.PlayerOneRace,
                PlayerTwoRace = match.PlayerTwoRace,
                GameType = match.GameType.ToString(),
                DatePlayed = match.DatePlayed
            });
        }

        // POST: api/matches
        [HttpPost]
        public async Task<ActionResult<MatchDTO>> CreateMatch(CreateMatchDTO dto)
        {
            // Players can't play themselves
            if (dto.PlayerOneId == dto.PlayerTwoId)
                return BadRequest("A player cannot play against themselves.");

            // Both players must exist and be active
            var playerOne = await _context.Players.FindAsync(dto.PlayerOneId);
            if (playerOne == null || !playerOne.IsActive)
                return BadRequest($"Player One with ID {dto.PlayerOneId} not found or inactive.");

            var playerTwo = await _context.Players.FindAsync(dto.PlayerTwoId);
            if (playerTwo == null || !playerTwo.IsActive)
                return BadRequest($"Player Two with ID {dto.PlayerTwoId} not found or inactive.");

            // WinnerId must be one of the two players
            if (dto.WinnerId != dto.PlayerOneId && dto.WinnerId != dto.PlayerTwoId)
                return BadRequest("Winner must be one of the two players in the match.");

            // Forfeit validation
            if (dto.ForfeitingPlayerId.HasValue)
            {
                if (dto.ForfeitingPlayerId != dto.PlayerOneId &&
                    dto.ForfeitingPlayerId != dto.PlayerTwoId)
                    return BadRequest("Forfeiting player must be one of the two players.");

                bool playerOneForfeits = dto.ForfeitingPlayerId == dto.PlayerOneId;

                if (playerOneForfeits)
                {
                    if (dto.PlayerOneScore != 0)
                        return BadRequest("Forfeiting player's score must be 0.");
                    if (dto.WinnerId != dto.PlayerTwoId)
                        return BadRequest("Winner must be the non-forfeiting player.");

                    // Auto-set winner's score to their race
                    dto.PlayerTwoScore = dto.PlayerTwoRace;
                }
                else
                {
                    if (dto.PlayerTwoScore != 0)
                        return BadRequest("Forfeiting player's score must be 0.");
                    if (dto.WinnerId != dto.PlayerOneId)
                        return BadRequest("Winner must be the non-forfeiting player.");

                    // Auto-set winner's score to their race
                    dto.PlayerOneScore = dto.PlayerOneRace;
                }
            }
            else
            {
                // Normal match validation
                if (dto.WinnerId == dto.PlayerOneId)
                {
                    if (dto.PlayerOneScore != dto.PlayerOneRace)
                        return BadRequest("Winner's score must equal their race.");
                    if (dto.PlayerTwoScore >= dto.PlayerTwoRace)
                        return BadRequest("Loser's score must be less than their race.");
                }
                else
                {
                    if (dto.PlayerTwoScore != dto.PlayerTwoRace)
                        return BadRequest("Winner's score must equal their race.");
                    if (dto.PlayerOneScore >= dto.PlayerOneRace)
                        return BadRequest("Loser's score must be less than their race.");
                }
            }

            var match = new Match
            {
                PlayerOneId = dto.PlayerOneId,
                PlayerTwoId = dto.PlayerTwoId,
                WinnerId = dto.WinnerId,
                PlayerOneScore = dto.PlayerOneScore,
                PlayerTwoScore = dto.PlayerTwoScore,
                PlayerOneRace = dto.PlayerOneRace,
                PlayerTwoRace = dto.PlayerTwoRace,
                GameType = dto.GameType,
                DatePlayed = DateTime.UtcNow
            };

            _context.Matches.Add(match);
            await _context.SaveChangesAsync();

            // Reload with navigation properties for response
            await _context.Entry(match).Reference(m => m.PlayerOne).LoadAsync();
            await _context.Entry(match).Reference(m => m.PlayerTwo).LoadAsync();
            await _context.Entry(match).Reference(m => m.Winner).LoadAsync();

            return CreatedAtAction(nameof(GetMatch), new { id = match.Id }, new MatchDTO
            {
                Id = match.Id,
                PlayerOneName = match.PlayerOne!.FirstName + " " + match.PlayerOne.LastName,
                PlayerTwoName = match.PlayerTwo!.FirstName + " " + match.PlayerTwo.LastName,
                WinnerName = match.Winner != null
                    ? match.Winner.FirstName + " " + match.Winner.LastName
                    : null,
                PlayerOneScore = match.PlayerOneScore,
                PlayerTwoScore = match.PlayerTwoScore,
                PlayerOneRace = match.PlayerOneRace,
                PlayerTwoRace = match.PlayerTwoRace,
                GameType = match.GameType.ToString(),
                DatePlayed = match.DatePlayed
            });
        }
    }
}