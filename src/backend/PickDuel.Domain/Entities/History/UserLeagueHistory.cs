using PickDuel.Domain.Common;
using PickDuel.Domain.Entities.Matchups;
using PickDuel.Domain.Enums;

namespace PickDuel.Domain.Entities.History;

public class UserLeagueHistory : Entity
{
    public User User { get; private set; }

    public League League { get; private set; }


    private readonly List<LeagueMatchup> _matchups = new();

    public IReadOnlyCollection<LeagueMatchup> Matchups =>
        _matchups.AsReadOnly();


    public int TotalPoints { get; private set; }

    public int MatchupWins { get; private set; }

    public int MatchupLosses { get; private set; }

    public int MatchupTies { get; private set; }


    public int CurrentWinStreak { get; private set; }

    public int LongestWinStreak { get; private set; }


    public DateTime CreatedAt { get; private set; }


    /// <summary>
    /// Initializes a new user league history tracker.
    /// </summary>
    /// <param name="user">User whose completed matchup history is tracked.</param>
    /// <param name="league">League that the history belongs to.</param>
    public UserLeagueHistory(
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

        CurrentWinStreak = 0;
        LongestWinStreak = 0;

        CreatedAt = DateTime.UtcNow;
    }


    /// <summary>
    /// Adds a completed matchup to the user's history and updates cached statistics.
    /// </summary>
    /// <param name="matchup">Completed matchup to add to history.</param>
    public void AddMatchup(
        LeagueMatchup matchup)
    {
        ArgumentNullException.ThrowIfNull(matchup);


        if (matchup.League != League)
        {
            throw new InvalidOperationException(
                "Matchup must belong to this league."
            );
        }


        if (matchup.Status != MatchupStatus.Completed)
        {
            throw new InvalidOperationException(
                "Only completed matchups can be added to history."
            );
        }


        if (matchup.UserOne != User &&
            matchup.UserTwo != User)
        {
            throw new InvalidOperationException(
                "User must participate in the matchup."
            );
        }


        if (_matchups.Contains(matchup))
        {
            throw new InvalidOperationException(
                "Matchup already exists in history."
            );
        }


        _matchups.Add(matchup);

        UpdateStatistics(matchup);
    }


    /// <summary>
    /// Updates cached performance statistics based on a completed matchup.
    /// </summary>
    /// <param name="matchup">Completed matchup used to update statistics.</param>
    private void UpdateStatistics(
        LeagueMatchup matchup)
    {
        TotalPoints += GetUserPoints(matchup);


        if (matchup.Result == MatchupResult.Tie)
        {
            MatchupTies++;

            CurrentWinStreak = 0;

            return;
        }


        bool userWon =
            matchup.Result == MatchupResult.UserOneWin &&
            matchup.UserOne == User
            ||
            matchup.Result == MatchupResult.UserTwoWin &&
            matchup.UserTwo == User;


        if (userWon)
        {
            MatchupWins++;

            CurrentWinStreak++;


            if (CurrentWinStreak > LongestWinStreak)
            {
                LongestWinStreak = CurrentWinStreak;
            }


            return;
        }


        MatchupLosses++;

        CurrentWinStreak = 0;
    }


    /// <summary>
    /// Gets the number of points earned by this user in a matchup.
    /// </summary>
    /// <param name="matchup">Matchup containing user point totals.</param>
    /// <returns>The points earned by this user.</returns>
    private int GetUserPoints(
        LeagueMatchup matchup)
    {
        if (matchup.UserOne == User)
        {
            return matchup.UserOnePoints;
        }


        return matchup.UserTwoPoints;
    }
}