namespace PickDuel.Domain.ValueObjects;

public class ScoringSettings
{
    public int WinnerPoints { get; private set; }

    public int ExactScorePoints { get; private set; }


    public ScoringSettings(
        int winnerPoints,
        int exactScorePoints)
    {
        if (winnerPoints < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(winnerPoints));
        }

        if (exactScorePoints < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(exactScorePoints));
        }
        WinnerPoints = winnerPoints;
        ExactScorePoints = exactScorePoints;
    }
}