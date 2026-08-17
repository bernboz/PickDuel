using NUnit.Framework;
using PickDuel.Domain.Entities;

namespace PickDuel.Tests.Domain;

public class GameOddsTests
{
    [Test]
    public void GameOdds_ShouldInitializeCorrectly()
    {
        var game = CreateGame();

        var odds = new GameOdds(
            game,
            0.75m,
            0.25m
        );

        Assert.That(odds.Game, Is.EqualTo(game));
        Assert.That(odds.HomeWinProbability, Is.EqualTo(0.75m));
        Assert.That(odds.AwayWinProbability, Is.EqualTo(0.25m));
        Assert.That(odds.IsLocked, Is.False);
    }


    [Test]
    public void GameOdds_ShouldThrow_WhenProbabilitiesDoNotEqualOne()
    {
        var game = CreateGame();

        Assert.Throws<ArgumentException>(() =>
            new GameOdds(
                game,
                0.60m,
                0.60m
            ));
    }
    
    [Test]
    public void Constructor_ShouldThrow_WhenHomeProbabilityIsInvalid()
    {
        var game = new Game(
            "Chiefs",
            "Bills",
            DateTime.UtcNow.AddHours(1),
            DateTime.UtcNow.AddHours(4)
        );


        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new GameOdds(
                game,
                0m,
                1m
            )
        );
    }


    [Test]
    public void Lock_ShouldLockOdds()
    {
        var odds = new GameOdds(
            CreateGame(),
            0.75m,
            0.25m
        );

        odds.Lock();

        Assert.That(odds.IsLocked, Is.True);
        Assert.That(odds.LockedAt, Is.Not.Null);
    }


    [Test]
    public void Lock_ShouldThrow_WhenAlreadyLocked()
    {
        var odds = new GameOdds(
            CreateGame(),
            0.75m,
            0.25m
        );

        odds.Lock();

        Assert.Throws<InvalidOperationException>(() =>
            odds.Lock()
        );
    }


    private static Game CreateGame()
    {
        return new Game(
            "Chiefs",
            "Bills",
            DateTime.UtcNow.AddHours(1),
            DateTime.UtcNow.AddHours(4)
        );
    }
}