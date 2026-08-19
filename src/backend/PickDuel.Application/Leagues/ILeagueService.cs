using PickDuel.Domain.Entities;

namespace PickDuel.Application.Leagues;

public interface ILeagueService
{
    /// <summary>
    /// Creates a new league.
    /// </summary>
    /// <param name="league">League to create.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The created league.</returns>
    Task<League> CreateLeagueAsync(League league, CancellationToken cancellationToken = default);


    /// <summary>
    /// Retrieves a league by identifier.
    /// </summary>
    /// <param name="leagueId">League identifier.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The league if found.</returns>
    Task<League?> GetLeagueAsync(Guid leagueId, CancellationToken cancellationToken = default);


    /// <summary>
    /// Retrieves all leagues belonging to a user.
    /// </summary>
    /// <param name="userId">User identifier.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>User leagues.</returns>
    Task<IReadOnlyCollection<League>> GetUserLeaguesAsync(Guid userId, CancellationToken cancellationToken = default);


    /// <summary>
    /// Adds a member to an existing league.
    /// </summary>
    /// <param name="leagueId">League identifier.</param>
    /// <param name="user">User joining the league.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    Task AddMemberAsync(Guid leagueId, User user, CancellationToken cancellationToken = default);


    /// <summary>
    /// Updates an existing league.
    /// </summary>
    /// <param name="league">Updated league.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The updated league.</returns>
    Task<League> UpdateLeagueAsync(League league, CancellationToken cancellationToken = default);


    /// <summary>
    /// Deletes a league.
    /// </summary>
    /// <param name="leagueId">League identifier.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    Task DeleteLeagueAsync(Guid leagueId, CancellationToken cancellationToken = default);
}