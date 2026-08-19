using PickDuel.Domain.Entities;
using PickDuel.Infrastructure.Repositories.Interfaces;

namespace PickDuel.Application.Picks;

/// <summary>
/// Provides application workflows for creating, retrieving,
/// updating, and deleting prediction picks.
/// </summary>
public class PickService : IPickService
{
    private readonly IPickRepository _pickRepository;


    /// <summary>
    /// Initializes a new PickService with required dependencies.
    /// </summary>
    /// <param name="pickRepository">Repository used for pick persistence.</param>
    public PickService(IPickRepository pickRepository)
    {
        ArgumentNullException.ThrowIfNull(pickRepository);

        _pickRepository = pickRepository;
    }


    /// <summary>
    /// Creates a new prediction pick for a user.
    /// </summary>
    /// <param name="pick">Pick to create.</param>
    /// <returns>The created pick.</returns>
    public async Task<Pick> CreatePickAsync(Pick pick)
    {
        ArgumentNullException.ThrowIfNull(pick);

        await _pickRepository.AddAsync(pick);

        await _pickRepository.SaveChangesAsync();

        return pick;
    }


    /// <summary>
    /// Retrieves a user's pick for a specific game.
    /// </summary>
    /// <param name="userId">User identifier.</param>
    /// <param name="gameId">Game identifier.</param>
    /// <returns>The user's pick if found.</returns>
    public async Task<Pick?> GetUserPickForGameAsync(Guid userId, Guid gameId)
    {
        ValidateId(userId, nameof(userId));
        ValidateId(gameId, nameof(gameId));

        return await _pickRepository.GetUserPickForGameAsync(
            userId,
            gameId
        );
    }


    /// <summary>
    /// Retrieves all picks made by a user.
    /// </summary>
    /// <param name="userId">User identifier.</param>
    /// <returns>Collection of user picks.</returns>
    public async Task<IReadOnlyCollection<Pick>> GetUserPicksAsync(Guid userId)
    {
        ValidateId(userId, nameof(userId));

        return await _pickRepository.GetByUserIdAsync(userId);
    }


    /// <summary>
    /// Updates an existing pick before the associated game begins.
    /// </summary>
    /// <param name="pick">Updated pick.</param>
    /// <returns>The updated pick.</returns>
    public async Task<Pick> UpdatePickAsync(Pick pick)
    {
        ArgumentNullException.ThrowIfNull(pick);

        await _pickRepository.UpdateAsync(pick);

        await _pickRepository.SaveChangesAsync();

        return pick;
    }


    /// <summary>
    /// Deletes a user's pick.
    /// </summary>
    /// <param name="pickId">Pick identifier.</param>
    public async Task DeletePickAsync(Guid pickId)
    {
        ValidateId(pickId, nameof(pickId));

        var pick =
            await _pickRepository.GetByIdAsync(pickId);

        if (pick == null)
        {
            throw new KeyNotFoundException(
                "Pick was not found."
            );
        }

        await _pickRepository.DeleteAsync(pick);

        await _pickRepository.SaveChangesAsync();
    }


    private static void ValidateId(
        Guid id,
        string parameterName)
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