using Microsoft.EntityFrameworkCore;
using PickDuel.Domain.Entities;
using PickDuel.Domain.Entities.Standings;

namespace PickDuel.Infrastructure.Data;

public class PickDuelDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();

    public DbSet<League> Leagues => Set<League>();

    public DbSet<Game> Games => Set<Game>();

    public DbSet<Pick> Picks => Set<Pick>();

    public DbSet<LeagueStanding> LeagueStandings => Set<LeagueStanding>();

    public DbSet<ScoreEvent> ScoreEvents => Set<ScoreEvent>();


    /// <summary>
    /// Initializes a new database context for PickDuel persistence.
    /// </summary>
    /// <param name="options">Database configuration options.</param>
    public PickDuelDbContext(DbContextOptions<PickDuelDbContext> options) : base(options)
    {
    }


    /// <summary>
    /// Configures entity relationships and database mappings.
    /// </summary>
    /// <param name="modelBuilder">Builder used to configure entities.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PickDuelDbContext).Assembly);
    }
}