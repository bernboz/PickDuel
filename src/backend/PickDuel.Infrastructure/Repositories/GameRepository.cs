using Microsoft.EntityFrameworkCore;
using PickDuel.Application.Repositories.Interfaces;
using PickDuel.Domain.Entities;
using PickDuel.Infrastructure.Data;

namespace PickDuel.Infrastructure.Repositories;

public class GameRepository : IGameRepository
{
    private readonly PickDuelDbContext _context;


    public GameRepository(PickDuelDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        _context = context;
    }


    public async Task AddAsync(Game game, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(game);

        await _context.Games.AddAsync(
            game,
            cancellationToken);
    }


    public async Task<Game?> GetByIdAsync(Guid gameId, CancellationToken cancellationToken = default)
    {
        return await _context.Games
            .FirstOrDefaultAsync(
                x => x.Id == gameId,
                cancellationToken);
    }


    public async Task<IReadOnlyCollection<Game>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        return await _context.Games
            .Where(x => x.StartTime >= startDate && x.StartTime <= endDate)
            .ToListAsync(cancellationToken);
    }


    public Task UpdateAsync(Game game, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(game);

        _context.Games.Update(game);

        return Task.CompletedTask;
    }


    public Task DeleteAsync(Game game, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(game);

        _context.Games.Remove(game);

        return Task.CompletedTask;
    }


    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}