using PickDuel.Domain.Entities;
using PickDuel.Domain.Enums;

namespace PickDuel.Application.Scoring;

public class PickResultProcessor
{
    private readonly PickScoringService _pickScoringService;


    /// <summary>
    /// Initializes a new PickResultProcessor with the required scoring service.
    /// </summary>
    /// <param name="pickScoringService">Service used to calculate prediction points.</param>
    public PickResultProcessor(PickScoringService pickScoringService)
    {
        if (pickScoringService is null)
        {
            throw new ArgumentNullException(nameof(pickScoringService));
        }

        _pickScoringService = pickScoringService;
    }


    /// <summary>
    /// Processes a completed pick evaluation and creates a score event.
    /// </summary>
    /// <param name="context">Context containing the pick, result, and odds information.</param>
    /// <returns>A ScoreEvent representing the result of the prediction.</returns>
    public ScoreEvent ProcessPickResult(PickEvaluationContext context)
    {
        if (context is null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        var points = _pickScoringService.CalculateTotalPoints(context);

        var scoreEventType = points switch
        {
            > 0 => ScoreEventType.CorrectWinner,
            < 0 => ScoreEventType.Penalty,
            _ => ScoreEventType.CorrectWinner
        };

        var description = scoreEventType switch
        {
            ScoreEventType.CorrectWinner => "Correct winner prediction",
            ScoreEventType.Penalty => "Incorrect prediction penalty",
            _ => "Prediction processed"
        };

        return new ScoreEvent(
            context.Pick.User,
            context.Pick.League,
            points,
            scoreEventType,
            description
        );
    }


    /// <summary>
    /// Applies a score event to update a league standing.
    /// </summary>
    /// <param name="scoreEvent">Score event containing points and result information.</param>
    /// <param name="standing">League standing receiving the update.</param>
    public void ApplyScoreEvent(ScoreEvent scoreEvent, LeagueStanding standing)
    {
        if (scoreEvent is null)
        {
            throw new ArgumentNullException(nameof(scoreEvent));
        }

        if (standing is null)
        {
            throw new ArgumentNullException(nameof(standing));
        }

        standing.ApplyScoreEvent(scoreEvent);
    }
}