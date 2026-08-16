using PickDuel.Domain.Enums;

namespace PickDuel.Domain.Entities;

public class League
{
    
    private const int MAX_MEMBERS = 32;

    public string Name { get; private set; }
    public Guid Id { get; private set; }
    private readonly List<User> _members = new();
    public IReadOnlyCollection<User> Members => _members;    
    public DateTime CreatedAt { get; private set; }
    
    public SportType Sport { get; private set; }

    /// <summary>
    /// Initializes a new instance of the League class with the specified name.
    /// </summary>
    /// <param name="name">The name of the league.</param>
    /// <param name="sport">The type of sport</param>
    /// <param name="owner">The creator of the league</param>
    public League(string name, SportType sport, User owner)
    {
        this.Name = name;
        this.Sport = sport;
        this.Id = Guid.NewGuid();
        this.CreatedAt = DateTime.UtcNow;
        
        _members.Add(owner);
    }

    /// <summary>
    /// Adds a member to the league. Throws an exception if the maximum number of members is reached.
    /// </summary>
    public void AddMember(User user)
    {
        if (_members.Count >= MAX_MEMBERS)
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
}