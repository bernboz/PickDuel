using PickDuel.Domain.Entities.Standings;

namespace PickDuel.Application.Standings;

public interface IStandingService
{
    /// <summary>
    /// Creates a new league standing.
    /// </summary>
    /// <param name="standing">
    /// Standing entity to create.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation token.
    /// </param>
    /// <returns>
    /// Created standing.
    /// </returns>
    Task<LeagueStanding> CreateStandingAsync(
        LeagueStanding standing,
        CancellationToken cancellationToken = default);


    /// <summary>
    /// Retrieves a standing by identifier.
    /// </summary>
    /// <param name="standingId">
    /// Standing identifier.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation token.
    /// </param>
    /// <returns>
    /// Standing if found; otherwise null.
    /// </returns>
    Task<LeagueStanding?> GetStandingAsync(
        Guid standingId,
        CancellationToken cancellationToken = default);


    /// <summary>
    /// Retrieves all standings for a league.
    /// </summary>
    /// <param name="leagueId">
    /// League identifier.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation token.
    /// </param>
    /// <returns>
    /// League standings.
    /// </returns>
    Task<IReadOnlyCollection<LeagueStanding>> GetLeagueStandingsAsync(
        Guid leagueId,
        CancellationToken cancellationToken = default);


    /// <summary>
    /// Retrieves a user's standing within a league.
    /// </summary>
    /// <param name="leagueId">
    /// League identifier.
    /// </param>
    /// <param name="userId">
    /// User identifier.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation token.
    /// </param>
    /// <returns>
    /// User standing if found; otherwise null.
    /// </returns>
    Task<LeagueStanding?> GetUserStandingAsync(
        Guid leagueId,
        Guid userId,
        CancellationToken cancellationToken = default);


    /// <summary>
    /// Updates an existing standing.
    /// </summary>
    /// <param name="standing">
    /// Updated standing entity.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation token.
    /// </param>
    /// <returns>
    /// Updated standing.
    /// </returns>
    Task<LeagueStanding> UpdateStandingAsync(
        LeagueStanding standing,
        CancellationToken cancellationToken = default);


    /// <summary>
    /// Deletes a standing.
    /// </summary>
    /// <param name="standingId">
    /// Standing identifier.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation token.
    /// </param>
    Task DeleteStandingAsync(
        Guid standingId,
        CancellationToken cancellationToken = default);
}