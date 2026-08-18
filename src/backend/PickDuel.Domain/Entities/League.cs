using PickDuel.Domain.Common;
using PickDuel.Domain.Enums;
using PickDuel.Domain.ValueObjects;

namespace PickDuel.Domain.Entities;

public class League : Entity
{
    public const int MaxMembers = 32;


    public string Name { get; private set; }


    private readonly List<User> _members = new();

    public IReadOnlyCollection<User> Members => _members.AsReadOnly();


    public DateTime CreatedAt { get; private set; }


    public SportType Sport { get; private set; }


    public User Owner { get; private set; }


    public ScoringSettings ScoringSettings { get; private set; }


    /// <summary>
    /// Initializes a new league.
    /// </summary>
    /// <param name="name">League display name.</param>
    /// <param name="sport">Sport type used by the league.</param>
    /// <param name="owner">User who created the league.</param>
    /// <param name="scoringSettings">Optional scoring configuration.</param>
    public League(
        string name,
        SportType sport,
        User owner,
        ScoringSettings? scoringSettings = null)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(
                "League name is required.",
                nameof(name)
            );
        }

        ArgumentNullException.ThrowIfNull(owner);


        Name = name;
        Sport = sport;
        Owner = owner;

        ScoringSettings = scoringSettings ?? CreateDefaultScoringSettings();

        CreatedAt = DateTime.UtcNow;

        _members.Add(owner);
    }


    /// <summary>
    /// Adds a user to the league.
    /// </summary>
    public void AddMember(User user)
    {
        ArgumentNullException.ThrowIfNull(user);


        if (_members.Count >= MaxMembers)
        {
            throw new InvalidOperationException(
                "League has reached the maximum number of members."
            );
        }


        if (_members.Contains(user))
        {
            throw new InvalidOperationException(
                "User is already a member of this league."
            );
        }


        _members.Add(user);
    }


    /// <summary>
    /// Updates the scoring configuration for this league.
    /// </summary>
    public void UpdateScoringSettings(
        ScoringSettings scoringSettings)
    {
        ArgumentNullException.ThrowIfNull(scoringSettings);

        ScoringSettings = scoringSettings;
    }


    /// <summary>
    /// Transfers ownership to another existing league member.
    /// </summary>
    public void TransferOwnership(
        User newOwner)
    {
        ArgumentNullException.ThrowIfNull(newOwner);


        if (!_members.Contains(newOwner))
        {
            throw new InvalidOperationException(
                "New owner must already be a member of the league."
            );
        }


        if (ReferenceEquals(Owner, newOwner))
        {
            throw new InvalidOperationException(
                "User is already the owner of the league."
            );
        }


        Owner = newOwner;
    }

    private static ScoringSettings CreateDefaultScoringSettings()
    {
        return new ScoringSettings(
            winnerPoints: 10,
            exactScorePoints: 50,
            scoreAccuracyBonus: 25,
            scoreAccuracyPenalty: -50,
            scoreTolerance: 5,
            maxScoreDifferencePenalty: 10
        );
    }
}