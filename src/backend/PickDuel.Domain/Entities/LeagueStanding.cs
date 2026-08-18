using PickDuel.Domain.Common;
using PickDuel.Domain.Enums;

namespace PickDuel.Domain.Entities;

public class LeagueStanding : Entity
{
    public User User { get; private set; }

    public League League { get; private set; }

    public int TotalPoints { get; private set; }

    public int TotalWins { get; private set; }

    public int TotalLosses { get; private set; }

    public int TotalPicks { get; private set; }


    /// <summary>
    /// Creates a league standing for a user within a league.
    /// </summary>
    /// <param name="user">User represented by this standing.</param>
    /// <param name="league">League associated with this standing.</param>
    public LeagueStanding(User user, League league)
    {
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        if (league is null)
        {
            throw new ArgumentNullException(nameof(league));
        }

        User = user;
        League = league;

        TotalPoints = 0;
        TotalWins = 0;
        TotalLosses = 0;
        TotalPicks = 0;
    }


    /// <summary>
    /// Adds points earned from a completed pick result.
    /// </summary>
    /// <param name="points">Points gained or lost from scoring rules.</param>
    public void AddPoints(int points)
    {
        TotalPoints += points;
    }


    /// <summary>
    /// Records the result of a completed prediction.
    /// </summary>
    /// <param name="isWinner">Whether the user's prediction was correct.</param>
    public void RecordPredictionResult(bool isWinner)
    {
        TotalPicks++;

        if (isWinner)
        {
            TotalWins++;
        }
        else
        {
            TotalLosses++;
        }
    }


    /// <summary>
    /// Calculates the user's prediction success percentage.
    /// </summary>
    /// <returns>
    /// Percentage of correct predictions from completed picks.
    /// </returns>
    public decimal GetWinPercentage()
    {
        if (TotalPicks == 0)
        {
            return 0;
        }

        return (decimal)TotalWins / TotalPicks * 100;
    }
    
    /// <summary>
    /// Applies a completed score event to this league standing.
    /// </summary>
    /// <param name="scoreEvent">Completed scoring event.</param>
    public void ApplyScoreEvent(ScoreEvent scoreEvent)
    {
        if (scoreEvent is null)
        {
            throw new ArgumentNullException(nameof(scoreEvent));
        }

        TotalPoints += scoreEvent.Points;
    
        TotalPicks++;

        switch (scoreEvent.Type)
        {
            case ScoreEventType.CorrectWinner:
                TotalWins++;
                break;

            case ScoreEventType.Penalty:
                TotalLosses++;
                break;
        }
    }
}