using PickDuel.Domain.Entities;
using PickDuel.Domain.Enums;

namespace PickDuel.Application.Scoring;

public class PickResultProcessor
{
    private readonly PickScoringService _pickScoringService;


    public PickResultProcessor(
        PickScoringService pickScoringService)
    {
        if (pickScoringService is null)
        {
            throw new ArgumentNullException(nameof(pickScoringService));
        }
        _pickScoringService = pickScoringService;
    }


    public ScoreEvent ProcessPickResult(
        PickEvaluationContext context)
    {
        if (context is null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        var points = _pickScoringService.CalculateTotalPoints(context);

        ScoreEventType scoreEventType = points switch
        {
            > 0 => ScoreEventType.CorrectWinner,
            < 0 => ScoreEventType.Penalty,
            _ => ScoreEventType.CorrectWinner
        };

        string description = scoreEventType switch
        {
            ScoreEventType.CorrectWinner => "Correct winner prediction",
            ScoreEventType.Penalty => "Incorrect prediction penalty",
            _ => "Pick result processed"
        };

        return new ScoreEvent(
            context.Pick.User,
            context.Pick.League,
            points,
            scoreEventType,
            description
        );
    }


    public void ApplyScoreEvent(
        ScoreEvent scoreEvent,
        LeagueStanding standing)
    {
        if (scoreEvent is null)
        {
            throw new ArgumentNullException(nameof(scoreEvent));
        }

        if (standing is null)
        {
            throw new ArgumentNullException(nameof(standing));
        }

        standing.AddPoints(scoreEvent.Points);
    }
}