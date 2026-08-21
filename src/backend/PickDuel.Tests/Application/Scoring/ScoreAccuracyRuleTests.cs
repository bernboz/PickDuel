using NUnit.Framework;
using PickDuel.Application.Scoring;
using PickDuel.Domain.ValueObjects;
using PickDuel.Tests.Common;

namespace PickDuel.Tests.Application.Scoring;

public class ScoreAccuracyRuleTests
{
    [Test]
    public void Constructor_ShouldCreateRuleSuccessfully()
    {
        var rule = new ScoreAccuracyRule(CreateSettings());

        Assert.That(rule, Is.Not.Null);
    }


    [Test]
    public void CalculatePoints_ShouldAwardExactScoreBonus_WhenPredictionMatchesResult()
    {
        var rule = new ScoreAccuracyRule(CreateSettings());

        var context = TestDataFactory.CreateExactScorePredictionContext();

        var points = rule.CalculatePoints(context);

        Assert.That(
            points,
            Is.EqualTo(50)
        );
    }


    [Test]
    public void CalculatePoints_ShouldAwardAccuracyBonus_WhenPredictionIsWithinTolerance()
    {
        var rule = new ScoreAccuracyRule(CreateSettings());

        var context = TestDataFactory.CreateCloseScorePredictionContext();

        var points = rule.CalculatePoints(context);

        Assert.That(
            points,
            Is.EqualTo(25)
        );
    }


    [Test]
    public void CalculatePoints_ShouldApplyMaximumPenalty_WhenPredictionIsTooFarAway()
    {
        var rule = new ScoreAccuracyRule(CreateSettings());

        var context = TestDataFactory.CreateIncorrectScoreContext();

        var points = rule.CalculatePoints(context);

        Assert.That(
            points,
            Is.EqualTo(-50)
        );
    }


    [Test]
    public void CalculatePoints_ShouldThrow_WhenContextIsNull()
    {
        var rule = new ScoreAccuracyRule(CreateSettings());

        Assert.Throws<ArgumentNullException>(() =>
            rule.CalculatePoints(null!)
        );
    }


    [Test]
    public void CalculatePoints_ShouldAwardAccuracyBonus_WhenHomeScoreIsWithinTolerance()
    {
        var rule = new ScoreAccuracyRule(CreateSettings());

        var context = TestDataFactory.CreateHomeScoreMismatchContext();

        var points = rule.CalculatePoints(context);

        Assert.That(
            points,
            Is.EqualTo(25)
        );
    }


    [Test]
    public void CalculatePoints_ShouldAwardAccuracyBonus_WhenAwayScoreIsWithinTolerance()
    {
        var rule = new ScoreAccuracyRule(CreateSettings());

        var context = TestDataFactory.CreateAwayScoreMismatchContext();

        var points = rule.CalculatePoints(context);

        Assert.That(
            points,
            Is.EqualTo(25)
        );
    }


    private static ScoringSettings CreateSettings()
    {
        return new ScoringSettings(
            winnerPoints: 10,
            exactScorePoints: 50,
            scoreAccuracyBonus: 25,
            scoreAccuracyPenalty: -50,
            scoreTolerance: 5,
            maxScoreDifferencePenalty: 10
        );
    }
}