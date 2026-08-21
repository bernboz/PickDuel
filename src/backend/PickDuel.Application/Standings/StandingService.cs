using PickDuel.Application.Repositories.Interfaces;
using PickDuel.Domain.Entities.Standings;

namespace PickDuel.Application.Standings;

public class StandingService : IStandingService
{
    private readonly IStandingRepository _standingRepository;


    public StandingService(IStandingRepository standingRepository)
    {
        ArgumentNullException.ThrowIfNull(standingRepository);

        _standingRepository = standingRepository;
    }


    /// <summary>
    /// Creates a new league standing.
    /// </summary>
    /// <param name="standing">Standing entity to create.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Created standing.</returns>
    public async Task<LeagueStanding> CreateStandingAsync(LeagueStanding standing, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(standing);

        await _standingRepository.AddAsync(standing, cancellationToken);

        await _standingRepository.SaveChangesAsync(cancellationToken);

        return standing;
    }


    /// <summary>
    /// Retrieves a standing by identifier.
    /// </summary>
    /// <param name="standingId">Standing identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Standing if found; otherwise null.</returns>
    public async Task<LeagueStanding?> GetStandingAsync(Guid standingId, CancellationToken cancellationToken = default)
    {
        ValidateId(standingId, nameof(standingId));

        return await _standingRepository.GetByIdAsync(standingId, cancellationToken);
    }


    /// <summary>
    /// Retrieves all standings for a league.
    /// </summary>
    /// <param name="leagueId">League identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>League standings.</returns>
    public async Task<IReadOnlyCollection<LeagueStanding>> GetLeagueStandingsAsync(Guid leagueId, CancellationToken cancellationToken = default)
    {
        ValidateId(leagueId, nameof(leagueId));

        return await _standingRepository.GetByLeagueIdAsync(leagueId, cancellationToken);
    }


    /// <summary>
    /// Retrieves a user's standing within a league.
    /// </summary>
    /// <param name="leagueId">League identifier.</param>
    /// <param name="userId">User identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>User standing if found; otherwise null.</returns>
    public async Task<LeagueStanding?> GetUserStandingAsync(Guid leagueId, Guid userId, CancellationToken cancellationToken = default)
    {
        ValidateId(leagueId, nameof(leagueId));
        ValidateId(userId, nameof(userId));

        return await _standingRepository.GetByLeagueAndUserAsync(leagueId, userId, cancellationToken);
    }


    /// <summary>
    /// Updates an existing standing.
    /// </summary>
    /// <param name="standing">Updated standing entity.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Updated standing.</returns>
    public async Task<LeagueStanding> UpdateStandingAsync(LeagueStanding standing, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(standing);

        await _standingRepository.UpdateAsync(standing, cancellationToken);

        await _standingRepository.SaveChangesAsync(cancellationToken);

        return standing;
    }


    /// <summary>
    /// Deletes a standing.
    /// </summary>
    /// <param name="standingId">Standing identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public async Task DeleteStandingAsync(Guid standingId, CancellationToken cancellationToken = default)
    {
        ValidateId(standingId, nameof(standingId));

        var standing = await _standingRepository.GetByIdAsync(standingId, cancellationToken);

        if (standing == null)
        {
            throw new KeyNotFoundException("Standing was not found.");
        }

        await _standingRepository.DeleteAsync(standing, cancellationToken);

        await _standingRepository.SaveChangesAsync(cancellationToken);
    }


    private static void ValidateId(Guid id, string parameterName)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Identifier cannot be empty.", parameterName);
        }
    }
}