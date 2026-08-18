using NUnit.Framework;
using PickDuel.Domain.Entities;
using PickDuel.Tests.Common;

namespace PickDuel.Tests.Domain;

public class GameTests
{
    [Test]
    public void NewGame_ShouldInitializeCorrectly()
    {
        var startTime = DateTime.UtcNow.AddDays(1);
        var endTime = DateTime.UtcNow.AddDays(1).AddHours(3);

        var game = new Game(
            "Clemson",
            "Florida State",
            startTime,
            endTime
        );

        Assert.Multiple(() =>
        {
            Assert.That(game.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(game.HomeTeam, Is.EqualTo("Clemson"));
            Assert.That(game.AwayTeam, Is.EqualTo("Florida State"));
            Assert.That(game.StartTime, Is.EqualTo(startTime));
            Assert.That(game.EndTime, Is.EqualTo(endTime));
            Assert.That(game.HasStarted, Is.False);
        });
    }


    [Test]
    public void NewGame_ShouldThrow_WhenHomeTeamIsMissing()
    {
        Assert.Throws<ArgumentException>(() =>
            new Game(
                "",
                "Florida State",
                DateTime.UtcNow.AddHours(1),
                DateTime.UtcNow.AddHours(3)
            ));
    }


    [Test]
    public void NewGame_ShouldThrow_WhenAwayTeamIsMissing()
    {
        Assert.Throws<ArgumentException>(() =>
            new Game(
                "Clemson",
                "",
                DateTime.UtcNow.AddHours(1),
                DateTime.UtcNow.AddHours(3)
            ));
    }


    [Test]
    public void NewGame_ShouldThrow_WhenTeamsAreTheSame()
    {
        Assert.Throws<ArgumentException>(() =>
            new Game(
                "Clemson",
                "Clemson",
                DateTime.UtcNow.AddHours(1),
                DateTime.UtcNow.AddHours(3)
            ));
    }


    [Test]
    public void NewGame_ShouldThrow_WhenEndTimeOccursBeforeStartTime()
    {
        Assert.Throws<ArgumentException>(() =>
            new Game(
                "Chiefs",
                "Bills",
                DateTime.UtcNow.AddHours(3),
                DateTime.UtcNow.AddHours(1)
            ));
    }


    [Test]
    public void HasStarted_ShouldReturnFalse_WhenGameIsInFuture()
    {
        var game = TestDataFactory.CreateGame();

        Assert.That(
            game.HasStarted,
            Is.False
        );
    }


    [Test]
    public void HasStarted_ShouldReturnTrue_WhenGameStartTimeHasPassed()
    {
        var game = new Game(
            "Chiefs",
            "Bills",
            DateTime.UtcNow.AddHours(-4),
            DateTime.UtcNow.AddHours(-1)
        );

        Assert.That(
            game.HasStarted,
            Is.True
        );
    }


    [Test]
    public void HasStarted_ShouldReturnTrue_WhenGameStartsImmediately()
    {
        var game = new Game(
            "Chiefs",
            "Bills",
            DateTime.UtcNow,
            DateTime.UtcNow.AddHours(3)
        );

        Assert.That(
            game.HasStarted,
            Is.True
        );
    }
}