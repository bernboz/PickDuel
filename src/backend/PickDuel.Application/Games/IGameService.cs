using PickDuel.Domain.Entities;

namespace PickDuel.Application.Games;

/// <summary>
/// Defines application operations for managing games.
/// </summary>
public interface IGameService
{
    /// <summary>
    /// Creates a new game.
    /// </summary>
    /// <param name="game">
    /// Game to create.
    /// </param>
    /// <param name="cancellationToken">
    /// Token used to cancel the operation.
    /// </param>
    /// <returns>
    /// The created game.
    /// </returns>
    Task<Game> CreateGameAsync(
        Game game,
        CancellationToken cancellationToken = default);


    /// <summary>
    /// Retrieves a game by identifier.
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
    Task<Game?> GetGameAsync(
        Guid gameId,
        CancellationToken cancellationToken = default);


    /// <summary>
    /// Retrieves games scheduled within a date range.
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
    Task<IReadOnlyCollection<Game>> GetGamesByDateRangeAsync(
        DateTime startDate,
        DateTime endDate,
        CancellationToken cancellationToken = default);


    /// <summary>
    /// Updates an existing game.
    /// </summary>
    /// <param name="game">
    /// Updated game.
    /// </param>
    /// <param name="cancellationToken">
    /// Token used to cancel the operation.
    /// </param>
    /// <returns>
    /// The updated game.
    /// </returns>
    Task<Game> UpdateGameAsync(
        Game game,
        CancellationToken cancellationToken = default);


    /// <summary>
    /// Deletes an existing game.
    /// </summary>
    /// <param name="gameId">
    /// Game identifier.
    /// </param>
    /// <param name="cancellationToken">
    /// Token used to cancel the operation.
    /// </param>
    Task DeleteGameAsync(
        Guid gameId,
        CancellationToken cancellationToken = default);
}