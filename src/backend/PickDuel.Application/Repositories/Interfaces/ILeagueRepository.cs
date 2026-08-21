using PickDuel.Domain.Entities;

namespace PickDuel.Application.Repositories.Interfaces;

public interface ILeagueRepository
{
    /// <summary>
    /// Adds a new league to the data store.
    /// </summary>
    Task AddAsync(
        League league,
        CancellationToken cancellationToken = default);


    /// <summary>
    /// Retrieves a league by identifier.
    /// </summary>
    Task<League?> GetByIdAsync(
        Guid leagueId,
        CancellationToken cancellationToken = default);


    /// <summary>
    /// Retrieves leagues owned by a user.
    /// </summary>
    Task<IReadOnlyCollection<League>> GetByOwnerIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default);


    /// <summary>
    /// Retrieves leagues where the user is a member.
    /// </summary>
    Task<IReadOnlyCollection<League>> GetByMemberIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default);


    /// <summary>
    /// Updates an existing league.
    /// </summary>
    Task UpdateAsync(
        League league,
        CancellationToken cancellationToken = default);


    /// <summary>
    /// Deletes an existing league.
    /// </summary>
    Task DeleteAsync(
        League league,
        CancellationToken cancellationToken = default);


    /// <summary>
    /// Saves pending changes.
    /// </summary>
    Task SaveChangesAsync(
        CancellationToken cancellationToken = default);
}