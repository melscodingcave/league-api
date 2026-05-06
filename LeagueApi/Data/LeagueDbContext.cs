using Microsoft.EntityFrameworkCore;
using LeagueApi.Models;

namespace LeagueApi.Data
{
    public class LeagueDbContext : DbContext
    {
        public LeagueDbContext(DbContextOptions<LeagueDbContext> options)
            : base(options) { }

        public DbSet<Player> Players { get; set; }
        public DbSet<Match> Matches { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Match>()
                .HasOne(m => m.PlayerOne)
                .WithMany()
                .HasForeignKey(m => m.PlayerOneId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Match>()
                .HasOne(m => m.PlayerTwo)
                .WithMany()
                .HasForeignKey(m => m.PlayerTwoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Match>()
                .HasOne(m => m.Winner)
                .WithMany()
                .HasForeignKey(m => m.WinnerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Player>()
                .HasIndex(p => p.Email)
                .IsUnique();
        }
    }
}