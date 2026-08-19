using PickDuel.Domain.Entities;

namespace PickDuel.Application.Scoring;

public interface IScoreEventFactory
{
    ScoreEvent Create(PickEvaluationContext context, int points);
}