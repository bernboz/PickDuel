using PickDuel.Domain.Entities;

namespace PickDuel.Application.Scoring;

public class PickScoringService
{
    private readonly IEnumerable<IPickScoringRule> _scoringRules;


    public PickScoringService(
        IEnumerable<IPickScoringRule> scoringRules)
    {
        if (scoringRules is null)
        {
            throw new ArgumentNullException(nameof(scoringRules));
        }

        _scoringRules = scoringRules;
    }


    public int CalculateTotalPoints(
        PickEvaluationContext context)
    {
        if (context is null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        var totalPoints = 0;

        foreach (var rule in _scoringRules)
        {
            totalPoints += rule.CalculatePoints(context);
        }

        return totalPoints;
    }
}