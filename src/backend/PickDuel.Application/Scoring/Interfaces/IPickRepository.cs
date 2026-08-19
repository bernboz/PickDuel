using PickDuel.Domain.Entities;

namespace PickDuel.Infrastructure.Repositories.Interfaces;

public interface IPickRepository
{
    /// <summary>
    /// Adds a new pick to the data store.
    /// </summary>
    /// <param name="pick">Pick entity to add.</param>
    Task AddAsync(Pick pick);


    /// <summary>
    /// Retrieves a pick by its identifier.
    /// </summary>
    /// <param name="pickId">Pick identifier.</param>
    /// <returns>The pick if found.</returns>
    Task<Pick?> GetByIdAsync(Guid pickId);


    /// <summary>
    /// Retrieves a user's pick for a specific game.
    /// </summary>
    /// <param name="userId">User identifier.</param>
    /// <param name="gameId">Game identifier.</param>
    /// <returns>The matching pick if found.</returns>
    Task<Pick?> GetUserPickForGameAsync(Guid userId, Guid gameId);


    /// <summary>
    /// Retrieves all picks created by a user.
    /// </summary>
    /// <param name="userId">User identifier.</param>
    /// <returns>User's picks.</returns>
    Task<IReadOnlyCollection<Pick>> GetByUserIdAsync(Guid userId);


    /// <summary>
    /// Updates an existing pick in the data store.
    /// </summary>
    /// <param name="pick">Pick entity to update.</param>
    Task UpdateAsync(Pick pick);


    /// <summary>
    /// Removes a pick from the data store.
    /// </summary>
    /// <param name="pick">Pick entity to remove.</param>
    Task DeleteAsync(Pick pick);


    /// <summary>
    /// Saves all pending repository changes.
    /// </summary>
    Task SaveChangesAsync();
}