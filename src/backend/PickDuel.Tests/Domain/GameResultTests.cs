using NUnit.Framework;
using PickDuel.Domain.Entities;
using PickDuel.Domain.Enums;

namespace PickDuel.Tests.Domain;

public class GameResultTests
{
    [Test]
    public void NewGameResult_ShouldInitializeCorrectly_WhenHomeTeamWins()
    {
        var game = new Game(
            "Chiefs",
            "Bills",
            DateTime.UtcNow.AddDays(1),
            DateTime.UtcNow.AddDays(1).AddHours(3)
        );

        var result = new GameResult(
            game,
            GameOutcome.HomeWin,
            27,
            24
        );

        Assert.That(result.Game, Is.EqualTo(game));
        Assert.That(result.Outcome, Is.EqualTo(GameOutcome.HomeWin));
        Assert.That(result.HomeScore, Is.EqualTo(27));
        Assert.That(result.AwayScore, Is.EqualTo(24));
        Assert.That(result.CompletedAt <= DateTime.UtcNow);
    }


    [Test]
    public void NewGameResult_ShouldInitializeCorrectly_WhenAwayTeamWins()
    {
        var game = new Game(
            "Chiefs",
            "Bills",
            DateTime.UtcNow.AddDays(1),
            DateTime.UtcNow.AddDays(1).AddHours(3)
        );

        var result = new GameResult(
            game,
            GameOutcome.AwayWin,
            21,
            28
        );

        Assert.That(result.Outcome, Is.EqualTo(GameOutcome.AwayWin));
        Assert.That(result.HomeScore, Is.EqualTo(21));
        Assert.That(result.AwayScore, Is.EqualTo(28));
    }


    [Test]
    public void NewGameResult_ShouldInitializeCorrectly_WhenGameIsTied()
    {
        var game = new Game(
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

        Assert.That(result.Outcome, Is.EqualTo(GameOutcome.Tie));
        Assert.That(result.HomeScore, Is.EqualTo(20));
        Assert.That(result.AwayScore, Is.EqualTo(20));
    }


    [Test]
    public void NewGameResult_ShouldThrowException_WhenOutcomeDoesNotMatchScore()
    {
        var game = new Game(
            "Chiefs",
            "Bills",
            DateTime.UtcNow.AddDays(1),
            DateTime.UtcNow.AddDays(1).AddHours(3)
        );

        Assert.Throws<ArgumentException>(() =>
            new GameResult(
                game,
                GameOutcome.AwayWin,
                27,
                24
            ));
    }


    [Test]
    public void NewGameResult_ShouldThrowException_WhenScoresAreNegative()
    {
        var game = new Game(
            "Chiefs",
            "Bills",
            DateTime.UtcNow.AddDays(1),
            DateTime.UtcNow.AddDays(1).AddHours(3)
        );

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new GameResult(
                game,
                GameOutcome.HomeWin,
                -1,
                24
            ));
    }
}