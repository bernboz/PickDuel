using PickDuel.Domain.Common;

namespace PickDuel.Domain.Entities.Playoffs;

public class PlayoffBracket : Entity
{
    public LeagueSeason Season { get; private set; }


    private readonly List<PlayoffRound> _rounds = new();

    public IReadOnlyCollection<PlayoffRound> Rounds =>
        _rounds.AsReadOnly();


    public User? Champion { get; private set; }


    public bool IsCompleted { get; private set; }


    public DateTime CreatedAt { get; private set; }


    public PlayoffBracket(LeagueSeason season)
    {
        ArgumentNullException.ThrowIfNull(season);

        Season = season;

        IsCompleted = false;

        CreatedAt = DateTime.UtcNow;
    }


    /// <summary>
    /// Generates the opening playoff round from the qualified season standings.
    /// </summary>
    /// <param name="standings">Season standings containing playoff qualifiers.</param>
    public void GenerateBracket(SeasonStandings standings)
    {
        ArgumentNullException.ThrowIfNull(standings);

        if (standings.Season != Season)
        {
            throw new InvalidOperationException(
                "Standings must belong to this season."
            );
        }

        if (!Season.IsCompleted)
        {
            throw new InvalidOperationException(
                "Cannot generate a playoff bracket until the season has ended."
            );
        }

        if (_rounds.Count > 0)
        {
            throw new InvalidOperationException(
                "Playoff bracket has already been generated."
            );
        }

        var qualifiers = standings.GetPlayoffQualifiers();

        if (qualifiers.Count < 2)
        {
            throw new InvalidOperationException(
                "At least two playoff qualifiers are required."
            );
        }

        string roundName = qualifiers.Count switch
        {
            2 => "Championship",
            4 => "Semifinals",
            8 => "Quarterfinals",
            16 => "Round of 16",
            _ => "Playoffs"
        };

        var round = new PlayoffRound(
            this,
            roundName,
            1
        );

        for (int i = 0; i < qualifiers.Count / 2; i++)
        {
            var higherSeed = qualifiers.ElementAt(i);
            var lowerSeed = qualifiers.ElementAt(qualifiers.Count - 1 - i);

            round.AddMatchup(
                new PlayoffMatchup(
                    round,
                    higherSeed.User,
                    lowerSeed.User
                )
            );
        }

        AddRound(round);
    }


    /// <summary>
    /// Adds a playoff round to the bracket.
    /// </summary>
    /// <param name="round">Playoff round to add.</param>
    public void AddRound(PlayoffRound round)
    {
        ArgumentNullException.ThrowIfNull(round);

        if (round.Bracket != this)
        {
            throw new InvalidOperationException(
                "Round must belong to this playoff bracket."
            );
        }

        if (_rounds.Contains(round))
        {
            throw new InvalidOperationException(
                "Playoff round already exists in this bracket."
            );
        }

        _rounds.Add(round);
    }


    /// <summary>
    /// Sets the playoff champion and completes the bracket.
    /// </summary>
    /// <param name="champion">User who won the championship.</param>
    public void SetChampion(User champion)
    {
        ArgumentNullException.ThrowIfNull(champion);

        Champion = champion;

        IsCompleted = true;
    }


    /// <summary>
    /// Marks the playoff bracket as completed.
    /// </summary>
    public void Complete()
    {
        if (Champion == null)
        {
            throw new InvalidOperationException(
                "Cannot complete playoff bracket without a champion."
            );
        }

        IsCompleted = true;
    }
    
}