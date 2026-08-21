using PickDuel.Application.Repositories.Interfaces;
using PickDuel.Application.Scoring.Interfaces;
using PickDuel.Domain.Entities;

namespace PickDuel.Application.Scoring;

public class ScoringService : IScoringService
{
    private readonly IScoringCalculator _scoringCalculator;

    private readonly IScoreEventRepository _scoreEventRepository;


    public ScoringService(
        IScoringCalculator scoringCalculator,
        IScoreEventRepository scoreEventRepository)
    {
        ArgumentNullException.ThrowIfNull(scoringCalculator);
        ArgumentNullException.ThrowIfNull(scoreEventRepository);

        _scoringCalculator = scoringCalculator;
        _scoreEventRepository = scoreEventRepository;
    }


    /// <summary>
    /// Evaluates a completed pick and creates all generated score events.
    /// </summary>
    /// <param name="pick">
    /// Pick being evaluated.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation token.
    /// </param>
    /// <returns>
    /// Score events generated from evaluating the pick.
    /// </returns>
    public async Task<IReadOnlyCollection<ScoreEvent>> EvaluatePickAsync(Pick pick, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(pick);

        var settings = pick.League.ScoringSettings;

        var results = _scoringCalculator.Calculate(
            pick,
            settings
        );

        var events = results
            .Select(result =>
                new ScoreEvent(
                    pick.User,
                    pick.League,
                    result.Points,
                    result.Type,
                    result.Description,
                    pick
                ))
            .ToList();

        foreach (var scoreEvent in events)
        {
            await _scoreEventRepository.AddAsync(
                scoreEvent,
                cancellationToken
            );
        }

        pick.MarkAsScored();

        await _scoreEventRepository.SaveChangesAsync(
            cancellationToken
        );

        return events.AsReadOnly();
    }


    /// <summary>
    /// Evaluates all unscored picks for a completed game.
    /// </summary>
    /// <param name="game">
    /// Completed game to evaluate.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation token.
    /// </param>
    public async Task EvaluateGameAsync(Game game, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(game);

        if (!game.IsCompleted)
        {
            throw new InvalidOperationException(
                "Cannot evaluate a game that is not completed."
            );
        }

        throw new NotImplementedException(
            "Game pick retrieval will be implemented when PickRepository exposes game queries."
        );
    }
}