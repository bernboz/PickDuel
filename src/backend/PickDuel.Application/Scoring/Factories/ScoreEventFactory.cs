using PickDuel.Domain.Entities;
using PickDuel.Domain.Enums;

namespace PickDuel.Application.Scoring.Factories;

public class ScoreEventFactory : IScoreEventFactory
{
    public ScoreEvent Create(
        PickEvaluationContext context,
        int points)
    {
        ArgumentNullException.ThrowIfNull(context);

        var type = DetermineScoreEventType(
            context,
            points
        );

        var description = DetermineDescription(type);

        return new ScoreEvent(
            context.Pick.User,
            context.Pick.League,
            points,
            type,
            description,
            context.Pick
        );
    }


    private static ScoreEventType DetermineScoreEventType(
        PickEvaluationContext context,
        int points)
    {
        if (points == 0)
        {
            return ScoreEventType.Neutral;
        }

        if (context.Pick.ScorePrediction is not null &&
            context.Pick.ScorePrediction.HomeScore == context.GameResult.HomeScore &&
            context.Pick.ScorePrediction.AwayScore == context.GameResult.AwayScore)
        {
            return ScoreEventType.ExactScore;
        }

        if (context.Pick.SelectedTeam == 
            context.GameResult.GetWinningTeam())
        {
            return ScoreEventType.CorrectWinner;
        }

        return ScoreEventType.Penalty;
    }


    private static string DetermineDescription(
        ScoreEventType type)
    {
        return type switch
        {
            ScoreEventType.CorrectWinner =>
                "Correct winner prediction",

            ScoreEventType.ExactScore =>
                "Exact score prediction",

            ScoreEventType.ScoreDifference =>
                "Score accuracy prediction",

            ScoreEventType.Penalty =>
                "Incorrect prediction penalty",

            ScoreEventType.Neutral =>
                "Prediction processed with no points",

            _ =>
                "Prediction processed"
        };
    }
}