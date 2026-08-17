using NUnit.Framework;
using PickDuel.Application.Scoring;
using PickDuel.Domain.Entities;
using PickDuel.Domain.Enums;

namespace PickDuel.Tests.Application;

public class PickEvaluationContextTests
{
    [Test]
    public void NewContext_ShouldInitializeCorrectly()
    {
        var user = new User(
            "Bob",
            "Smith",
            "bob@test.com",
            "bob"
        );

        var league = new League(
            "NFL",
            SportType.NFL,
            user
        );

        var game = new Game(
            "Chiefs",
            "Bills",
            DateTime.UtcNow,
            DateTime.UtcNow.AddHours(3)
        );

        var pick = new Pick(
            user,
            league,
            game,
            "Chiefs"
        );

        var result = new GameResult(
            game,
            GameOutcome.HomeWin,
            27,
            24
        );

        var context = new PickEvaluationContext(
            pick,
            result
        );

        Assert.That(context.Pick, Is.EqualTo(pick));
        Assert.That(context.GameResult, Is.EqualTo(result));
    }

    [Test]
    public void NewContext_ShouldThrow_WhenGamesDoNotMatch()
    {
        var user = new User(
            "Bob",
            "Smith",
            "bob@test.com",
            "bob"
        );

        var league = new League(
            "NFL",
            SportType.NFL,
            user
        );

        var game1 = new Game(
            "Chiefs",
            "Bills",
            DateTime.UtcNow,
            DateTime.UtcNow.AddHours(3)
        );

        var game2 = new Game(
            "Packers",
            "Bears",
            DateTime.UtcNow,
            DateTime.UtcNow.AddHours(3)
        );

        var pick = new Pick(
            user,
            league,
            game1,
            "Chiefs"
        );

        var result = new GameResult(
            game2,
            GameOutcome.HomeWin,
            21,
            17
        );

        Assert.Throws<ArgumentException>(() =>
            new PickEvaluationContext(
                pick,
                result
            ));
    }
}