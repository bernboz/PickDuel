using Microsoft.EntityFrameworkCore;
using PickDuel.Application.Repositories.Interfaces;
using PickDuel.Domain.Entities.Standings;
using PickDuel.Infrastructure.Data;

namespace PickDuel.Infrastructure.Repositories;

public class StandingRepository : IStandingRepository
{
    private readonly PickDuelDbContext _context;


    public StandingRepository(PickDuelDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        _context = context;
    }


    public async Task AddAsync(LeagueStanding standing, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(standing);

        await _context.LeagueStandings.AddAsync(
            standing,
            cancellationToken);
    }


    public async Task<LeagueStanding?> GetByIdAsync(Guid standingId, CancellationToken cancellationToken = default)
    {
        return await _context.LeagueStandings
            .Include(x => x.User)
            .Include(x => x.League)
            .FirstOrDefaultAsync(
                x => x.Id == standingId,
                cancellationToken);
    }


    public async Task<IReadOnlyCollection<LeagueStanding>> GetByLeagueIdAsync(Guid leagueId, CancellationToken cancellationToken = default)
    {
        return await _context.LeagueStandings
            .Where(x => x.League.Id == leagueId)
            .Include(x => x.User)
            .Include(x => x.League)
            .OrderByDescending(x => x.TotalPoints)
            .ToListAsync(cancellationToken);
    }


    public async Task<LeagueStanding?> GetByLeagueAndUserAsync(Guid leagueId, Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.LeagueStandings
            .Include(x => x.User)
            .Include(x => x.League)
            .FirstOrDefaultAsync(
                x => x.League.Id == leagueId &&
                     x.User.Id == userId,
                cancellationToken);
    }


    public Task UpdateAsync(LeagueStanding standing, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(standing);

        _context.LeagueStandings.Update(standing);

        return Task.CompletedTask;
    }


    public Task DeleteAsync(LeagueStanding standing, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(standing);

        _context.LeagueStandings.Remove(standing);

        return Task.CompletedTask;
    }


    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}