using PickDuel.Domain.Entities;

namespace PickDuel.Application.Scoring;

public class PickScoringService : IPickScoringService
{
    private readonly IEnumerable<IPickScoringRule> _scoringRules;


    public PickScoringService(IEnumerable<IPickScoringRule> scoringRules)
    {
        ArgumentNullException.ThrowIfNull(scoringRules);

        _scoringRules = scoringRules;
    }


    /// <summary>
    /// Calculates the total number of points earned for a completed pick by
    /// evaluating every registered scoring rule.
    /// </summary>
    /// <param name="context">Context containing the completed pick and game result.</param>
    /// <returns>Total points awarded by all scoring rules.</returns>
    public int CalculateTotalPoints(PickEvaluationContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var totalPoints = 0;

        foreach (var rule in _scoringRules)
        {
            totalPoints += rule.CalculatePoints(context);
        }

        return totalPoints;
    }
}