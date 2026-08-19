using PickDuel.Domain.Entities;

namespace PickDuel.Application.Repositories.Interfaces;

/// <summary>
/// Defines persistence operations for game entities.
/// </summary>
public interface IGameRepository
{
    /// <summary>
    /// Adds a new game to the data store.
    /// </summary>
    /// <param name="game">
    /// Game entity to add.
    /// </param>
    /// <param name="cancellationToken">
    /// Token used to cancel the operation.
    /// </param>
    Task AddAsync(
        Game game,
        CancellationToken cancellationToken = default);


    /// <summary>
    /// Retrieves a game by its identifier.
    /// </summary>
    /// <param name="gameId">
    /// Game identifier.
    /// </param>
    /// <param name="cancellationToken">
    /// Token used to cancel the operation.
    /// </param>
    /// <returns>
    /// The game if found; otherwise null.
    /// </returns>
    Task<Game?> GetByIdAsync(
        Guid gameId,
        CancellationToken cancellationToken = default);


    /// <summary>
    /// Retrieves all games scheduled between two dates.
    /// </summary>
    /// <param name="startDate">
    /// Beginning of the date range.
    /// </param>
    /// <param name="endDate">
    /// End of the date range.
    /// </param>
    /// <param name="cancellationToken">
    /// Token used to cancel the operation.
    /// </param>
    /// <returns>
    /// Games scheduled within the date range.
    /// </returns>
    Task<IReadOnlyCollection<Game>> GetByDateRangeAsync(
        DateTime startDate,
        DateTime endDate,
        CancellationToken cancellationToken = default);


    /// <summary>
    /// Updates an existing game in the data store.
    /// </summary>
    /// <param name="game">
    /// Game entity to update.
    /// </param>
    /// <param name="cancellationToken">
    /// Token used to cancel the operation.
    /// </param>
    Task UpdateAsync(
        Game game,
        CancellationToken cancellationToken = default);


    /// <summary>
    /// Removes a game from the data store.
    /// </summary>
    /// <param name="game">
    /// Game entity to remove.
    /// </param>
    /// <param name="cancellationToken">
    /// Token used to cancel the operation.
    /// </param>
    Task DeleteAsync(
        Game game,
        CancellationToken cancellationToken = default);


    /// <summary>
    /// Saves all pending repository changes.
    /// </summary>
    /// <param name="cancellationToken">
    /// Token used to cancel the operation.
    /// </param>
    Task SaveChangesAsync(
        CancellationToken cancellationToken = default);
}