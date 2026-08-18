using PickDuel.Domain.Common;
using PickDuel.Domain.Entities.Matchups;

namespace PickDuel.Domain.Entities;

public class LeagueSeason : Entity
{
    public League League { get; private set; }


    public string Name { get; private set; }


    public int Year { get; private set; }


    private readonly List<LeagueMatchup> _matchups = new();

    public IReadOnlyCollection<LeagueMatchup> Matchups =>
        _matchups.AsReadOnly();


    public int MatchupCount =>
        _matchups.Count;


    public DateTime StartDate { get; private set; }

    public DateTime EndDate { get; private set; }


    public bool IsCompleted { get; private set; }


    public DateTime CreatedAt { get; private set; }


    /// <summary>
    /// Initializes a league season that represents a historical competition period.
    /// </summary>
    /// <param name="league">League that this season belongs to.</param>
    /// <param name="name">Display name of the season.</param>
    /// <param name="year">Primary calendar year associated with the season.</param>
    /// <param name="startDate">Date when the season begins.</param>
    /// <param name="endDate">Date when the season ends.</param>
    public LeagueSeason(
        League league,
        string name,
        int year,
        DateTime startDate,
        DateTime endDate)
    {
        ArgumentNullException.ThrowIfNull(league);


        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(
                "Season name cannot be empty.",
                nameof(name)
            );
        }


        if (year <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(year),
                "Season year must be valid."
            );
        }


        if (startDate >= endDate)
        {
            throw new ArgumentException(
                "Season start date must be before end date."
            );
        }


        League = league;

        Name = name;

        Year = year;

        StartDate = startDate;

        EndDate = endDate;


        IsCompleted = false;


        CreatedAt = DateTime.UtcNow;
    }


    /// <summary>
    /// Adds a matchup associated with this league season.
    /// </summary>
    /// <param name="matchup">League matchup being added to the season.</param>
    public void AddMatchup(
        LeagueMatchup matchup)
    {
        ArgumentNullException.ThrowIfNull(matchup);


        if (IsCompleted)
        {
            throw new InvalidOperationException(
                "Cannot add matchups to a completed season."
            );
        }


        if (matchup.League != League)
        {
            throw new InvalidOperationException(
                "Matchup must belong to this league."
            );
        }


        if (_matchups.Contains(matchup))
        {
            throw new InvalidOperationException(
                "Matchup already exists in this season."
            );
        }


        _matchups.Add(matchup);
    }


    /// <summary>
    /// Marks this league season as completed.
    /// </summary>
    public void Complete()
    {
        if (IsCompleted)
        {
            throw new InvalidOperationException(
                "Season is already completed."
            );
        }


        IsCompleted = true;
    }
}