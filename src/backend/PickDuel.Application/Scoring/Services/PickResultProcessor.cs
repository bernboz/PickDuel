using PickDuel.Domain.Entities;

namespace PickDuel.Application.Scoring;

public class PickResultProcessor : IPickResultProcessor
{
    private readonly IPickScoringService _pickScoringService;

    private readonly IScoreEventFactory _scoreEventFactory;


    /// <summary>
    /// Initializes a new PickResultProcessor with scoring dependencies.
    /// </summary>
    /// <param name="pickScoringService">Service used to calculate prediction points.</param>
    /// <param name="scoreEventFactory">Factory used to create score events.</param>
    public PickResultProcessor(IPickScoringService pickScoringService, IScoreEventFactory scoreEventFactory)
    {
        ArgumentNullException.ThrowIfNull(pickScoringService);
        ArgumentNullException.ThrowIfNull(scoreEventFactory);

        _pickScoringService = pickScoringService;
        _scoreEventFactory = scoreEventFactory;
    }


    /// <summary>
    /// Processes a completed pick evaluation and creates a score event.
    /// </summary>
    /// <param name="context">Context containing the pick, result, and odds information.</param>
    /// <returns>A score event representing the prediction outcome.</returns>
    public ScoreEvent ProcessPickResult(PickEvaluationContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var points =
            _pickScoringService.CalculateTotalPoints(context);

        return _scoreEventFactory.Create(
            context,
            points
        );
    }


    /// <summary>
    /// Applies a completed score event to a league standing.
    /// Updates points and prediction statistics.
    /// </summary>
    /// <param name="scoreEvent">Score event containing the scoring outcome.</param>
    /// <param name="standing">League standing to update.</param>
    public void ApplyScoreEvent(ScoreEvent scoreEvent, LeagueStanding standing)
    {
        ArgumentNullException.ThrowIfNull(scoreEvent);
        ArgumentNullException.ThrowIfNull(standing);

        standing.AddPoints(scoreEvent.Points);
    }
}