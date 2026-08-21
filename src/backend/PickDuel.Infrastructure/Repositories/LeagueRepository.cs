using Microsoft.EntityFrameworkCore;
using PickDuel.Application.Repositories.Interfaces;
using PickDuel.Domain.Entities;
using PickDuel.Infrastructure.Data;

namespace PickDuel.Infrastructure.Repositories;

public class LeagueRepository : ILeagueRepository
{
    private readonly PickDuelDbContext _context;


    public LeagueRepository(PickDuelDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        _context = context;
    }


    public async Task AddAsync(League league, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(league);

        await _context.Leagues.AddAsync(league, cancellationToken);
    }


    public async Task<League?> GetByIdAsync(Guid leagueId, CancellationToken cancellationToken = default)
    {
        return await _context.Leagues
            .Include(x => x.Owner)
            .Include(x => x.Members)
            .FirstOrDefaultAsync(x => x.Id == leagueId, cancellationToken);
    }


    public async Task<IReadOnlyCollection<League>> GetByOwnerIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Leagues
            .Where(x => x.Owner.Id == userId)
            .Include(x => x.Owner)
            .Include(x => x.Members)
            .ToListAsync(cancellationToken);
    }


    public async Task<IReadOnlyCollection<League>> GetByMemberIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Leagues
            .Where(x => x.Members.Any(member => member.Id == userId))
            .Include(x => x.Owner)
            .Include(x => x.Members)
            .ToListAsync(cancellationToken);
    }


    public Task UpdateAsync(League league, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(league);

        _context.Leagues.Update(league);

        return Task.CompletedTask;
    }


    public Task DeleteAsync(League league, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(league);

        _context.Leagues.Remove(league);

        return Task.CompletedTask;
    }


    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}