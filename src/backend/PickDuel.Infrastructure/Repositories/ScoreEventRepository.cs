using Microsoft.EntityFrameworkCore;
using PickDuel.Application.Repositories.Interfaces;
using PickDuel.Domain.Entities;
using PickDuel.Infrastructure.Data;

namespace PickDuel.Infrastructure.Repositories;

public class ScoreEventRepository : IScoreEventRepository
{
    private readonly PickDuelDbContext _context;


    public ScoreEventRepository(PickDuelDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        _context = context;
    }


    public async Task AddAsync(ScoreEvent scoreEvent, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(scoreEvent);

        await _context.ScoreEvents.AddAsync(scoreEvent, cancellationToken);
    }


    public async Task<ScoreEvent?> GetByIdAsync(Guid scoreEventId, CancellationToken cancellationToken = default)
    {
        return await _context.ScoreEvents
            .Include(x => x.User)
            .Include(x => x.League)
            .FirstOrDefaultAsync(x => x.Id == scoreEventId, cancellationToken);
    }


    public async Task<IReadOnlyCollection<ScoreEvent>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.ScoreEvents
            .Include(x => x.League)
            .Where(x => x.User.Id == userId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
    }


    public async Task<IReadOnlyCollection<ScoreEvent>> GetByLeagueIdAsync(Guid leagueId, CancellationToken cancellationToken = default)
    {
        return await _context.ScoreEvents
            .Include(x => x.User)
            .Where(x => x.League.Id == leagueId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
    }


    public async Task<IReadOnlyCollection<ScoreEvent>> GetByPickIdAsync(Guid pickId, CancellationToken cancellationToken = default)
    {
        return await _context.ScoreEvents
            .Include(x => x.User)
            .Include(x => x.League)
            .Where(x => x.Pick != null && x.Pick.Id == pickId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
    }


    public Task DeleteAsync(ScoreEvent scoreEvent, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(scoreEvent);

        _context.ScoreEvents.Remove(scoreEvent);

        return Task.CompletedTask;
    }


    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}