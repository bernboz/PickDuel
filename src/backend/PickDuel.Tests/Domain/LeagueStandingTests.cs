using NUnit.Framework;
using PickDuel.Domain.Entities;
using PickDuel.Domain.Enums;

namespace PickDuel.Tests.Domain;

public class LeagueStandingTests
{
    [Test]
    public void NewLeagueStanding_ShouldStartWithNoPoints()
    {
        // Arrange
        var user = CreateUser();
        var league = CreateLeague();

        // Act
        var standing = new LeagueStanding(
            user,
            league
        );

        // Assert
        Assert.That(standing.TotalPoints, Is.Zero);
    }


    [Test]
    public void AddPoints_ShouldUpdateStandingTotal()
    {
        // Arrange
        var standing = CreateStanding();

        // Act
        standing.AddPoints(25);

        // Assert
        Assert.That(standing.TotalPoints, Is.EqualTo(25));
    }


    [Test]
    public void AddPoints_ShouldAllowPenaltiesToReduceRanking()
    {
        // Arrange
        var standing = CreateStanding();

        standing.AddPoints(20);

        // Act
        standing.AddPoints(-5);

        // Assert
        Assert.That(standing.TotalPoints, Is.EqualTo(15));
    }


    [Test]
    public void AddPoints_ShouldAllowMultipleScoringEvents()
    {
        // Arrange
        var standing = CreateStanding();

        // Act
        standing.AddPoints(5);
        standing.AddPoints(10);
        standing.AddPoints(-2);

        // Assert
        Assert.That(standing.TotalPoints, Is.EqualTo(13));
    }


    [Test]
    public void CreatingStanding_ShouldRequireUser()
    {
        // Arrange
        var league = CreateLeague();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new LeagueStanding(
                null!,
                league
            ));
    }


    [Test]
    public void CreatingStanding_ShouldRequireLeague()
    {
        // Arrange
        var user = CreateUser();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new LeagueStanding(
                user,
                null!
            ));
    }


    private static LeagueStanding CreateStanding()
    {
        return new LeagueStanding(
            CreateUser(),
            CreateLeague()
        );
    }


    private static User CreateUser()
    {
        return new User(
            "Bob",
            "Smith",
            "bob@test.com",
            "bob"
        );
    }


    private static League CreateLeague()
    {
        return new League(
            "NFL League",
            SportType.NFL,
            CreateUser()
        );
    }
}