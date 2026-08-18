using PickDuel.Domain.Common;
using PickDuel.Domain.Entities.Predictions;
using PickDuel.Domain.Enums;

namespace PickDuel.Domain.Entities.History;

public class PickHistory : Entity
{
    public User User { get; private set; }

    public League League { get; private set; }

    public Game Game { get; private set; }

    public string PredictedTeam { get; private set; }

    public ScorePrediction? PredictedScore { get; private set; }

    public GameOutcome ActualOutcome { get; private set; }

    public int ActualHomeScore { get; private set; }

    public int ActualAwayScore { get; private set; }

    public int PointsEarned { get; private set; }

    public ScoreEventType ResultType { get; private set; }

    public DateTime CompletedAt { get; private set; }


    public PickHistory(
        User user,
        League league,
        Game game,
        string predictedTeam,
        ScorePrediction? predictedScore,
        GameOutcome actualOutcome,
        int actualHomeScore,
        int actualAwayScore,
        int pointsEarned,
        ScoreEventType resultType)
    {
        ArgumentNullException.ThrowIfNull(user);
        ArgumentNullException.ThrowIfNull(league);
        ArgumentNullException.ThrowIfNull(game);

        if (string.IsNullOrWhiteSpace(predictedTeam))
        {
            throw new ArgumentException(
                "Predicted team cannot be empty.",
                nameof(predictedTeam)
            );
        }

        if (predictedTeam != game.HomeTeam &&
            predictedTeam != game.AwayTeam)
        {
            throw new ArgumentException(
                "Predicted team must belong to the game.",
                nameof(predictedTeam)
            );
        }

        if (actualHomeScore < 0 ||
            actualAwayScore < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(actualHomeScore),
                "Actual scores cannot be negative."
            );
        }

        ValidateOutcome(
            actualOutcome,
            actualHomeScore,
            actualAwayScore
        );


        User = user;
        League = league;
        Game = game;

        PredictedTeam = predictedTeam;
        PredictedScore = predictedScore;

        ActualOutcome = actualOutcome;
        ActualHomeScore = actualHomeScore;
        ActualAwayScore = actualAwayScore;

        PointsEarned = pointsEarned;
        ResultType = resultType;

        CompletedAt = DateTime.UtcNow;
    }


    private static void ValidateOutcome(
        GameOutcome outcome,
        int homeScore,
        int awayScore)
    {
        if (homeScore > awayScore &&
            outcome != GameOutcome.HomeWin)
        {
            throw new ArgumentException(
                "Outcome does not match the final score."
            );
        }

        if (awayScore > homeScore &&
            outcome != GameOutcome.AwayWin)
        {
            throw new ArgumentException(
                "Outcome does not match the final score."
            );
        }

        if (homeScore == awayScore &&
            outcome != GameOutcome.Tie)
        {
            throw new ArgumentException(
                "Outcome does not match the final score."
            );
        }
    }
}