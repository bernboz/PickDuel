using PickDuel.Domain.Entities.Standings;

namespace PickDuel.Application.Repositories.Interfaces;

public interface IStandingRepository
{
    /// <summary>
    /// Adds a new league standing to the data store.
    /// </summary>
    /// <param name="standing">
    /// League standing entity to add.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation token.
    /// </param>
    Task AddAsync(
        LeagueStanding standing,
        CancellationToken cancellationToken = default);


    /// <summary>
    /// Retrieves a standing by its identifier.
    /// </summary>
    /// <param name="standingId">
    /// Standing identifier.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation token.
    /// </param>
    /// <returns>
    /// The standing if found; otherwise null.
    /// </returns>
    Task<LeagueStanding?> GetByIdAsync(
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
    /// League standings ordered by ranking.
    /// </returns>
    Task<IReadOnlyCollection<LeagueStanding>> GetByLeagueIdAsync(
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
    /// The user's standing if found; otherwise null.
    /// </returns>
    Task<LeagueStanding?> GetByLeagueAndUserAsync(
        Guid leagueId,
        Guid userId,
        CancellationToken cancellationToken = default);


    /// <summary>
    /// Updates an existing league standing.
    /// </summary>
    /// <param name="standing">
    /// Standing entity to update.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation token.
    /// </param>
    Task UpdateAsync(
        LeagueStanding standing,
        CancellationToken cancellationToken = default);


    /// <summary>
    /// Removes a league standing.
    /// </summary>
    /// <param name="standing">
    /// Standing entity to remove.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation token.
    /// </param>
    Task DeleteAsync(
        LeagueStanding standing,
        CancellationToken cancellationToken = default);


    /// <summary>
    /// Saves all pending repository changes.
    /// </summary>
    /// <param name="cancellationToken">
    /// Cancellation token.
    /// </param>
    Task SaveChangesAsync(
        CancellationToken cancellationToken = default);
}