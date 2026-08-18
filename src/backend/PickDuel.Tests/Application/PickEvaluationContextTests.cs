using NUnit.Framework;
using PickDuel.Application.Scoring;
using PickDuel.Domain.Entities;
using PickDuel.Domain.Enums;
using PickDuel.Tests.Common;

namespace PickDuel.Tests.Application;

public class PickEvaluationContextTests
{
    [Test]
    public void NewContext_ShouldInitializeCorrectly()
    {
        var context = TestDataFactory.CreateCorrectPredictionContext();

        Assert.Multiple(() =>
        {
            Assert.That(context.Pick, Is.Not.Null);
            Assert.That(context.GameResult, Is.Not.Null);
            Assert.That(context.GameOdds, Is.Not.Null);

            Assert.That(context.Pick.Game, Is.EqualTo(context.GameResult.Game));
            Assert.That(context.Pick.Game, Is.EqualTo(context.GameOdds.Game));
        });
    }


    [Test]
    public void NewContext_ShouldThrow_WhenPickIsNull()
    {
        var game = TestDataFactory.CreateGame();
        var result = TestDataFactory.CreateHomeWinResult(game);
        var odds = TestDataFactory.CreateGameOdds(game);

        Assert.Throws<ArgumentNullException>(() =>
            new PickEvaluationContext(
                null!,
                result,
                odds
            ));
    }


    [Test]
    public void NewContext_ShouldThrow_WhenGameResultIsNull()
    {
        var pickContext = TestDataFactory.CreateCorrectPredictionContext();

        Assert.Throws<ArgumentNullException>(() =>
            new PickEvaluationContext(
                pickContext.Pick,
                null!,
                pickContext.GameOdds
            ));
    }


    [Test]
    public void NewContext_ShouldThrow_WhenGameOddsIsNull()
    {
        var pickContext = TestDataFactory.CreateCorrectPredictionContext();

        Assert.Throws<ArgumentNullException>(() =>
            new PickEvaluationContext(
                pickContext.Pick,
                pickContext.GameResult,
                null!
            ));
    }


    [Test]
    public void NewContext_ShouldThrow_WhenPickAndResultGamesDoNotMatch()
    {
        var user = TestDataFactory.CreateUser();
        var league = TestDataFactory.CreateLeague(user);

        var firstGame = TestDataFactory.CreateGame();

        var secondGame = new Game(
            "Packers",
            "Bears",
            DateTime.UtcNow.AddHours(1),
            DateTime.UtcNow.AddHours(3)
        );

        var pick = TestDataFactory.CreatePick(
            user,
            league,
            firstGame,
            firstGame.HomeTeam
        );

        var result = new GameResult(
            secondGame,
            GameOutcome.HomeWin,
            21,
            17
        );

        var odds = TestDataFactory.CreateGameOdds(firstGame);

        Assert.Throws<ArgumentException>(() =>
            new PickEvaluationContext(
                pick,
                result,
                odds
            ));
    }


    [Test]
    public void NewContext_ShouldThrow_WhenPickAndOddsGamesDoNotMatch()
    {
        var context = TestDataFactory.CreateCorrectPredictionContext();

        var differentGame = new Game(
            "Packers",
            "Bears",
            DateTime.UtcNow.AddHours(1),
            DateTime.UtcNow.AddHours(3)
        );

        var differentOdds = TestDataFactory.CreateGameOdds(
            differentGame
        );

        Assert.Throws<ArgumentException>(() =>
            new PickEvaluationContext(
                context.Pick,
                context.GameResult,
                differentOdds
            ));
    }
}