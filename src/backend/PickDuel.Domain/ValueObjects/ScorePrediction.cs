namespace PickDuel.Domain.ValueObjects;

public class ScorePrediction
{
    public int HomeScore { get; private set; }

    public int AwayScore { get; private set; }


    /// <summary>
    /// Creates a predicted final score for a game.
    /// </summary>
    /// <param name="homeScore">
    /// Predicted home team score.
    /// </param>
    /// <param name="awayScore">
    /// Predicted away team score.
    /// </param>
    public ScorePrediction(int homeScore, int awayScore)
    {
        if (homeScore < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(homeScore));
        }

        if (awayScore < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(awayScore));
        }

        HomeScore = homeScore;
        AwayScore = awayScore;
    }
}