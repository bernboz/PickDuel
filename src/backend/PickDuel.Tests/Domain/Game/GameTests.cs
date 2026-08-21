using NUnit.Framework;
using PickDuel.Domain.Entities;
using PickDuel.Tests.Common;

namespace PickDuel.Tests.Domain.Game;

public class GameTests
{
    [Test]
    public void NewGame_ShouldInitializeCorrectly()
    {
        var startTime = DateTime.UtcNow.AddDays(1);
        var endTime = DateTime.UtcNow.AddDays(1).AddHours(3);

        var game = new PickDuel.Domain.Entities.Game(
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
            new PickDuel.Domain.Entities.Game(
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
            new PickDuel.Domain.Entities.Game(
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
            new PickDuel.Domain.Entities.Game(
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
            new PickDuel.Domain.Entities.Game(
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
        var game = new PickDuel.Domain.Entities.Game(
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
        var game = new PickDuel.Domain.Entities.Game(
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
    
        [Test]
    public void NewGame_ShouldHaveNoResult_WhenCreated()
    {
        var game = TestDataFactory.CreateGame();

        Assert.Multiple(() =>
        {
            Assert.That(game.HomeScore, Is.Null);
            Assert.That(game.AwayScore, Is.Null);
            Assert.That(game.WinningTeam, Is.Null);
            Assert.That(game.IsCompleted, Is.False);
        });
    }


    [Test]
    public void CompleteGame_ShouldSetFinalScoreAndWinner_WhenHomeTeamWins()
    {
        var game = TestDataFactory.CreateGame();

        game.CompleteGame(24, 17);

        Assert.Multiple(() =>
        {
            Assert.That(game.HomeScore, Is.EqualTo(24));
            Assert.That(game.AwayScore, Is.EqualTo(17));
            Assert.That(game.WinningTeam, Is.EqualTo(game.HomeTeam));
            Assert.That(game.IsCompleted, Is.True);
        });
    }


    [Test]
    public void CompleteGame_ShouldSetFinalScoreAndWinner_WhenAwayTeamWins()
    {
        var game = TestDataFactory.CreateGame();

        game.CompleteGame(14, 28);

        Assert.Multiple(() =>
        {
            Assert.That(game.HomeScore, Is.EqualTo(14));
            Assert.That(game.AwayScore, Is.EqualTo(28));
            Assert.That(game.WinningTeam, Is.EqualTo(game.AwayTeam));
            Assert.That(game.IsCompleted, Is.True);
        });
    }


    [Test]
    public void CompleteGame_ShouldHaveNoWinner_WhenGameEndsInTie()
    {
        var game = TestDataFactory.CreateGame();

        game.CompleteGame(21, 21);

        Assert.Multiple(() =>
        {
            Assert.That(game.HomeScore, Is.EqualTo(21));
            Assert.That(game.AwayScore, Is.EqualTo(21));
            Assert.That(game.WinningTeam, Is.Null);
            Assert.That(game.IsCompleted, Is.True);
        });
    }


    [Test]
    public void CompleteGame_ShouldThrow_WhenGameHasAlreadyBeenCompleted()
    {
        var game = TestDataFactory.CreateGame();

        game.CompleteGame(24, 17);

        Assert.Throws<InvalidOperationException>(() =>
            game.CompleteGame(30, 20)
        );
    }


    [Test]
    public void CompleteGame_ShouldThrow_WhenHomeScoreIsNegative()
    {
        var game = TestDataFactory.CreateGame();

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            game.CompleteGame(-1, 20)
        );
    }


    [Test]
    public void CompleteGame_ShouldThrow_WhenAwayScoreIsNegative()
    {
        var game = TestDataFactory.CreateGame();

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            game.CompleteGame(20, -1)
        );
    }
}