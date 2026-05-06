using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LeagueApi.Data;
using LeagueApi.DTOs;
using LeagueApi.Models;

namespace LeagueApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayersController : ControllerBase
    {
        private readonly LeagueDbContext _context;

        public PlayersController(LeagueDbContext context)
        {
            _context = context;
        }

        // GET: api/players
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlayerDTO>>> GetPlayers(bool all = false)
        {
            var query = _context.Players.AsQueryable();

            if (!all)
                query = query.Where(p => p.IsActive);
            
            var players = await query
                .Select(p => new PlayerDTO
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Email = p.Email,
                    PhoneNumber = p.PhoneNumber,
                    DateJoined = p.DateJoined,
                    FargoRate = p.FargoRate,
                    IsActive = p.IsActive
                })
                .ToListAsync();

            return Ok(players);
        }
        // GET: api/players/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<PlayerDTO>> GetPlayer(int id)
        {
            var player = await _context.Players.FindAsync(id);

            if (player == null)
                return NotFound($"Player with ID {id} not found.");

            return Ok(new PlayerDTO
            {
                Id = player.Id,
                FirstName = player.FirstName,
                LastName = player.LastName,
                Email = player.Email,
                PhoneNumber = player.PhoneNumber,
                DateJoined = player.DateJoined,
                FargoRate = player.FargoRate,
                IsActive= player.IsActive
            });
        }

        // POST: api/players
        [HttpPost]
        public async Task<ActionResult<PlayerDTO>> CreatePlayer(CreatePlayerDTO dto)
        {
            // Check for duplicate email
            var exists = await _context.Players
                .AnyAsync(p => p.Email == dto.Email.ToLower());

            if (exists)
                return Conflict($"A player with email {dto.Email} already exists.");

            var player = new Player
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email.ToLower(),
                PhoneNumber = dto.PhoneNumber,
                DateJoined = DateTime.UtcNow,
                FargoRate = dto.FargoRate
            };

            _context.Players.Add(player);
            await _context.SaveChangesAsync();

            var result = new PlayerDTO
            {
                Id = player.Id,
                FirstName = player.FirstName,
                LastName = player.LastName,
                Email = player.Email,
                PhoneNumber = player.PhoneNumber,
                DateJoined = player.DateJoined,
                FargoRate = player.FargoRate
            };

            return CreatedAtAction(nameof(GetPlayer), new { id = player.Id }, result);
        }

        // PUT: api/players/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<PlayerDTO>> UpdatePlayer(int id, UpdatePlayerDTO dto)
        {
            var player = await _context.Players.FindAsync(id);

            if (player == null)
                return NotFound($"Player with ID {id} not found.");

            // Only update fields that were provided
            if (dto.FirstName != null)
                player.FirstName = dto.FirstName;

            if (dto.LastName != null)
                player.LastName = dto.LastName;

            if (dto.Email != null)
            {
                // Check for duplicate email if email is being changed
                var exists = await _context.Players
                    .AnyAsync(p => p.Email == dto.Email.ToLower() && p.Id != id);

                if (exists)
                    return Conflict($"A player with email {dto.Email} already exists.");

                player.Email = dto.Email.ToLower();
            }

            if (dto.PhoneNumber != null)
                player.PhoneNumber = dto.PhoneNumber;

            if (dto.FargoRate != null)
                player.FargoRate = dto.FargoRate;

            if (dto.IsActive != null)
                player.IsActive = dto.IsActive.Value;

            await _context.SaveChangesAsync();

            return Ok(new PlayerDTO
            {
                Id = player.Id,
                FirstName = player.FirstName,
                LastName = player.LastName,
                Email = player.Email,
                PhoneNumber = player.PhoneNumber,
                DateJoined = player.DateJoined,
                FargoRate = player.FargoRate,
                IsActive = player.IsActive
            });
        }

        // DELETE: api/players/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePlayer(int id)
        {
            var player = await _context.Players.FindAsync(id);

            if (player == null)
                return NotFound($"Player with ID {id} not found.");

            player.IsActive = false;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}