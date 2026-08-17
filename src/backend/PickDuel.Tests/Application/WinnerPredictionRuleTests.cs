using NUnit.Framework;
using PickDuel.Application.Scoring;
using PickDuel.Application.Scoring.Rules;
using PickDuel.Domain.Entities;
using PickDuel.Domain.Enums;
using PickDuel.Domain.ValueObjects;

namespace PickDuel.Tests.Application;

public class WinnerPredictionRuleTests
{
    [Test]
    public void CalculatePoints_ShouldReturnWinnerPoints_WhenUserPickedWinningTeam()
    {
        var user = new User(
            "Bob",
            "Smith",
            "bob@test.com",
            "bob"
        );

        var settings = new ScoringSettings(
            winnerPoints: 3,
            exactScorePoints: 5
        );

        var league = new League(
            "NFL League",
            SportType.NFL,
            user,
            settings
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
            21
        );

        var context = new PickEvaluationContext(
            pick,
            result
        );

        var rule = new WinnerPredictionRule();

        var points = rule.CalculatePoints(context);

        Assert.That(points, Is.EqualTo(3));
    }


    [Test]
    public void CalculatePoints_ShouldReturnZero_WhenUserPickedLosingTeam()
    {
        var user = new User(
            "Bob",
            "Smith",
            "bob@test.com",
            "bob"
        );

        var league = new League(
            "NFL League",
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
            "Bills"
        );

        var result = new GameResult(
            game,
            GameOutcome.HomeWin,
            27,
            21
        );

        var context = new PickEvaluationContext(
            pick,
            result
        );

        var rule = new WinnerPredictionRule();

        var points = rule.CalculatePoints(context);

        Assert.That(points, Is.EqualTo(0));
    }


    [Test]
    public void CalculatePoints_ShouldReturnZero_WhenGameEndsInTie()
    {
        var user = new User(
            "Bob",
            "Smith",
            "bob@test.com",
            "bob"
        );

        var league = new League(
            "NFL League",
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
            GameOutcome.Tie,
            21,
            21
        );

        var context = new PickEvaluationContext(
            pick,
            result
        );

        var rule = new WinnerPredictionRule();

        var points = rule.CalculatePoints(context);

        Assert.That(points, Is.EqualTo(0));
    }
}