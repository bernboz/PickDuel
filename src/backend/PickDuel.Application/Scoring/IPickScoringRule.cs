using PickDuel.Domain.Entities;

namespace PickDuel.Application.Scoring;

public interface IPickScoringRule
{
    int CalculatePoints(PickEvaluationContext context);
}