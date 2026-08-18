using NUnit.Framework;
using PickDuel.Domain.Entities;
using PickDuel.Tests.Common;

namespace PickDuel.Tests.Domain;

public class GameOddsTests
{
    [Test]
    public void NewGameOdds_ShouldInitializeCorrectly()
    {
        var game = TestDataFactory.CreateGame();

        var odds = new GameOdds(
            game,
            0.75m,
            0.25m
        );

        Assert.Multiple(() =>
        {
            Assert.That(odds.Game, Is.EqualTo(game));
            Assert.That(odds.HomeWinProbability, Is.EqualTo(0.75m));
            Assert.That(odds.AwayWinProbability, Is.EqualTo(0.25m));
            Assert.That(odds.IsLocked, Is.False);
            Assert.That(odds.LockedAt, Is.Null);
        });
    }


    [Test]
    public void NewGameOdds_ShouldThrow_WhenGameIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new GameOdds(
                null!,
                0.75m,
                0.25m
            ));
    }


    [Test]
    public void NewGameOdds_ShouldThrow_WhenProbabilitiesDoNotEqualOne()
    {
        var game = TestDataFactory.CreateGame();

        Assert.Throws<ArgumentException>(() =>
            new GameOdds(
                game,
                0.60m,
                0.60m
            ));
    }


    [Test]
    public void NewGameOdds_ShouldThrow_WhenHomeProbabilityIsInvalid()
    {
        var game = TestDataFactory.CreateGame();

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new GameOdds(
                game,
                0m,
                1m
            ));
    }


    [Test]
    public void NewGameOdds_ShouldThrow_WhenAwayProbabilityIsInvalid()
    {
        var game = TestDataFactory.CreateGame();

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new GameOdds(
                game,
                1m,
                0m
            ));
    }


    [Test]
    public void NewGameOdds_ShouldAllowValidProbabilityBoundaries()
    {
        var game = TestDataFactory.CreateGame();

        var odds = new GameOdds(
            game,
            0.99m,
            0.01m
        );

        Assert.Multiple(() =>
        {
            Assert.That(odds.HomeWinProbability, Is.EqualTo(0.99m));
            Assert.That(odds.AwayWinProbability, Is.EqualTo(0.01m));
        });
    }


    [Test]
    public void Lock_ShouldLockOddsAndSetLockedTimestamp()
    {
        var odds = CreateOdds();

        odds.Lock();

        Assert.Multiple(() =>
        {
            Assert.That(odds.IsLocked, Is.True);
            Assert.That(odds.LockedAt, Is.Not.Null);
        });
    }


    [Test]
    public void Lock_ShouldPreserveProbabilities()
    {
        var odds = CreateOdds();

        odds.Lock();

        Assert.Multiple(() =>
        {
            Assert.That(
                odds.HomeWinProbability,
                Is.EqualTo(0.75m)
            );

            Assert.That(
                odds.AwayWinProbability,
                Is.EqualTo(0.25m)
            );
        });
    }


    [Test]
    public void Lock_ShouldThrow_WhenOddsAreAlreadyLocked()
    {
        var odds = CreateOdds();

        odds.Lock();

        Assert.Throws<InvalidOperationException>(() =>
            odds.Lock()
        );
    }


    [Test]
    public void Lock_ShouldNotChangeLockedTimestamp()
    {
        var odds = CreateOdds();

        odds.Lock();

        var lockedTime = odds.LockedAt;

        Assert.Throws<InvalidOperationException>(() =>
            odds.Lock()
        );

        Assert.That(
            odds.LockedAt,
            Is.EqualTo(lockedTime)
        );
    }


    private static GameOdds CreateOdds()
    {
        return new GameOdds(
            TestDataFactory.CreateGame(),
            0.75m,
            0.25m
        );
    }
}