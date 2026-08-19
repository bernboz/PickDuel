using NUnit.Framework;
using PickDuel.Domain.Entities;
using PickDuel.Domain.Entities.Playoffs;
using PickDuel.Tests.Common;

namespace PickDuel.Tests.Domain.Playoffs;

public class PlayoffAdvancementTests
{
    [Test]
    public void CreateNextRound_ShouldCreateNextRoundFromWinners()
    {
        var bracket = CreateCompletedBracket();

        var firstRound = CreateCompletedRound(
            bracket,
            4
        );

        bracket.AddRound(firstRound);

        var nextRound =
            bracket.CreateNextRound();

        Assert.Multiple(() =>
        {
            Assert.That(
                nextRound.RoundNumber,
                Is.EqualTo(2)
            );

            Assert.That(
                nextRound.Matchups.Count,
                Is.EqualTo(1)
            );
        });
    }


    [Test]
    public void CreateNextRound_ShouldUsePreviousRoundWinners()
    {
        var bracket = CreateCompletedBracket();

        var userOne = TestDataFactory.CreateUser();
        var userTwo = TestDataFactory.CreateUser();
        var userThree = TestDataFactory.CreateUser();
        var userFour = TestDataFactory.CreateUser();

        var round =
            new PlayoffRound(
                bracket,
                "Quarterfinals",
                1
            );

        var matchupOne =
            new PlayoffMatchup(
                round,
                userOne,
                userTwo
            );

        var matchupTwo =
            new PlayoffMatchup(
                round,
                userThree,
                userFour
            );

        round.AddMatchup(matchupOne);
        round.AddMatchup(matchupTwo);

        matchupOne.Complete(userOne);
        matchupTwo.Complete(userThree);

        round.Complete();

        bracket.AddRound(round);

        var nextRound =
            bracket.CreateNextRound();

        var matchup =
            nextRound.Matchups.First();

        Assert.Multiple(() =>
        {
            Assert.That(
                matchup.UserOne,
                Is.EqualTo(userOne)
            );

            Assert.That(
                matchup.UserTwo,
                Is.EqualTo(userThree)
            );
        });
    }


    [Test]
    public void CreateNextRound_ShouldIncrementRoundNumber()
    {
        var bracket =
            CreateCompletedBracket();

        var round =
            CreateCompletedRound(
                bracket,
                4
            );

        bracket.AddRound(round);

        var next =
            bracket.CreateNextRound();

        Assert.That(
            next.RoundNumber,
            Is.EqualTo(round.RoundNumber + 1)
        );
    }


    [Test]
    public void CreateNextRound_ShouldThrow_WhenNoRoundsExist()
    {
        var bracket =
            CreateCompletedBracket();

        Assert.Throws<InvalidOperationException>(() =>
            bracket.CreateNextRound()
        );
    }


    [Test]
    public void CreateNextRound_ShouldThrow_WhenCurrentRoundIsIncomplete()
    {
        var bracket =
            CreateCompletedBracket();

        var round =
            new PlayoffRound(
                bracket,
                "Quarterfinals",
                1
            );

        round.AddMatchup(
            new PlayoffMatchup(
                round,
                TestDataFactory.CreateUser(),
                TestDataFactory.CreateUser()
            ));

        bracket.AddRound(round);

        Assert.Throws<InvalidOperationException>(() =>
            bracket.CreateNextRound()
        );
    }


    [Test]
    public void CreateNextRound_ShouldThrow_WhenNotEnoughWinners()
    {
        var bracket =
            CreateCompletedBracket();

        var round =
            new PlayoffRound(
                bracket,
                "Championship",
                1
            );

        var matchup =
            new PlayoffMatchup(
                round,
                TestDataFactory.CreateUser(),
                TestDataFactory.CreateUser()
            );

        round.AddMatchup(matchup);

        matchup.Complete(
            matchup.UserOne
        );

        round.Complete();

        bracket.AddRound(round);

        Assert.Throws<InvalidOperationException>(() =>
            bracket.CreateNextRound()
        );
    }


    private static PlayoffBracket CreateCompletedBracket()
    {
        return new PlayoffBracket(
            CreateSeason()
        );
    }


    private static PlayoffRound CreateCompletedRound(
        PlayoffBracket bracket,
        int matchupCount)
    {
        var round =
            new PlayoffRound(
                bracket,
                "Quarterfinals",
                1
            );

        for (int i = 0; i < matchupCount / 2; i++)
        {
            var matchup =
                new PlayoffMatchup(
                    round,
                    TestDataFactory.CreateUser(),
                    TestDataFactory.CreateUser()
                );

            round.AddMatchup(matchup);

            matchup.Complete(
                matchup.UserOne
            );
        }

        round.Complete();

        return round;
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
            DateTime.UtcNow
        );
    }
}