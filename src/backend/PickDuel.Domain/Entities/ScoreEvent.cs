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

    public DateTime CreatedAt { get; private set; }


    public ScoreEvent(User user, League league, int points, ScoreEventType type, string description)
    {
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        if (league is null)
        {
            throw new ArgumentNullException(nameof(league));
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException("Description cannot be empty.", nameof(description));
        }


        User = user;
        League = league;
        Points = points;
        Type = type;
        Description = description;
        CreatedAt = DateTime.UtcNow;
    }
}