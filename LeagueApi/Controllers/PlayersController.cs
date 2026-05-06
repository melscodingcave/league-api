using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LeagueApi.Data;
using LeagueApi.DTOs;

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
        public async Task<ActionResult<IEnumerable<PlayerDTO>>> GetPlayers()
        {
            var players = await _context.Players
                .Select(p => new PlayerDTO
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Email = p.Email,
                    PhoneNumber = p.PhoneNumber,
                    DateJoined = p.DateJoined,
                    FargoRate = p.FargoRate
                })
                .ToListAsync();

            return Ok(players);
        }
    }
}