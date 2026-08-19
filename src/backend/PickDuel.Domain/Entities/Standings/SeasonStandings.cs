using PickDuel.Domain.Common;
using PickDuel.Domain.Entities.Matchups;
using PickDuel.Domain.Enums;

namespace PickDuel.Domain.Entities;

public class SeasonStandings : Entity
{
    public LeagueSeason Season { get; private set; }


    private readonly List<SeasonStanding> _standings = new();

    public IReadOnlyCollection<SeasonStanding> Standings =>
        _standings.AsReadOnly();


    public DateTime CreatedAt { get; private set; }


    /// <summary>
    /// Initializes a season standings tracker for a league season.
    /// </summary>
    /// <param name="season">Season this standings tracker belongs to.</param>
    public SeasonStandings(
        LeagueSeason season)
    {
        ArgumentNullException.ThrowIfNull(season);

        Season = season;

        CreatedAt = DateTime.UtcNow;
    }


    /// <summary>
    /// Adds a user to the season standings.
    /// </summary>
    /// <param name="user">User being added to the season leaderboard.</param>
    public void AddUser(
        User user)
    {
        ArgumentNullException.ThrowIfNull(user);


        if (_standings.Any(x => x.User == user))
        {
            throw new InvalidOperationException(
                "User already exists in season standings."
            );
        }


        var standing = new SeasonStanding(
            Season,
            user
        );


        _standings.Add(standing);

        UpdateRankings();
    }


    /// <summary>
    /// Processes a completed matchup and updates both participating users' season statistics.
    /// </summary>
    /// <param name="matchup">Completed matchup used to update standings.</param>
    public void ProcessMatchup(
        LeagueMatchup matchup)
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
        
        if (matchup.Status != MatchupStatus.Completed)
        {
            throw new InvalidOperationException(
                "Only completed matchups can update standings."
            );
        }


        var userOneStanding =
            _standings.FirstOrDefault(
                x => x.User == matchup.UserOne);


        var userTwoStanding =
            _standings.FirstOrDefault(
                x => x.User == matchup.UserTwo);


        if (userOneStanding == null ||
            userTwoStanding == null)
        {
            throw new InvalidOperationException(
                "Both users must exist in season standings."
            );
        }


        userOneStanding.UpdateFromMatchup(matchup);

        userTwoStanding.UpdateFromMatchup(matchup);


        UpdateRankings();
    }


    /// <summary>
    /// Recalculates all rankings based on current season performance.
    /// </summary>
    private void UpdateRankings()
    {
        var orderedStandings = _standings
            .OrderByDescending(x => x.Wins)
            .ThenByDescending(x => x.TotalPoints)
            .ThenByDescending(x => x.Ties)
            .ThenBy(x => x.Losses)
            .ToList();


        for (int i = 0; i < orderedStandings.Count; i++)
        {
            orderedStandings[i].UpdateRank(i + 1);
        }


        _standings.Clear();

        _standings.AddRange(orderedStandings);
    }


    /// <summary>
    /// Gets the current first-place user in the season standings.
    /// </summary>
    /// <returns>The highest ranked user's standing.</returns>
    public SeasonStanding GetLeader()
    {
        var leader = _standings
            .FirstOrDefault(x => x.Rank == 1);


        if (leader == null)
        {
            throw new InvalidOperationException(
                "No standings exist."
            );
        }


        return leader;
    }


    /// <summary>
    /// Marks users as playoff qualifiers based on playoff spots available.
    /// </summary>
    /// <param name="playoffSpots">Number of users who qualify for playoffs.</param>
    public void AssignPlayoffQualification(
        int playoffSpots)
    {
        if (playoffSpots <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(playoffSpots)
            );
        }


        foreach (var standing in _standings)
        {
            standing.SetPlayoffStatus(false);
        }


        foreach (var standing in _standings
                     .OrderBy(x => x.Rank)
                     .Take(playoffSpots))
        {
            standing.SetPlayoffStatus(true);
        }
    }


    /// <summary>
    /// Marks a user as the season champion.
    /// </summary>
    /// <param name="user">User who won the season championship.</param>
    public void CrownChampion(
        User user)
    {
        ArgumentNullException.ThrowIfNull(user);


        var standing = _standings
            .FirstOrDefault(
                x => x.User == user);


        if (standing == null)
        {
            throw new InvalidOperationException(
                "User does not exist in season standings."
            );
        }


        foreach (var currentStanding in _standings)
        {
            currentStanding.RemoveChampionStatus();
        }


        standing.CrownChampion();
    }
    
    /// <summary>
    /// Gets the users who qualified for the playoffs ordered by seed.
    /// </summary>
    /// <returns>Playoff-qualified season standings ordered by rank.</returns>
    public IReadOnlyList<SeasonStanding> GetPlayoffQualifiers()
    {
        return _standings
            .Where(x => x.MadePlayoffs)
            .OrderBy(x => x.Rank)
            .ToList();
    }
    
}