using PickDuel.Domain.Enums;

namespace PickDuel.Domain.ValueObjects;

public class ScoringResult
{
    public int Points { get; }

    public ScoreEventType Type { get; }

    public string Description { get; }


    /// <summary>
    /// Creates a result produced by the scoring calculation process.
    /// </summary>
    /// <param name="points">
    /// Points awarded or deducted from the pick evaluation.
    /// </param>
    /// <param name="type">
    /// Category of scoring event produced.
    /// </param>
    /// <param name="description">
    /// Human-readable explanation of the scoring result.
    /// </param>
    public ScoringResult(int points, ScoreEventType type, string description)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException("Description cannot be empty.", nameof(description));
        }

        if (type == ScoreEventType.Penalty && points >= 0)
        {
            throw new ArgumentException("Penalty events must have negative points.", nameof(points));
        }

        if (type != ScoreEventType.Penalty && points < 0)
        {
            throw new ArgumentException("Only penalty events can have negative points.", nameof(points));
        }

        Points = points;
        Type = type;
        Description = description;
    }
}