using PickDuel.Domain.Common;

namespace PickDuel.Domain.Entities.Standings;

public class LeagueStanding : Entity
{
    public User User { get; private set; }

    public League League { get; private set; }

    public int TotalPoints { get; private set; }

    public int MatchupWins { get; private set; }

    public int MatchupLosses { get; private set; }

    public int MatchupTies { get; private set; }

    public int TotalMatchups =>
        MatchupWins +
        MatchupLosses +
        MatchupTies;


    /// <summary>
    /// Creates a league standing for a user within a league.
    /// </summary>
    /// <param name="user">User represented by the standing.</param>
    /// <param name="league">League associated with the standing.</param>
    public LeagueStanding(
        User user,
        League league)
    {
        ArgumentNullException.ThrowIfNull(user);
        ArgumentNullException.ThrowIfNull(league);

        User = user;
        League = league;

        TotalPoints = 0;

        MatchupWins = 0;
        MatchupLosses = 0;
        MatchupTies = 0;
    }


    /// <summary>
    /// Adds points earned from completed predictions.
    /// </summary>
    public void AddPoints(int points)
    {
        TotalPoints += points;
    }


    /// <summary>
    /// Records a head-to-head matchup victory.
    /// </summary>
    public void RecordMatchupWin()
    {
        MatchupWins++;
    }


    /// <summary>
    /// Records a head-to-head matchup loss.
    /// </summary>
    public void RecordMatchupLoss()
    {
        MatchupLosses++;
    }


    /// <summary>
    /// Records a tied head-to-head matchup.
    /// </summary>
    public void RecordMatchupTie()
    {
        MatchupTies++;
    }


    /// <summary>
    /// Calculates the user's matchup win percentage.
    /// </summary>
    /// <returns>Percentage of matchups won.</returns>
    public decimal GetMatchupWinPercentage()
    {
        if (TotalMatchups == 0)
        {
            return 0;
        }

        return (decimal)MatchupWins / TotalMatchups * 100;
    }
}