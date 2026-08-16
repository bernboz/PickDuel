namespace PickDuel.Domain.Entities;

public class League
{
    private const int MAX_MEMBERS = 32;

    public string name { get; private set; }
    public Guid id { get; private set; }
    public int memberCount { get; private set; }
    public DateTime createdAt { get; private set; }

    /// <summary>
    /// Initializes a new instance of the League class with the specified name.
    /// </summary>
    /// <param name="name">The name of the league.</param>
    public League(string name)
    {
        this.name = name;
        this.id = Guid.NewGuid();
        this.memberCount = 1;
        this.createdAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Adds a member to the league. Throws an exception if the maximum number of members is reached.
    /// </summary>
    public void AddMember()
    {
        if (memberCount >= MAX_MEMBERS)
        {
            throw new InvalidOperationException("Cannot add more members to the league.");
        }
        memberCount++;
    }
}