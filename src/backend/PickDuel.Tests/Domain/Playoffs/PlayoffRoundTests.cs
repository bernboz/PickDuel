using NUnit.Framework;
using PickDuel.Domain.Entities;
using PickDuel.Domain.Entities.Playoffs;
using PickDuel.Tests.Common;

namespace PickDuel.Tests.Domain.Playoffs;

public class PlayoffRoundTests
{
    [Test]
    public void Constructor_ShouldInitializeCorrectly()
    {
        var round = CreateRound();

        Assert.Multiple(() =>
        {
            Assert.That(round.Matchups, Is.Empty);
            Assert.That(round.IsCompleted, Is.False);
            Assert.That(round.RoundNumber, Is.EqualTo(1));
        });
    }


    [Test]
    public void Constructor_ShouldThrow_WhenRoundNumberInvalid()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new PlayoffRound(
                CreateBracket(),
                "Final",
                0
            ));
    }


    [Test]
    public void AddMatchup_ShouldAddMatchup()
    {
        var round = CreateRound();

        var matchup =
            new PlayoffMatchup(
                round,
                TestDataFactory.CreateUser(),
                TestDataFactory.CreateUser()
            );

        round.AddMatchup(matchup);

        Assert.That(
            round.Matchups.Count,
            Is.EqualTo(1));
    }


    [Test]
    public void AddMatchup_ShouldThrow_WhenDifferentRound()
    {
        var round = CreateRound();

        var matchup =
            new PlayoffMatchup(
                CreateRound(),
                TestDataFactory.CreateUser(),
                TestDataFactory.CreateUser()
            );

        Assert.Throws<InvalidOperationException>(() =>
            round.AddMatchup(matchup));
    }


    [Test]
    public void Complete_ShouldCompleteWhenAllMatchupsFinished()
    {
        var round = CreateRound();

        var matchup =
            new PlayoffMatchup(
                round,
                TestDataFactory.CreateUser(),
                TestDataFactory.CreateUser()
            );

        round.AddMatchup(matchup);

        matchup.Complete(matchup.UserOne);

        round.Complete();

        Assert.That(
            round.IsCompleted,
            Is.True);
    }


    [Test]
    public void Complete_ShouldThrow_WhenMatchupsIncomplete()
    {
        var round = CreateRound();

        round.AddMatchup(
            new PlayoffMatchup(
                round,
                TestDataFactory.CreateUser(),
                TestDataFactory.CreateUser()
            ));

        Assert.Throws<InvalidOperationException>(() =>
            round.Complete());
    }


    [Test]
    public void GetWinners_ShouldReturnWinners()
    {
        var round = CreateRound();

        var matchup =
            new PlayoffMatchup(
                round,
                TestDataFactory.CreateUser(),
                TestDataFactory.CreateUser()
            );

        round.AddMatchup(matchup);

        matchup.Complete(matchup.UserOne);

        round.Complete();

        var winners =
            round.GetWinners();

        Assert.That(
            winners.First(),
            Is.EqualTo(matchup.UserOne));
    }


    private static PlayoffRound CreateRound()
    {
        return new PlayoffRound(
            CreateBracket(),
            "Semifinals",
            1
        );
    }


    private static PlayoffBracket CreateBracket()
    {
        return new PlayoffBracket(CreateSeason());
    }


    private static LeagueSeason CreateSeason()
    {
        var user = TestDataFactory.CreateUser();

        return new LeagueSeason(
            TestDataFactory.CreateLeague(user),
            "2026 Season",
            2026,
            DateTime.UtcNow.AddMonths(-6),
            DateTime.UtcNow
        );
    }
}