using PickDuel.Domain.Entities;

namespace PickDuel.Tests.Domain;

public class LeagueTests
{
    [Test]
    public void AddMember_ShouldIncreaseMemberCount()
    {
        // Arrange
        var league = new League("Test League");

        // Act
        league.AddMember();

        // Assert
        Assert.That(league.memberCount == 2);
    }

    [Test]
    public void AddMember_ShouldThrowException_WhenMaxMembersReached()
    {
        // Arrange
        var league = new League("Test League");
        for (int i = 1; i < 32; i++)
        {
            league.AddMember();
        }

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => league.AddMember());
    }

    [Test]
    public void League_ShouldInitializeWithCorrectValues()
    {
        // Arrange
        var leagueName = "Test League";
        var league = new League(leagueName);

        // Act & Assert
        Assert.That(leagueName, Is.EqualTo("Test League"));
        Assert.That(league, Is.Not.Null);
        Assert.That(league, Is.InstanceOf<League>());
        Assert.That(league.memberCount, Is.EqualTo(1));
        Assert.That(league.createdAt <= DateTime.UtcNow);
    }
}