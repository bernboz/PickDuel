using PickDuel.Application.Repositories.Interfaces;
using PickDuel.Domain.Entities;

namespace PickDuel.Application.Picks;

/// <summary>
/// Provides application workflows for creating, retrieving,
/// updating, and deleting prediction picks.
/// </summary>
public class PickService : IPickService
{
    private readonly IPickRepository _pickRepository;


    /// <summary>
    /// Initializes a new instance of the <see cref="PickService"/> class.
    /// </summary>
    /// <param name="pickRepository">
    /// Repository used for pick persistence.
    /// </param>
    public PickService(IPickRepository pickRepository)
    {
        ArgumentNullException.ThrowIfNull(pickRepository);

        _pickRepository = pickRepository;
    }


    /// <summary>
    /// Creates a new prediction pick for a user in a league.
    /// </summary>
    /// <param name="pick">Pick to create.</param>
    /// <param name="cancellationToken">
    /// Token used to cancel the operation.
    /// </param>
    /// <returns>The created pick.</returns>
    public async Task<Pick> CreatePickAsync(Pick pick, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(pick);

        await _pickRepository.AddAsync(
            pick,
            cancellationToken
        );

        await _pickRepository.SaveChangesAsync(
            cancellationToken
        );

        return pick;
    }


    /// <summary>
    /// Retrieves a user's pick for a specific game.
    /// </summary>
    /// <param name="userId">User identifier.</param>
    /// <param name="gameId">Game identifier.</param>
    /// <param name="cancellationToken">
    /// Token used to cancel the operation.
    /// </param>
    /// <returns>The user's pick if found.</returns>
    public async Task<Pick?> GetUserPickForGameAsync(Guid userId, Guid gameId, CancellationToken cancellationToken = default)
    {
        ValidateId(
            userId,
            nameof(userId)
        );

        ValidateId(
            gameId,
            nameof(gameId)
        );

        return await _pickRepository.GetUserPickForGameAsync(
            userId,
            gameId,
            cancellationToken
        );
    }


    /// <summary>
    /// Retrieves all picks made by a user.
    /// </summary>
    /// <param name="userId">User identifier.</param>
    /// <param name="cancellationToken">
    /// Token used to cancel the operation.
    /// </param>
    /// <returns>Collection of user picks.</returns>
    public async Task<IReadOnlyCollection<Pick>> GetUserPicksAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        ValidateId(
            userId,
            nameof(userId)
        );

        return await _pickRepository.GetByUserIdAsync(
            userId,
            cancellationToken
        );
    }


    /// <summary>
    /// Updates an existing pick before the associated game begins.
    /// </summary>
    /// <param name="pick">Updated pick.</param>
    /// <param name="cancellationToken">
    /// Token used to cancel the operation.
    /// </param>
    /// <returns>The updated pick.</returns>
    public async Task<Pick> UpdatePickAsync(Pick pick, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(pick);

        await _pickRepository.UpdateAsync(
            pick,
            cancellationToken
        );

        await _pickRepository.SaveChangesAsync(
            cancellationToken
        );

        return pick;
    }


    /// <summary>
    /// Deletes a user's pick.
    /// </summary>
    /// <param name="pickId">Pick identifier.</param>
    /// <param name="cancellationToken">
    /// Token used to cancel the operation.
    /// </param>
    public async Task DeletePickAsync(Guid pickId, CancellationToken cancellationToken = default)
    {
        ValidateId(
            pickId,
            nameof(pickId)
        );

        var pick = await _pickRepository.GetByIdAsync(
            pickId,
            cancellationToken
        );

        if (pick == null)
        {
            throw new KeyNotFoundException(
                "Pick was not found."
            );
        }

        await _pickRepository.DeleteAsync(
            pick,
            cancellationToken
        );

        await _pickRepository.SaveChangesAsync(
            cancellationToken
        );
    }


    /// <summary>
    /// Validates that an identifier contains a valid value.
    /// </summary>
    /// <param name="id">Identifier to validate.</param>
    /// <param name="parameterName">Parameter name used for exceptions.</param>
    private static void ValidateId(Guid id, string parameterName)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException(
                "Identifier cannot be empty.",
                parameterName
            );
        }
    }
}