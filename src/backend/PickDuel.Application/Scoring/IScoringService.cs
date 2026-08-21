using PickDuel.Domain.Entities;

namespace PickDuel.Application.Scoring;

public interface IScoringService
{
    /// <summary>
    /// Evaluates a completed pick and applies all generated scoring results.
    /// </summary>
    /// <param name="pick">
    /// Pick being evaluated.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation token.
    /// </param>
    /// <returns>
    /// Generated score events created from the evaluation.
    /// </returns>
    Task<IReadOnlyCollection<ScoreEvent>> EvaluatePickAsync(Pick pick, CancellationToken cancellationToken = default);


    /// <summary>
    /// Evaluates all unscored picks for a completed game.
    /// </summary>
    /// <param name="game">
    /// Completed game to evaluate.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation token.
    /// </param>
    Task EvaluateGameAsync(Game game, CancellationToken cancellationToken = default);
}