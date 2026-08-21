using PickDuel.Domain.Entities;
using PickDuel.Domain.ValueObjects;

namespace PickDuel.Application.Scoring.Interfaces;

public interface IScoringCalculator
{
    /// <summary>
    /// Calculates all scoring events generated from a completed pick.
    /// </summary>
    /// <param name="pick">
    /// Pick being evaluated.
    /// </param>
    /// <param name="settings">
    /// Scoring rules used to evaluate the pick.
    /// </param>
    /// <returns>
    /// Scoring events generated from the pick evaluation.
    /// </returns>
    IReadOnlyCollection<ScoringResult> Calculate(Pick pick, ScoringSettings settings);
}