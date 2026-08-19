using PickDuel.Domain.Entities;

namespace PickDuel.Application.Scoring;

public class PickScoringService : IPickScoringService
{
    private readonly IScoringRuleFactory _scoringRuleFactory;


    /// <summary>
    /// Initializes a PickScoringService using a scoring rule provider.
    /// </summary>
    /// <param name="scoringRuleFactory">
    /// Provider responsible for supplying scoring rules.
    /// </param>
    public PickScoringService(
        IScoringRuleFactory scoringRuleFactory)
    {
        ArgumentNullException.ThrowIfNull(scoringRuleFactory);

        _scoringRuleFactory = scoringRuleFactory;
    }


    /// <summary>
    /// Calculates the total number of points earned for a completed pick by
    /// evaluating every configured scoring rule.
    /// </summary>
    /// <param name="context">
    /// Context containing the completed pick and game result.
    /// </param>
    /// <returns>
    /// Total points awarded by all scoring rules.
    /// </returns>
    public int CalculateTotalPoints(
        PickEvaluationContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var totalPoints = 0;

        foreach (var rule in _scoringRuleFactory.GetRules(context))
        {
            totalPoints += rule.CalculatePoints(context);
        }

        return totalPoints;
    }
}