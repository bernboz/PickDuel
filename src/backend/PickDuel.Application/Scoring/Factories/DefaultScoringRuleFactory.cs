using PickDuel.Application.Scoring.Rules;
using PickDuel.Domain.Entities;

namespace PickDuel.Application.Scoring.Providers;

public class DefaultScoringRuleFactory : IScoringRuleFactory
{
    /// <summary>
    /// Provides the default scoring rules used by PickDuel.
    /// </summary>
    public IEnumerable<IPickScoringRule> GetRules(
        PickEvaluationContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        return
        [
            new ConfidenceScoringRule(),
            new ScoreAccuracyRule(
                context.Pick.League.ScoringSettings)
        ];
    }
}