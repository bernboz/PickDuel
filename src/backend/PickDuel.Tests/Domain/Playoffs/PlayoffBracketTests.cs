using NUnit.Framework;
using PickDuel.Tests.Common;
using PickDuel.Domain.Entities;
using PickDuel.Domain.Entities.Playoffs;

namespace PickDuel.Tests.Domain.Playoffs;

public class PlayoffBracketTests
{
    [Test]
    public void Constructor_ShouldInitializeCorrectly()
    {
        var bracket =
            new PlayoffBracket(CreateSeason());

        Assert.Multiple(() =>
        {
            Assert.That(bracket.Rounds, Is.Empty);
            Assert.That(bracket.Champion, Is.Null);
            Assert.That(bracket.IsCompleted, Is.False);
        });
    }


    [Test]
    public void AddRound_ShouldAddRound()
    {
        var bracket =
            new PlayoffBracket(CreateSeason());

        var round =
            new PlayoffRound(
                bracket,
                "Final",
                1);

        bracket.AddRound(round);

        Assert.That(
            bracket.Rounds.Count,
            Is.EqualTo(1));
    }


    [Test]
    public void AddRound_ShouldThrow_WhenRoundBelongsToDifferentBracket()
    {
        var bracket =
            new PlayoffBracket(CreateSeason());

        var round =
            new PlayoffRound(
                new PlayoffBracket(CreateSeason()),
                "Final",
                1);

        Assert.Throws<InvalidOperationException>(() =>
            bracket.AddRound(round));
    }


    [Test]
    public void Complete_ShouldThrowWithoutChampion()
    {
        var bracket =
            new PlayoffBracket(CreateSeason());

        Assert.Throws<InvalidOperationException>(() =>
            bracket.Complete());
    }


    [Test]
    public void SetChampion_ShouldSetChampion()
    {
        var bracket =
            new PlayoffBracket(CreateSeason());

        var user =
            TestDataFactory.CreateUser();

        var round =
            new PlayoffRound(
                bracket,
                "Final",
                1);

        var matchup =
            new PlayoffMatchup(
                round,
                user,
                TestDataFactory.CreateUser());

        round.AddMatchup(matchup);
        bracket.AddRound(round);

        matchup.Complete(user);

        bracket.SetChampion(user);

        Assert.That(
            bracket.Champion,
            Is.EqualTo(user));
    }


    private static LeagueSeason CreateSeason()
    {
        var user =
            TestDataFactory.CreateUser();

        return new LeagueSeason(
            TestDataFactory.CreateLeague(user),
            "2026 Season",
            2026,
            DateTime.UtcNow.AddMonths(-6),
            DateTime.UtcNow);
    }
}