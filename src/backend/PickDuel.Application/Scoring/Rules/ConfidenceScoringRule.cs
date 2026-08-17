using PickDuel.Domain.Enums;

namespace PickDuel.Application.Scoring.Rules;

public class ConfidenceScoringRule : IPickScoringRule
{
    private const int BasePredictionPoints = 10;
    private const decimal MaximumDifficultyMultiplier = 3m;


    /// <summary>
    /// Calculates points earned or lost based on prediction accuracy, confidence, and game difficulty.
    /// </summary>
    /// <param name="context">The evaluation context containing the pick, result, and odds.</param>
    /// <returns>
    /// Positive points for correct predictions and negative points for incorrect predictions.
    /// </returns>
    public int CalculatePoints(PickEvaluationContext context)
    {
        if (context is null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        var selectedTeamProbability = GetSelectedTeamProbability(context);

        var difficultyMultiplier = CalculateDifficultyMultiplier(selectedTeamProbability);

        var potentialPoints =
            BasePredictionPoints
            * difficultyMultiplier
            * context.Pick.ConfidenceMultiplier;


        if (IsCorrectPrediction(context))
        {
            return (int)Math.Round(potentialPoints);
        }


        return -(int)Math.Round(potentialPoints / 2);
    }


    /// <summary>
    /// Calculates the difficulty multiplier based on how likely the selected team was expected to win.
    /// </summary>
    /// <param name="winProbability">The probability of the selected team winning.</param>
    /// <returns>
    /// A multiplier between 1 and the maximum difficulty multiplier.
    /// </returns>
    private static decimal CalculateDifficultyMultiplier(decimal winProbability)
    {
        if (winProbability <= 0 || winProbability > 1)
        {
            throw new ArgumentOutOfRangeException(nameof(winProbability));
        }

        var difficultyMultiplier = 1 / winProbability;

        return Math.Min(
            difficultyMultiplier,
            MaximumDifficultyMultiplier
        );
    }


    /// <summary>
    /// Gets the win probability associated with the user's selected team.
    /// </summary>
    /// <param name="context">The pick evaluation context.</param>
    /// <returns>
    /// The implied probability of the selected team's victory.
    /// </returns>
    private static decimal GetSelectedTeamProbability(PickEvaluationContext context)
    {
        return context.Pick.SelectedTeam == context.Pick.Game.HomeTeam
            ? context.GameOdds.HomeWinProbability
            : context.GameOdds.AwayWinProbability;
    }


    /// <summary>
    /// Determines whether the user's selected team won the game.
    /// </summary>
    /// <param name="context">The pick evaluation context.</param>
    /// <returns>
    /// True when the selected team matches the game outcome; otherwise false.
    /// </returns>
    private static bool IsCorrectPrediction(PickEvaluationContext context)
    {
        return context.GameResult.Outcome switch
        {
            GameOutcome.HomeWin =>
                context.Pick.SelectedTeam == context.Pick.Game.HomeTeam,

            GameOutcome.AwayWin =>
                context.Pick.SelectedTeam == context.Pick.Game.AwayTeam,

            GameOutcome.Tie =>
                false,

            _ => false
        };
    }
}