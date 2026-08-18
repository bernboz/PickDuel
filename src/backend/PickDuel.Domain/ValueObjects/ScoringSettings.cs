namespace PickDuel.Domain.ValueObjects;

/// <summary>
/// Defines configurable scoring rules for a league.
/// </summary>
public class ScoringSettings
{
    public int WinnerPoints { get; private set; }

    public int ExactScorePoints { get; private set; }

    public int ScoreAccuracyBonus { get; private set; }

    public int ScoreAccuracyPenalty { get; private set; }

    public int ScoreTolerance { get; private set; }

    public int MaxScoreDifferencePenalty { get; private set; }


    /// <summary>
    /// Creates scoring settings used by league scoring rules.
    /// </summary>
    public ScoringSettings(
        int winnerPoints,
        int exactScorePoints,
        int scoreAccuracyBonus,
        int scoreAccuracyPenalty,
        int scoreTolerance,
        int maxScoreDifferencePenalty)
    {
        if (winnerPoints < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(winnerPoints));
        }

        if (exactScorePoints < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(exactScorePoints));
        }

        if (scoreAccuracyBonus < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(scoreAccuracyBonus));
        }

        if (scoreAccuracyPenalty > 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(scoreAccuracyPenalty),
                "Accuracy penalties must be zero or negative."
            );
        }

        if (scoreTolerance < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(scoreTolerance));
        }

        if (maxScoreDifferencePenalty < scoreTolerance)
        {
            throw new ArgumentOutOfRangeException(
                nameof(maxScoreDifferencePenalty),
                "Maximum penalty threshold must be greater than tolerance."
            );
        }


        WinnerPoints = winnerPoints;
        ExactScorePoints = exactScorePoints;
        ScoreAccuracyBonus = scoreAccuracyBonus;
        ScoreAccuracyPenalty = scoreAccuracyPenalty;
        ScoreTolerance = scoreTolerance;
        MaxScoreDifferencePenalty = maxScoreDifferencePenalty;
    }
}