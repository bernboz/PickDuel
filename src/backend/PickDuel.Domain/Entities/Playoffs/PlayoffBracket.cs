using PickDuel.Domain.Common;
using PickDuel.Domain.Entities.Standings;

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
        CreatedAt = DateTime.UtcNow;
    }


    /// <summary>
    /// Generates the opening playoff round from qualified season standings.
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
                "Season must be completed before generating playoffs."
            );
        }

        if (_rounds.Count > 0)
        {
            throw new InvalidOperationException(
                "Playoff bracket already exists."
            );
        }

        var qualifiers = standings.GetPlayoffQualifiers();

        if (qualifiers.Count < 2)
        {
            throw new InvalidOperationException(
                "At least two playoff qualifiers are required."
            );
        }

        var round = new PlayoffRound(
            this,
            qualifiers.Count switch
            {
                2 => "Championship",
                4 => "Semifinals",
                8 => "Quarterfinals",
                16 => "Round of 16",
                _ => "Opening Round"
            },
            1
        );


        for (int i = 0; i < qualifiers.Count / 2; i++)
        {
            round.AddMatchup(
                new PlayoffMatchup(
                    round,
                    qualifiers.ElementAt(i).User,
                    qualifiers.ElementAt(qualifiers.Count - 1 - i).User
                )
            );
        }

        AddRound(round);
    }


    /// <summary>
    /// Adds a playoff round to this bracket.
    /// </summary>
    public void AddRound(PlayoffRound round)
    {
        ArgumentNullException.ThrowIfNull(round);

        if (round.Bracket != this)
        {
            throw new InvalidOperationException(
                "Round must belong to this bracket."
            );
        }

        if (_rounds.Contains(round))
        {
            throw new InvalidOperationException(
                "Round already exists."
            );
        }

        _rounds.Add(round);
    }
    
    /// <summary>
    /// Creates the next playoff round from the previous round winners.
    /// </summary>
    /// <returns>The newly created playoff round.</returns>
    public PlayoffRound CreateNextRound()
    {
        if (_rounds.Count == 0)
        {
            throw new InvalidOperationException(
                "Cannot create next round before the bracket starts."
            );
        }

        var currentRound =
            _rounds.Last();

        if (!currentRound.IsCompleted)
        {
            throw new InvalidOperationException(
                "Current round must be completed before advancing."
            );
        }


        var winners =
            currentRound.GetWinners();


        if (winners.Count < 2)
        {
            throw new InvalidOperationException(
                "Not enough winners to create another round."
            );
        }


        var nextRoundNumber =
            currentRound.RoundNumber + 1;


        var nextRound =
            new PlayoffRound(
                this,
                GetRoundName(winners.Count),
                nextRoundNumber
            );


        for (int i = 0; i < winners.Count; i += 2)
        {
            nextRound.AddMatchup(
                new PlayoffMatchup(
                    nextRound,
                    winners.ElementAt(i),
                    winners.ElementAt(i + 1)
                )
            );
        }


        AddRound(nextRound);

        return nextRound;
    }

    private static string GetRoundName(int participantCount)
    {
        return participantCount switch
        {
            2 => "Championship",
            4 => "Semifinals",
            8 => "Quarterfinals",
            16 => "Round of 16",
            _ => "Playoff Round"
        };
    }

    /// <summary>
    /// Sets the playoff champion.
    /// </summary>
    public void SetChampion(User champion)
    {
        ArgumentNullException.ThrowIfNull(champion);

        if (!_rounds.Any(r =>
            r.Matchups.Any(m => m.Winner == champion)))
        {
            throw new InvalidOperationException(
                "Champion must have won a playoff matchup."
            );
        }

        Champion = champion;
    }


    /// <summary>
    /// Completes the playoff tournament.
    /// </summary>
    public void Complete()
    {
        if (Champion == null)
        {
            throw new InvalidOperationException(
                "Cannot complete playoffs without a champion."
            );
        }

        IsCompleted = true;
    }
}