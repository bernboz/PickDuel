using PickDuel.Domain.Entities;

namespace PickDuel.Application.Scoring.Rules;

public class ExactScoreScoringRule : IPickScoringRule
{
    private const int ExactScoreBonusPoints = 50;


    /// <summary>
    /// Calculates bonus points when a user's predicted score exactly matches the final game score.
    /// </summary>
    /// <param name="context">Context containing the pick and completed game result.</param>
    /// <returns>
    /// Exact score bonus points or zero when the prediction does not match.
    /// </returns>
    public int CalculatePoints(PickEvaluationContext context)
    {
        throw new NotImplementedException();
    }
}