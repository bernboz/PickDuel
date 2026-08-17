using PickDuel.Domain.Entities;
using PickDuel.Domain.Enums;

namespace PickDuel.Application.Scoring.Rules;

public class WinnerPredictionRule : IPickScoringRule
{
    public int CalculatePoints(PickEvaluationContext context)
    {
        var selectedTeam = context.Pick.SelectedTeam;

        string? winningTeam = context.GameResult.Outcome switch
        {
            GameOutcome.HomeWin => context.Pick.Game.HomeTeam,
            GameOutcome.AwayWin => context.Pick.Game.AwayTeam,
            GameOutcome.Tie => null,
            _ => null
        };

        if (selectedTeam == winningTeam)
        {
            return context.Pick.League.ScoringSettings.WinnerPoints;
        }

        return 0;
    }
}