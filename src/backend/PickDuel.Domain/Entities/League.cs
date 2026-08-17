using PickDuel.Domain.Enums;
using PickDuel.Domain.Common;
using PickDuel.Domain.ValueObjects;

namespace PickDuel.Domain.Entities;

public class League : Entity
{
    
    private const int MaxMembers = 32;

    public string Name { get; private set; }
    private readonly List<User> _members = new();
    public IReadOnlyCollection<User> Members => _members;    
    public DateTime CreatedAt { get; private set; }
    public SportType Sport { get; private set; }
    public User Owner { get; private set; }
    public ScoringSettings ScoringSettings { get; private set; }

    /// <summary>
    /// Initializes a new instance of the League class with the specified name.
    /// </summary>
    /// <param name="name">The name of the league.</param>
    /// <param name="sport">The type of sport</param>
    /// <param name="owner">The creator of the league</param>
    /// <param name="scoringSettings">Scoring settings for the league</param>
    public League(string name, SportType sport, User owner, ScoringSettings? scoringSettings = null)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(
                "League name is required.",
                nameof(name));
        }

        if (owner == null)
        {
            throw new ArgumentNullException(nameof(owner));
        }

        Name = name;
        Sport = sport;
        Owner = owner;

        ScoringSettings = scoringSettings ??
                          new ScoringSettings(
                              winnerPoints: 1,
                              exactScorePoints: 5);

        CreatedAt = DateTime.UtcNow;

        _members.Add(owner);
    }

    /// <summary>
    /// Adds a member to the league. Throws an exception if the maximum number of members is reached.
    /// </summary>
    public void AddMember(User user)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        
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
    /// Updates the league scoring settings.
    /// </summary>
    public void UpdateScoringSettings(ScoringSettings scoringSettings)
    {
        if (scoringSettings == null)
        {
            throw new ArgumentNullException(nameof(scoringSettings));
        }

        ScoringSettings = scoringSettings;
    }
    
    /// <summary>
    /// Transfers league ownership to another member.
    /// </summary>
    public void TransferOwnership(User newOwner)
    {
        if (newOwner == null)
        {
            throw new ArgumentNullException(nameof(newOwner));
        }

        if (!_members.Contains(newOwner))
        {
            throw new InvalidOperationException(
                "New owner must already be a member of the league.");
        }

        if (Owner == newOwner)
        {
            throw new InvalidOperationException(
                "User is already the owner of the league.");
        }

        Owner = newOwner;
    }
}