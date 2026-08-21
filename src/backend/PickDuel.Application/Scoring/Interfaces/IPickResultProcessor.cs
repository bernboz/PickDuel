using PickDuel.Domain.Entities.Standings;
using PickDuel.Domain.Entities;

namespace PickDuel.Application.Scoring.Interfaces;

public interface IPickResultProcessor
{
    /// <summary>
    /// Processes a completed pick evaluation and creates a score event.
    /// </summary>
    /// <param name="context">Context containing the pick, result, and odds information.</param>
    /// <returns>A score event representing the prediction outcome.</returns>
    ScoreEvent ProcessPickResult(PickEvaluationContext context);


    /// <summary>
    /// Applies a completed score event to a league standing.
    /// </summary>
    /// <param name="scoreEvent">Score event containing the scoring outcome.</param>
    /// <param name="standing">League standing to update.</param>
    void ApplyScoreEvent(ScoreEvent scoreEvent, LeagueStanding standing);
}