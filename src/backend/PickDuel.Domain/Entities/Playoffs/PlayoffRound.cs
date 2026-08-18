using PickDuel.Domain.Common;

namespace PickDuel.Domain.Entities.Playoffs;

public class PlayoffRound : Entity
{
    public PlayoffBracket Bracket { get; private set; }


    public string Name { get; private set; }


    public int RoundNumber { get; private set; }


    private readonly List<PlayoffMatchup> _matchups = new();

    public IReadOnlyCollection<PlayoffMatchup> Matchups =>
        _matchups.AsReadOnly();


    public bool IsCompleted { get; private set; }


    public DateTime CreatedAt { get; private set; }


    public PlayoffRound(PlayoffBracket bracket, string name, int roundNumber)
    {
        ArgumentNullException.ThrowIfNull(bracket);

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(
                "Round name cannot be empty.",
                nameof(name)
            );
        }

        if (roundNumber < 1)
        {
            throw new ArgumentOutOfRangeException(
                nameof(roundNumber)
            );
        }

        Bracket = bracket;
        Name = name;
        RoundNumber = roundNumber;

        IsCompleted = false;

        CreatedAt = DateTime.UtcNow;
    }


    /// <summary>
    /// Adds a playoff matchup to this round.
    /// </summary>
    public void AddMatchup(PlayoffMatchup matchup)
    {
        ArgumentNullException.ThrowIfNull(matchup);

        if (_matchups.Contains(matchup))
        {
            throw new InvalidOperationException(
                "Playoff matchup already exists in this round."
            );
        }

        _matchups.Add(matchup);
    }


    /// <summary>
    /// Marks the round as completed once all matchups are finished.
    /// </summary>
    public void Complete()
    {
        if (_matchups.Any(x => !x.IsCompleted))
        {
            throw new InvalidOperationException(
                "Cannot complete round with unfinished matchups."
            );
        }

        IsCompleted = true;
    }
}