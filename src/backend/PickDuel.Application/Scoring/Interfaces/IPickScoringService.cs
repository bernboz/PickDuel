using PickDuel.Domain.Entities;

namespace PickDuel.Application.Scoring;

public interface IPickScoringService
{
    /// <summary>
    /// Calculates the total number of points earned for a completed pick by
    /// evaluating all configured scoring rules.
    /// </summary>
    /// <param name="context">Context containing the completed pick and game result.</param>
    /// <returns>Total points awarded.</returns>
    int CalculateTotalPoints(PickEvaluationContext context);
}