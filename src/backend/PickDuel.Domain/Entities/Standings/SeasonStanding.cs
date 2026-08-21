using PickDuel.Domain.Common;
using PickDuel.Domain.Entities.Matchups;
using PickDuel.Domain.Enums;

namespace PickDuel.Domain.Entities.Standings;

public class SeasonStanding : Entity
{
    public LeagueSeason Season { get; private set; }

    public User User { get; private set; }


    public int Rank { get; private set; }


    public int TotalPoints { get; private set; }

    public int PointsFor { get; private set; }

    public int PointsAgainst { get; private set; }


    public int Wins { get; private set; }

    public int Losses { get; private set; }

    public int Ties { get; private set; }


    public int MatchupsPlayed { get; private set; }


    public bool MadePlayoffs { get; private set; }

    public bool IsChampion { get; private set; }


    private readonly List<LeagueMatchup> _matchups = new();

    public IReadOnlyCollection<LeagueMatchup> Matchups =>
        _matchups.AsReadOnly();


    public DateTime CreatedAt { get; private set; }


    /// <summary>
    /// Initializes a season standing record for a user within a league season.
    /// </summary>
    /// <param name="season">Season this standing belongs to.</param>
    /// <param name="user">User whose performance is being tracked.</param>
    public SeasonStanding(LeagueSeason season, User user)
    {
        ArgumentNullException.ThrowIfNull(season);
        ArgumentNullException.ThrowIfNull(user);

        Season = season;
        User = user;
        
        Rank = 1;

        TotalPoints = 0;
        PointsFor = 0;
        PointsAgainst = 0;

        Wins = 0;
        Losses = 0;
        Ties = 0;

        MatchupsPlayed = 0;

        MadePlayoffs = false;
        IsChampion = false;
        
        CreatedAt = DateTime.UtcNow;
    }


    /// <summary>
    /// Updates season statistics using a completed matchup involving this user.
    /// </summary>
    /// <param name="matchup">Completed matchup used to update standings.</param>
    public void UpdateFromMatchup(LeagueMatchup matchup)
    {
        ArgumentNullException.ThrowIfNull(matchup);
        
        if (!Season.Matchups.Contains(matchup))
        {
            throw new InvalidOperationException(
                "Matchup must belong to this season."
            );
        }
        
        if (matchup.Status != MatchupStatus.Completed)
        {
            throw new InvalidOperationException(
                "Only completed matchups can update standings."
            );
        }
        
        if (matchup.UserOne != User &&
            matchup.UserTwo != User)
        {
            throw new InvalidOperationException(
                "User must participate in matchup."
            );
        }
        
        if (_matchups.Contains(matchup))
        {
            throw new InvalidOperationException(
                "Matchup has already been applied to this standing."
            );
        }
        
        _matchups.Add(matchup);
        
        bool isUserOne = matchup.UserOne == User;

        int userPoints = isUserOne
            ? matchup.UserOnePoints
            : matchup.UserTwoPoints;
        
        int opponentPoints = isUserOne
            ? matchup.UserTwoPoints
            : matchup.UserOnePoints;

        TotalPoints += userPoints;

        PointsFor += userPoints;

        PointsAgainst += opponentPoints;
        
        if (userPoints > opponentPoints)
        {
            Wins++;
        }
        else if (userPoints < opponentPoints)
        {
            Losses++;
        }
        else
        {
            Ties++;
        }

        MatchupsPlayed++;
    }


    /// <summary>
    /// Updates the user's current ranking position within the season.
    /// </summary>
    /// <param name="rank">New ranking position.</param>
    public void UpdateRank(int rank)
    {
        if (rank < 1)
        {
            throw new ArgumentOutOfRangeException(
                nameof(rank),
                "Rank must be greater than zero."
            );
        }


        Rank = rank;
    }


    /// <summary>
    /// Updates whether the user qualified for the playoffs.
    /// </summary>
    /// <param name="madePlayoffs">Whether the user made playoffs.</param>
    public void SetPlayoffStatus(bool madePlayoffs)
    {
        MadePlayoffs = madePlayoffs;
    }


    /// <summary>
    /// Marks this user as the season champion.
    /// </summary>
    public void CrownChampion()
    {
        if (!MadePlayoffs)
        {
            throw new InvalidOperationException(
                "Only playoff participants can become champions."
            );
        }


        IsChampion = true;
    }
    
    /// <summary>
    /// Removes the user's champion status for the season.
    /// </summary>
    public void RemoveChampionStatus()
    {
        IsChampion = false;
    }
}