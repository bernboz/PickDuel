using PickDuel.Domain.Common;
using PickDuel.Domain.Enums;

namespace PickDuel.Domain.Entities;

public class GameResult : Entity
{
    public Game Game { get; private set; }

    public GameOutcome Outcome { get; private set; }

    public int HomeScore { get; private set; }

    public int AwayScore { get; private set; }

    public DateTime CompletedAt { get; private set; }


    public GameResult(
        Game game,
        GameOutcome outcome,
        int homeScore,
        int awayScore)
    {
        ArgumentNullException.ThrowIfNull(game);

        if (homeScore < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(homeScore),
                "Home score cannot be negative."
            );
        }

        if (awayScore < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(awayScore),
                "Away score cannot be negative."
            );
        }


        ValidateOutcome(
            outcome,
            homeScore,
            awayScore
        );


        Game = game;
        Outcome = outcome;
        HomeScore = homeScore;
        AwayScore = awayScore;
        CompletedAt = DateTime.UtcNow;
    }


    /// <summary>
    /// Gets the winning team name from the completed result.
    /// </summary>
    /// <returns>The winning team or null for ties.</returns>
    public string? GetWinningTeam()
    {
        return Outcome switch
        {
            GameOutcome.HomeWin => Game.HomeTeam,
            GameOutcome.AwayWin => Game.AwayTeam,
            GameOutcome.Tie => null,
            _ => null
        };
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
                "Outcome must be HomeWin when the home team has the higher score."
            );
        }


        if (awayScore > homeScore &&
            outcome != GameOutcome.AwayWin)
        {
            throw new ArgumentException(
                "Outcome must be AwayWin when the away team has the higher score."
            );
        }


        if (homeScore == awayScore &&
            outcome != GameOutcome.Tie)
        {
            throw new ArgumentException(
                "Outcome must be Tie when scores are equal."
            );
        }
    }
}