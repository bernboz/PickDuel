namespace PickDuel.Domain.Entities.Predictions;

public class ScorePrediction
{
    public int HomeScore { get; private set; }

    public int AwayScore { get; private set; }


    public ScorePrediction(
        int homeScore,
        int awayScore)
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