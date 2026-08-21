using NUnit.Framework;
using PickDuel.Domain.Entities;
using PickDuel.Domain.Enums;
using PickDuel.Tests.Common;

namespace PickDuel.Tests.Domain.Game;

public class GameResultTests
{
    [Test]
    public void NewGameResult_ShouldInitializeCorrectly_WhenHomeTeamWins()
    {
        var game = TestDataFactory.CreateGame();

        var beforeCreation = DateTime.UtcNow;

        var result = new GameResult(
            game,
            GameOutcome.HomeWin,
            27,
            24
        );

        var afterCreation = DateTime.UtcNow;

        Assert.Multiple(() =>
        {
            Assert.That(result.Game, Is.EqualTo(game));
            Assert.That(result.Outcome, Is.EqualTo(GameOutcome.HomeWin));
            Assert.That(result.HomeScore, Is.EqualTo(27));
            Assert.That(result.AwayScore, Is.EqualTo(24));
            Assert.That(result.CompletedAt, Is.GreaterThanOrEqualTo(beforeCreation));
            Assert.That(result.CompletedAt, Is.LessThanOrEqualTo(afterCreation));
        });
    }


    [Test]
    public void NewGameResult_ShouldInitializeCorrectly_WhenAwayTeamWins()
    {
        var game = TestDataFactory.CreateGame();

        var result = new GameResult(
            game,
            GameOutcome.AwayWin,
            21,
            28
        );

        Assert.Multiple(() =>
        {
            Assert.That(result.Outcome, Is.EqualTo(GameOutcome.AwayWin));
            Assert.That(result.HomeScore, Is.EqualTo(21));
            Assert.That(result.AwayScore, Is.EqualTo(28));
        });
    }


    [Test]
    public void NewGameResult_ShouldInitializeCorrectly_WhenGameIsTied()
    {
        var game = new PickDuel.Domain.Entities.Game(
            "Packers",
            "Vikings",
            DateTime.UtcNow.AddDays(1),
            DateTime.UtcNow.AddDays(1).AddHours(3)
        );

        var result = new GameResult(
            game,
            GameOutcome.Tie,
            20,
            20
        );

        Assert.Multiple(() =>
        {
            Assert.That(result.Outcome, Is.EqualTo(GameOutcome.Tie));
            Assert.That(result.HomeScore, Is.EqualTo(20));
            Assert.That(result.AwayScore, Is.EqualTo(20));
        });
    }


    [Test]
    public void NewGameResult_ShouldThrow_WhenGameIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new GameResult(
                null!,
                GameOutcome.HomeWin,
                27,
                24
            ));
    }


    [Test]
    public void NewGameResult_ShouldThrow_WhenOutcomeDoesNotMatchHomeWinScore()
    {
        var game = TestDataFactory.CreateGame();

        Assert.Throws<ArgumentException>(() =>
            new GameResult(
                game,
                GameOutcome.AwayWin,
                27,
                24
            ));
    }


    [Test]
    public void NewGameResult_ShouldThrow_WhenOutcomeDoesNotMatchAwayWinScore()
    {
        var game = TestDataFactory.CreateGame();

        Assert.Throws<ArgumentException>(() =>
            new GameResult(
                game,
                GameOutcome.HomeWin,
                21,
                28
            ));
    }


    [Test]
    public void NewGameResult_ShouldThrow_WhenScoresAreNegative()
    {
        var game = TestDataFactory.CreateGame();

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new GameResult(
                game,
                GameOutcome.HomeWin,
                -1,
                24
            ));
    }


    [Test]
    public void NewGameResult_ShouldThrow_WhenAwayScoreIsNegative()
    {
        var game = TestDataFactory.CreateGame();

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new GameResult(
                game,
                GameOutcome.HomeWin,
                27,
                -1
            ));
    }
}