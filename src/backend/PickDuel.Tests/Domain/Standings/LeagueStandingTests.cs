using NUnit.Framework;
using PickDuel.Domain.Entities;
using PickDuel.Tests.Common;
using PickDuel.Domain.Entities.Standings;

namespace PickDuel.Tests.Domain;

public class LeagueStandingTests
{
    [Test]
    public void NewLeagueStanding_ShouldInitializeCorrectly()
    {
        var user = TestDataFactory.CreateUser();
        var league = TestDataFactory.CreateLeague(user);

        var standing = new LeagueStanding(
            user,
            league
        );

        Assert.Multiple(() =>
        {
            Assert.That(standing.User, Is.EqualTo(user));
            Assert.That(standing.League, Is.EqualTo(league));

            Assert.That(standing.TotalPoints, Is.Zero);
            Assert.That(standing.MatchupWins, Is.Zero);
            Assert.That(standing.MatchupLosses, Is.Zero);
        });
    }


    [Test]
    public void AddPoints_ShouldIncreaseTotalPoints()
    {
        var standing = CreateStanding();

        standing.AddPoints(50);

        Assert.That(
            standing.TotalPoints,
            Is.EqualTo(50)
        );
    }


    [Test]
    public void AddPoints_ShouldSupportPenalties()
    {
        var standing = CreateStanding();

        standing.AddPoints(100);
        standing.AddPoints(-40);

        Assert.That(
            standing.TotalPoints,
            Is.EqualTo(60)
        );
    }


    [Test]
    public void AddPoints_ShouldAccumulateMultipleScoringEvents()
    {
        var standing = CreateStanding();

        standing.AddPoints(25);
        standing.AddPoints(50);
        standing.AddPoints(-10);
        standing.AddPoints(5);

        Assert.That(
            standing.TotalPoints,
            Is.EqualTo(70)
        );
    }


    [Test]
    public void RecordMatchupWin_ShouldIncreaseMatchupWins()
    {
        var standing = CreateStanding();

        standing.RecordMatchupWin();

        Assert.That(
            standing.MatchupWins,
            Is.EqualTo(1)
        );
    }


    [Test]
    public void RecordMatchupLoss_ShouldIncreaseMatchupLosses()
    {
        var standing = CreateStanding();

        standing.RecordMatchupLoss();

        Assert.That(
            standing.MatchupLosses,
            Is.EqualTo(1)
        );
    }


    [Test]
    public void RecordMultipleMatchupResults_ShouldMaintainCorrectRecord()
    {
        var standing = CreateStanding();

        standing.RecordMatchupWin();
        standing.RecordMatchupWin();
        standing.RecordMatchupLoss();
        standing.RecordMatchupWin();
        standing.RecordMatchupLoss();

        Assert.Multiple(() =>
        {
            Assert.That(
                standing.MatchupWins,
                Is.EqualTo(3)
            );

            Assert.That(
                standing.MatchupLosses,
                Is.EqualTo(2)
            );
        });
    }


    [Test]
    public void CreatingStanding_ShouldThrow_WhenUserIsNull()
    {
        var league = TestDataFactory.CreateLeague(
            TestDataFactory.CreateUser()
        );

        Assert.Throws<ArgumentNullException>(() =>
            new LeagueStanding(
                null!,
                league
            ));
    }


    [Test]
    public void CreatingStanding_ShouldThrow_WhenLeagueIsNull()
    {
        var user = TestDataFactory.CreateUser();

        Assert.Throws<ArgumentNullException>(() =>
            new LeagueStanding(
                user,
                null!
            ));
    }


    private static LeagueStanding CreateStanding()
    {
        var user = TestDataFactory.CreateUser();
        var league = TestDataFactory.CreateLeague(user);

        return new LeagueStanding(
            user,
            league
        );
    }
}