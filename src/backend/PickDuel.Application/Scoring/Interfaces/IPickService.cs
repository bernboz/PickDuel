using PickDuel.Domain.Entities;

namespace PickDuel.Application.Picks;

public interface IPickService
{
    /// <summary>
    /// Creates a new prediction pick for a user in a league.
    /// </summary>
    /// <param name="pick">Pick to create.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The created pick.</returns>
    Task<Pick> CreatePickAsync(Pick pick, CancellationToken cancellationToken = default);


    /// <summary>
    /// Retrieves a user's pick for a specific game.
    /// </summary>
    /// <param name="userId">User identifier.</param>
    /// <param name="gameId">Game identifier.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The user's pick if found.</returns>
    Task<Pick?> GetUserPickForGameAsync(Guid userId, Guid gameId, CancellationToken cancellationToken = default);


    /// <summary>
    /// Retrieves all picks made by a user.
    /// </summary>
    /// <param name="userId">User identifier.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>Collection of user picks.</returns>
    Task<IReadOnlyCollection<Pick>> GetUserPicksAsync(Guid userId, CancellationToken cancellationToken = default);


    /// <summary>
    /// Updates an existing pick before the associated game begins.
    /// </summary>
    /// <param name="pick">Updated pick.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The updated pick.</returns>
    Task<Pick> UpdatePickAsync(Pick pick, CancellationToken cancellationToken = default);


    /// <summary>
    /// Deletes a user's pick.
    /// </summary>
    /// <param name="pickId">Pick identifier.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    Task DeletePickAsync(Guid pickId, CancellationToken cancellationToken = default);
}