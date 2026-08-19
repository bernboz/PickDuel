using Microsoft.EntityFrameworkCore;
using PickDuel.Application.Repositories.Interfaces;
using PickDuel.Domain.Entities;
using PickDuel.Infrastructure.Data;

namespace PickDuel.Infrastructure.Repositories;

/// <summary>
/// Provides database operations for Pick entities.
/// </summary>
public class PickRepository : IPickRepository
{
    private readonly PickDuelDbContext _context;


    /// <summary>
    /// Initializes a new instance of the <see cref="PickRepository"/> class.
    /// </summary>
    /// <param name="context">
    /// Database context used for persistence.
    /// </param>
    public PickRepository(PickDuelDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        _context = context;
    }


    /// <summary>
    /// Adds a new pick to the database.
    /// </summary>
    /// <param name="pick">
    /// Pick entity to add.
    /// </param>
    /// <param name="cancellationToken">
    /// Token used to cancel the operation.
    /// </param>
    public async Task AddAsync(Pick pick, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(pick);

        await _context.Picks.AddAsync(
            pick,
            cancellationToken
        );
    }


    /// <summary>
    /// Retrieves a pick by identifier.
    /// </summary>
    /// <param name="pickId">
    /// Pick identifier.
    /// </param>
    /// <param name="cancellationToken">
    /// Token used to cancel the operation.
    /// </param>
    /// <returns>
    /// Pick if found, otherwise null.
    /// </returns>
    public async Task<Pick?> GetByIdAsync(Guid pickId, CancellationToken cancellationToken = default)
    {
        return await _context.Picks
            .AsNoTracking()
            .Include(x => x.User)
            .Include(x => x.League)
            .Include(x => x.Game)
            .FirstOrDefaultAsync(
                x => x.Id == pickId,
                cancellationToken
            );
    }


    /// <summary>
    /// Retrieves a user's pick for a specific game.
    /// </summary>
    /// <param name="userId">
    /// User identifier.
    /// </param>
    /// <param name="gameId">
    /// Game identifier.
    /// </param>
    /// <param name="cancellationToken">
    /// Token used to cancel the operation.
    /// </param>
    /// <returns>
    /// Matching pick if found.
    /// </returns>
    public async Task<Pick?> GetUserPickForGameAsync(Guid userId, Guid gameId, CancellationToken cancellationToken = default)
    {
        return await _context.Picks
            .AsNoTracking()
            .Include(x => x.Game)
            .FirstOrDefaultAsync(
                x =>
                    x.User.Id == userId &&
                    x.Game.Id == gameId,
                cancellationToken
            );
    }


    /// <summary>
    /// Retrieves all picks created by a user.
    /// </summary>
    /// <param name="userId">
    /// User identifier.
    /// </param>
    /// <param name="cancellationToken">
    /// Token used to cancel the operation.
    /// </param>
    /// <returns>
    /// User's picks.
    /// </returns>
    public async Task<IReadOnlyCollection<Pick>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Picks
            .AsNoTracking()
            .Where(x =>
                x.User.Id == userId)
            .Include(x => x.Game)
            .Include(x => x.League)
            .ToListAsync(
                cancellationToken
            );
    }


    /// <summary>
    /// Updates an existing pick.
    /// </summary>
    /// <param name="pick">
    /// Pick entity to update.
    /// </param>
    /// <param name="cancellationToken">
    /// Token used to cancel the operation.
    /// </param>
    public Task UpdateAsync(Pick pick, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(pick);

        _context.Picks.Update(pick);

        return Task.CompletedTask;
    }


    /// <summary>
    /// Removes an existing pick from the database.
    /// </summary>
    /// <param name="pick">
    /// Pick entity to remove.
    /// </param>
    /// <param name="cancellationToken">
    /// Token used to cancel the operation.
    /// </param>
    public Task DeleteAsync(Pick pick, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(pick);

        _context.Picks.Remove(pick);

        return Task.CompletedTask;
    }


    /// <summary>
    /// Saves all pending database changes.
    /// </summary>
    /// <param name="cancellationToken">
    /// Token used to cancel the operation.
    /// </param>
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(
            cancellationToken
        );
    }
}