using PickDuel.Domain.Entities;

namespace PickDuel.Application.Picks;

public interface IPickService
{
    /// <summary>
    /// Creates a new prediction pick for a user in a league.
    /// </summary>
    /// <param name="pick">Pick to create.</param>
    /// <returns>The created pick.</returns>
    Task<Pick> CreatePickAsync(Pick pick);


    /// <summary>
    /// Retrieves a user's pick for a specific game.
    /// </summary>
    /// <param name="userId">User identifier.</param>
    /// <param name="gameId">Game identifier.</param>
    /// <returns>The user's pick if found.</returns>
    Task<Pick?> GetUserPickForGameAsync(Guid userId, Guid gameId);


    /// <summary>
    /// Retrieves all picks made by a user.
    /// </summary>
    /// <param name="userId">User identifier.</param>
    /// <returns>Collection of user picks.</returns>
    Task<IReadOnlyCollection<Pick>> GetUserPicksAsync(Guid userId);


    /// <summary>
    /// Updates an existing pick before the associated game begins.
    /// </summary>
    /// <param name="pick">Updated pick.</param>
    /// <returns>The updated pick.</returns>
    Task<Pick> UpdatePickAsync(Pick pick);


    /// <summary>
    /// Deletes a user's pick.
    /// </summary>
    /// <param name="pickId">Pick identifier.</param>
    Task DeletePickAsync(Guid pickId);
}