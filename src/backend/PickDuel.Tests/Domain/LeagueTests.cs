using PickDuel.Domain.Entities;
using PickDuel.Domain.Enums;

namespace PickDuel.Tests.Domain;

public class LeagueTests
{
    [Test]
    public void AddMember_ShouldIncreaseMemberCount()
    {
        // Arrange
        var owner = new User(
            "Bob",
            "Smith",
            "bob@test.com",
            "bob"
        );
        var newMember = new User(
            "John",
            "Doe", 
            "John@test.com", 
            "bob");
        var league = new League("Test League", SportType.NBA, owner);

        // Act
        league.AddMember(newMember);

        // Assert
        Assert.That(league.Members.Count == 2);
    }

    [Test]
    public void AddMember_ShouldThrowException_WhenMaxMembersReached()
    {
        // Arrange
        var owner = new User(
            "Bob",
            "Smith",
            "bob@test.com",
            "bob"
        );
        var newMember = new User(
            "John",
            "Doe", 
            "John@test.com", 
            "bob");
        var league = new League("Test League", SportType.NBA, owner);
        for (int i = 1; i < 32; i++)
        {
            league.AddMember(new User("Bob" + i, "Smith" + i, "bob@test.com" + i, "bob" + i));
        }

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => league.AddMember(newMember));
    }

    [Test]
    public void League_ShouldInitializeWithCorrectValues()
    {
        // Arrange
        var owner = new User(
            "Bob",
            "Smith",
            "bob@test.com",
            "bob"
        );
        var leagueName = "Test League";
        var league = new League(
            leagueName, 
            SportType.NBA,
            owner);

        // Act & Assert
        Assert.That(leagueName, Is.EqualTo("Test League"));
        Assert.That(league, Is.Not.Null);
        Assert.That(league, Is.InstanceOf<League>());
        Assert.That(league.Members.Count, Is.EqualTo(1));
        Assert.That(league.CreatedAt <= DateTime.UtcNow);
    }
    
    [Test]
    public void League_ShouldStoreCorrectSport()
    {
        var owner = new User(
            "Bob",
            "Smith",
            "bob@test.com",
            "bob"
        );
        var league = new League(
            "NFL League",
            SportType.NFL,
            owner
        );

        Assert.That(league.Sport, Is.EqualTo(SportType.NFL));
    }
    
    [Test]
    public void NewLeague_ShouldAddOwnerAsFirstMember()
    {
        var owner = new User(
            "Bob",
            "Smith",
            "bob@test.com",
            "bob"
        );

        var league = new League(
            "Test League",
            SportType.NFL,
            owner
        );

        Assert.That(league.Members.Count, Is.EqualTo(1));
        Assert.That(league.Members.First(), Is.EqualTo(owner));
    }
    
    [Test]
    public void AddMember_ShouldThrowException_WhenUserAlreadyExists()
    {
        var owner = new User(
            "Bob",
            "Smith",
            "bob@test.com",
            "bob"
        );

        var league = new League(
            "Test League",
            SportType.NFL,
            owner
        );

        Assert.Throws<InvalidOperationException>(() =>
            league.AddMember(owner)
        );
    }
}