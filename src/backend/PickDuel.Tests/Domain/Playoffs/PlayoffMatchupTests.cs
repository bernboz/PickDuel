using NUnit.Framework;
using PickDuel.Domain.Entities;
using PickDuel.Domain.Entities.Playoffs;
using PickDuel.Tests.Common;

namespace PickDuel.Tests.Domain.Playoffs;

public class PlayoffMatchupTests
{
    [Test]
    public void Constructor_ShouldInitializeCorrectly()
    {
        var matchup = CreateMatchup();

        Assert.Multiple(() =>
        {
            Assert.That(matchup.UserOne, Is.Not.Null);
            Assert.That(matchup.UserTwo, Is.Not.Null);
            Assert.That(matchup.IsCompleted, Is.False);
            Assert.That(matchup.Winner, Is.Null);
            Assert.That(matchup.CreatedAt,
                Is.LessThanOrEqualTo(DateTime.UtcNow));
        });
    }


    [Test]
    public void Constructor_ShouldThrow_WhenRoundIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new PlayoffMatchup(
                null!,
                TestDataFactory.CreateUser(),
                TestDataFactory.CreateUser()
            ));
    }


    [Test]
    public void Constructor_ShouldThrow_WhenUsersAreSame()
    {
        var user = TestDataFactory.CreateUser();

        Assert.Throws<ArgumentException>(() =>
            new PlayoffMatchup(
                CreateRound(),
                user,
                user
            ));
    }


    [Test]
    public void Complete_ShouldSetWinnerAndCompleteMatchup()
    {
        var matchup = CreateMatchup();

        var winner = matchup.UserOne;

        matchup.Complete(winner);

        Assert.Multiple(() =>
        {
            Assert.That(matchup.Winner,
                Is.EqualTo(winner));

            Assert.That(matchup.IsCompleted,
                Is.True);
        });
    }


    [Test]
    public void Complete_ShouldThrow_WhenWinnerNotInMatchup()
    {
        var matchup = CreateMatchup();

        var invalidUser =
            TestDataFactory.CreateUser();

        Assert.Throws<InvalidOperationException>(() =>
            matchup.Complete(invalidUser));
    }


    [Test]
    public void Complete_ShouldThrow_WhenAlreadyCompleted()
    {
        var matchup = CreateMatchup();

        matchup.Complete(matchup.UserOne);

        Assert.Throws<InvalidOperationException>(() =>
            matchup.Complete(matchup.UserTwo));
    }


    [Test]
    public void GetOpponent_ShouldReturnCorrectOpponent()
    {
        var matchup = CreateMatchup();

        Assert.Multiple(() =>
        {
            Assert.That(
                matchup.GetOpponent(matchup.UserOne),
                Is.EqualTo(matchup.UserTwo));

            Assert.That(
                matchup.GetOpponent(matchup.UserTwo),
                Is.EqualTo(matchup.UserOne));
        });
    }


    [Test]
    public void GetOpponent_ShouldThrow_WhenUserNotInMatchup()
    {
        var matchup = CreateMatchup();

        Assert.Throws<InvalidOperationException>(() =>
            matchup.GetOpponent(
                TestDataFactory.CreateUser()
            ));
    }


    private static PlayoffMatchup CreateMatchup()
    {
        return new PlayoffMatchup(
            CreateRound(),
            TestDataFactory.CreateUser(),
            TestDataFactory.CreateUser()
        );
    }


    private static PlayoffRound CreateRound()
    {
        return new PlayoffRound(
            new PlayoffBracket(CreateSeason()),
            "Championship",
            1
        );
    }


    private static LeagueSeason CreateSeason()
    {
        var owner = TestDataFactory.CreateUser();

        return new LeagueSeason(
            TestDataFactory.CreateLeague(owner),
            "2026 Season",
            2026,
            DateTime.UtcNow.AddMonths(-6),
            DateTime.UtcNow
        );
    }
}