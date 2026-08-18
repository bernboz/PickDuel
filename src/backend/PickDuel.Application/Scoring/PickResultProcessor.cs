using PickDuel.Domain.Entities;
using PickDuel.Domain.Enums;

namespace PickDuel.Application.Scoring;

public class PickResultProcessor
{
    private readonly PickScoringService _pickScoringService;

    private readonly ScoreEventFactory _scoreEventFactory;
    
    /// <summary>
    /// Initializes a new PickResultProcessor with the required scoring service.
    /// </summary>
    /// <param name="pickScoringService">Service used to calculate prediction points.</param>
    /// <param name="scoreEventFactory">ScoreEventFactory to create scoreevents</param>
    public PickResultProcessor(
        PickScoringService pickScoringService,
        ScoreEventFactory scoreEventFactory)
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
    /// <returns>A ScoreEvent representing the prediction outcome.</returns>
    public ScoreEvent ProcessPickResult(
        PickEvaluationContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var points = _pickScoringService.CalculateTotalPoints(context);

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