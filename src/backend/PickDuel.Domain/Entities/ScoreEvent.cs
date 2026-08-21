using PickDuel.Domain.Common;
using PickDuel.Domain.Enums;

namespace PickDuel.Domain.Entities;

public class ScoreEvent : Entity
{
    public User User { get; private set; }

    public League League { get; private set; }

    public int Points { get; private set; }

    public ScoreEventType Type { get; private set; }

    public string Description { get; private set; }

    public Pick? Pick { get; private set; }

    public DateTime CreatedAt { get; private set; }


    /// <summary>
    /// Creates a score event representing a scoring change within a league.
    /// </summary>
    /// <param name="user">User receiving the score event.</param>
    /// <param name="league">League where the event occurred.</param>
    /// <param name="points">Points awarded or deducted.</param>
    /// <param name="type">Category of scoring event.</param>
    /// <param name="description">Human-readable explanation of the event.</param>
    /// <param name="pick">Pick that generated the score event, if applicable.</param>
    public ScoreEvent(User user, League league, int points, ScoreEventType type, string description, Pick? pick = null)
    {
        ArgumentNullException.ThrowIfNull(user);
        ArgumentNullException.ThrowIfNull(league);

        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException(
                "Description cannot be empty.",
                nameof(description)
            );
        }

        if (type == ScoreEventType.Penalty && points >= 0)
        {
            throw new ArgumentException(
                "Penalty events must have negative points.",
                nameof(points)
            );
        }

        if (type != ScoreEventType.Penalty && points < 0)
        {
            throw new ArgumentException(
                "Only penalty events can have negative points.",
                nameof(points)
            );
        }

        User = user;
        League = league;
        Points = points;
        Type = type;
        Description = description;
        Pick = pick;
        CreatedAt = DateTime.UtcNow;
    }
}