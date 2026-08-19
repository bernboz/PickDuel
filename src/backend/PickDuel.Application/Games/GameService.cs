using PickDuel.Application.Repositories.Interfaces;
using PickDuel.Domain.Entities;

namespace PickDuel.Application.Games;

/// <summary>
/// Provides application workflows for creating,
/// retrieving, updating, and deleting games.
/// </summary>
public class GameService : IGameService
{
    private readonly IGameRepository _gameRepository;


    /// <summary>
    /// Initializes a new GameService.
    /// </summary>
    /// <param name="gameRepository">
    /// Repository used for game persistence.
    /// </param>
    public GameService(IGameRepository gameRepository)
    {
        ArgumentNullException.ThrowIfNull(gameRepository);

        _gameRepository = gameRepository;
    }


    /// <summary>
    /// Creates a new game.
    /// </summary>
    /// <param name="game">
    /// Game to create.
    /// </param>
    /// <param name="cancellationToken">
    /// Token used to cancel the operation.
    /// </param>
    /// <returns>
    /// The created game.
    /// </returns>
    public async Task<Game> CreateGameAsync(Game game, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(game);

        await _gameRepository.AddAsync(game, cancellationToken);

        await _gameRepository.SaveChangesAsync(cancellationToken);

        return game;
    }


    /// <summary>
    /// Retrieves a game by identifier.
    /// </summary>
    /// <param name="gameId">
    /// Game identifier.
    /// </param>
    /// <param name="cancellationToken">
    /// Token used to cancel the operation.
    /// </param>
    /// <returns>
    /// The game if found; otherwise null.
    /// </returns>
    public async Task<Game?> GetGameAsync(Guid gameId, CancellationToken cancellationToken = default)
    {
        ValidateId(gameId, nameof(gameId));

        return await _gameRepository.GetByIdAsync(
            gameId,
            cancellationToken);
    }


    /// <summary>
    /// Retrieves games scheduled within a date range.
    /// </summary>
    /// <param name="startDate">
    /// Beginning of the date range.
    /// </param>
    /// <param name="endDate">
    /// End of the date range.
    /// </param>
    /// <param name="cancellationToken">
    /// Token used to cancel the operation.
    /// </param>
    /// <returns>
    /// Games scheduled within the date range.
    /// </returns>
    public async Task<IReadOnlyCollection<Game>> GetGamesByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        if (startDate >= endDate)
        {
            throw new ArgumentException(
                "Start date must be before end date.");
        }

        return await _gameRepository.GetByDateRangeAsync(
            startDate,
            endDate,
            cancellationToken);
    }


    /// <summary>
    /// Updates an existing game.
    /// </summary>
    /// <param name="game">
    /// Updated game.
    /// </param>
    /// <param name="cancellationToken">
    /// Token used to cancel the operation.
    /// </param>
    /// <returns>
    /// The updated game.
    /// </returns>
    public async Task<Game> UpdateGameAsync(Game game, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(game);

        await _gameRepository.UpdateAsync(game, cancellationToken);

        await _gameRepository.SaveChangesAsync(cancellationToken);

        return game;
    }


    /// <summary>
    /// Deletes an existing game.
    /// </summary>
    /// <param name="gameId">
    /// Game identifier.
    /// </param>
    /// <param name="cancellationToken">
    /// Token used to cancel the operation.
    /// </param>
    public async Task DeleteGameAsync(Guid gameId, CancellationToken cancellationToken = default)
    {
        ValidateId(gameId, nameof(gameId));

        var game = await _gameRepository.GetByIdAsync(
            gameId,
            cancellationToken);


        if (game == null)
        {
            throw new KeyNotFoundException(
                "Game was not found.");
        }


        await _gameRepository.DeleteAsync(
            game,
            cancellationToken);

        await _gameRepository.SaveChangesAsync(
            cancellationToken);
    }


    private static void ValidateId(Guid id, string parameterName)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException(
                "Identifier cannot be empty.",
                parameterName);
        }
    }
}