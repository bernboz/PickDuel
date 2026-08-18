namespace PickDuel.Domain.ValueObjects;

public class ScorePrediction
{
    public int HomeScore { get; private set; }

    public int AwayScore { get; private set; }


    /// <summary>
    /// Initializes a new score prediction with the predicted scores for both teams.
    /// </summary>
    /// <param name="homeScore">Predicted score for the home team.</param>
    /// <param name="awayScore">Predicted score for the away team.</param>
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