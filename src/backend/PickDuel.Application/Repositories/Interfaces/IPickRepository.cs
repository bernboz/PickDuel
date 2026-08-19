using PickDuel.Domain.Entities;

namespace PickDuel.Application.Repositories.Interfaces;

/// <summary>
/// Defines persistence operations for pick entities.
/// </summary>
public interface IPickRepository
{
    /// <summary>
    /// Adds a new pick to the data store.
    /// </summary>
    /// <param name="pick">Pick entity to add.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    Task AddAsync(Pick pick, CancellationToken cancellationToken = default);


    /// <summary>
    /// Retrieves a pick by its identifier.
    /// </summary>
    /// <param name="pickId">Pick identifier.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The pick if found.</returns>
    Task<Pick?> GetByIdAsync(Guid pickId, CancellationToken cancellationToken = default);


    /// <summary>
    /// Retrieves a user's pick for a specific game.
    /// </summary>
    /// <param name="userId">User identifier.</param>
    /// <param name="gameId">Game identifier.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The matching pick if found.</returns>
    Task<Pick?> GetUserPickForGameAsync(Guid userId, Guid gameId, CancellationToken cancellationToken = default);


    /// <summary>
    /// Retrieves all picks created by a user.
    /// </summary>
    /// <param name="userId">User identifier.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>User's picks.</returns>
    Task<IReadOnlyCollection<Pick>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);


    /// <summary>
    /// Updates an existing pick in the data store.
    /// </summary>
    /// <param name="pick">Pick entity to update.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    Task UpdateAsync(Pick pick, CancellationToken cancellationToken = default);


    /// <summary>
    /// Removes a pick from the data store.
    /// </summary>
    /// <param name="pick">Pick entity to remove.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    Task DeleteAsync(Pick pick, CancellationToken cancellationToken = default);


    /// <summary>
    /// Saves all pending repository changes.
    /// </summary>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}