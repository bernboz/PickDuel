using PickDuel.Domain.Entities;

namespace PickDuel.Application.Scoring;

public interface IScoringRuleFactory
{
    /// <summary>
    /// Retrieves scoring rules applicable to the current prediction evaluation.
    /// </summary>
    /// <param name="context">
    /// Context containing prediction, game result, and scoring information.
    /// </param>
    /// <returns>
    /// Collection of scoring rules to evaluate.
    /// </returns>
    IEnumerable<IPickScoringRule> GetRules(
        PickEvaluationContext context);
}