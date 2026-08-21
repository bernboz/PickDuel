using PickDuel.Application.Scoring.Interfaces;
using PickDuel.Domain.Entities;
using PickDuel.Domain.Enums;
using PickDuel.Domain.ValueObjects;

namespace PickDuel.Application.Scoring;

public class ScoringCalculator : IScoringCalculator
{
    /// <summary>
    /// Calculates all scoring events generated from a completed pick.
    /// </summary>
    /// <param name="pick">
    /// Pick being evaluated.
    /// </param>
    /// <param name="settings">
    /// Scoring rules used during evaluation.
    /// </param>
    /// <returns>
    /// Collection of scoring results generated from the pick.
    /// </returns>
    public IReadOnlyCollection<ScoringResult> Calculate(Pick pick, ScoringSettings settings)
    {
        ArgumentNullException.ThrowIfNull(pick);
        ArgumentNullException.ThrowIfNull(settings);

        if (!pick.Game.IsCompleted)
        {
            throw new InvalidOperationException(
                "Cannot score a pick before the game is completed."
            );
        }

        if (pick.IsScored)
        {
            throw new InvalidOperationException(
                "Pick has already been scored."
            );
        }

        var results = new List<ScoringResult>();

        results.Add(CalculateWinnerResult(pick, settings));

        if (pick.ScorePrediction != null)
        {
            results.Add(CalculateScoreResult(pick, settings));
        }

        return results.AsReadOnly();
    }


    /// <summary>
    /// Calculates the scoring result for the predicted winner.
    /// </summary>
    /// <param name="pick">
    /// Pick containing the selected team.
    /// </param>
    /// <param name="settings">
    /// Scoring rules used during evaluation.
    /// </param>
    /// <returns>
    /// Winner scoring result.
    /// </returns>
    private static ScoringResult CalculateWinnerResult(Pick pick, ScoringSettings settings)
    {
        if (pick.SelectedTeam == pick.Game.WinningTeam)
        {
            return new ScoringResult(
                settings.WinnerPoints * pick.ConfidenceMultiplier,
                ScoreEventType.CorrectWinner,
                $"Correct winner prediction for {pick.SelectedTeam}."
            );
        }

        return new ScoringResult(
            settings.ScoreAccuracyPenalty,
            ScoreEventType.Penalty,
            $"Incorrect winner prediction for {pick.SelectedTeam}."
        );
    }


    /// <summary>
    /// Calculates the scoring result for an exact score prediction.
    /// </summary>
    /// <param name="pick">
    /// Pick containing the score prediction.
    /// </param>
    /// <param name="settings">
    /// Scoring rules used during evaluation.
    /// </param>
    /// <returns>
    /// Score prediction result.
    /// </returns>
    private static ScoringResult CalculateScoreResult(Pick pick, ScoringSettings settings)
    {
        var prediction = pick.ScorePrediction!;

        var actualHomeScore = pick.Game.HomeScore!.Value;
        var actualAwayScore = pick.Game.AwayScore!.Value;

        if (prediction.HomeScore == actualHomeScore &&
            prediction.AwayScore == actualAwayScore)
        {
            return new ScoringResult(
                settings.ExactScorePoints * pick.ConfidenceMultiplier,
                ScoreEventType.ExactScore,
                "Exact score prediction."
            );
        }

        var scoreDifference =
            Math.Abs(prediction.HomeScore - actualHomeScore) +
            Math.Abs(prediction.AwayScore - actualAwayScore);

        if (scoreDifference <= settings.ScoreTolerance)
        {
            return new ScoringResult(
                settings.ScoreAccuracyBonus * pick.ConfidenceMultiplier,
                ScoreEventType.ScoreDifference,
                "Score prediction was within the accuracy range."
            );
        }

        if (scoreDifference >= settings.MaxScoreDifferencePenalty)
        {
            return new ScoringResult(
                settings.ScoreAccuracyPenalty,
                ScoreEventType.Penalty,
                "Score prediction was outside the allowed accuracy range."
            );
        }

        return new ScoringResult(
            0,
            ScoreEventType.Neutral,
            "Score prediction was not accurate enough for bonus points."
        );
    }
}