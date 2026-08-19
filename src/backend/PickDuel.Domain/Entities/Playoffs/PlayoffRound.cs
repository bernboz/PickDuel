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
            throw new ArgumentOutOfRangeException(nameof(roundNumber));
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
    /// <param name="matchup">Playoff matchup being added.</param>
    public void AddMatchup(PlayoffMatchup matchup)
    {
        ArgumentNullException.ThrowIfNull(matchup);

        if (matchup.Round != this)
        {
            throw new InvalidOperationException(
                "Matchup must belong to this round."
            );
        }

        if (IsCompleted)
        {
            throw new InvalidOperationException(
                "Cannot add matchups after round completion."
            );
        }

        if (_matchups.Contains(matchup))
        {
            throw new InvalidOperationException(
                "Matchup already exists in this round."
            );
        }

        _matchups.Add(matchup);
    }


    /// <summary>
    /// Retrieves the users who won this playoff round.
    /// </summary>
    /// <returns>The users advancing to the next playoff round.</returns>
    public IReadOnlyCollection<User> GetWinners()
    {
        if (!IsCompleted)
        {
            throw new InvalidOperationException(
                "Round must be completed first."
            );
        }

        if (_matchups.Any(x => x.Winner == null))
        {
            throw new InvalidOperationException(
                "All matchups must have winners before advancing."
            );
        }

        return _matchups
            .Select(x => x.Winner!)
            .ToList()
            .AsReadOnly();
    }


    /// <summary>
    /// Marks the playoff round as completed once all matchups have winners.
    /// </summary>
    public void Complete()
    {
        if (_matchups.Count == 0)
        {
            throw new InvalidOperationException(
                "Round must contain matchups."
            );
        }

        if (_matchups.Any(x => !x.IsCompleted))
        {
            throw new InvalidOperationException(
                "All matchups must be completed."
            );
        }

        IsCompleted = true;
    }
}