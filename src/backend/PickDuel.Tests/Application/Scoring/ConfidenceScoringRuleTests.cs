using NUnit.Framework;
using PickDuel.Application.Scoring.Rules;
using PickDuel.Tests.Common;

namespace PickDuel.Tests.Application.Scoring;

public class ConfidenceScoringRuleTests
{
    [Test]
    public void CalculatePoints_ShouldRewardHigherConfidenceOnSamePrediction()
    {
        var lowConfidenceContext =
            TestDataFactory.CreateCorrectPredictionContext(1);

        var highConfidenceContext =
            TestDataFactory.CreateCorrectPredictionContext(5);

        var rule = new ConfidenceScoringRule();

        var lowPoints =
            rule.CalculatePoints(lowConfidenceContext);

        var highPoints =
            rule.CalculatePoints(highConfidenceContext);

        Assert.That(highPoints, Is.GreaterThan(lowPoints));
    }


    [Test]
    public void CalculatePoints_ShouldRewardUnderdogPredictionMoreThanFavorite()
    {
        var favoriteContext =
            TestDataFactory.CreateCorrectPredictionContext(
                5,
                0.75m,
                0.25m
            );

        var underdogContext =
            TestDataFactory.CreateCorrectAwayPredictionContext(
                5,
                0.75m,
                0.25m
            );

        var rule = new ConfidenceScoringRule();

        var favoritePoints =
            rule.CalculatePoints(favoriteContext);

        var underdogPoints =
            rule.CalculatePoints(underdogContext);

        Assert.That(
            underdogPoints,
            Is.GreaterThan(favoritePoints)
        );
    }


    [Test]
    public void CalculatePoints_ShouldReturnNegativePoints_WhenPredictionIsIncorrect()
    {
        var context =
            TestDataFactory.CreateIncorrectPredictionContext();

        var rule = new ConfidenceScoringRule();

        var points =
            rule.CalculatePoints(context);

        Assert.That(points, Is.LessThan(0));
    }


    [Test]
    public void CalculatePoints_ShouldThrow_WhenContextIsNull()
    {
        var rule = new ConfidenceScoringRule();

        Assert.Throws<ArgumentNullException>(() =>
            rule.CalculatePoints(null!)
        );
    }
    
    [Test]
    public void CalculatePoints_ShouldCapDifficultyMultiplierAtMaximumValue()
    {
        var context = TestDataFactory.CreateCorrectPredictionContext(
            confidenceMultiplier: 5,
            homeProbability: 0.10m,
            awayProbability: 0.90m
        );

        var rule = new ConfidenceScoringRule();

        var points = rule.CalculatePoints(context);

        Assert.That(points, Is.EqualTo(150));
    }


    [Test]
    public void CalculatePoints_ShouldGiveEqualPointsForEvenOdds()
    {
        var context = TestDataFactory.CreateCorrectPredictionContext(
            confidenceMultiplier: 5,
            homeProbability: 0.50m,
            awayProbability: 0.50m
        );

        var rule = new ConfidenceScoringRule();

        var points = rule.CalculatePoints(context);

        Assert.That(points, Is.EqualTo(100));
    }


    [Test]
    public void CalculatePoints_ShouldIncreaseWithMaximumConfidence()
    {
        var lowConfidenceContext = TestDataFactory.CreateCorrectPredictionContext(1);

        var highConfidenceContext = TestDataFactory.CreateCorrectPredictionContext(5);

        var rule = new ConfidenceScoringRule();

        var lowConfidencePoints = rule.CalculatePoints(lowConfidenceContext);

        var highConfidencePoints = rule.CalculatePoints(highConfidenceContext);

        Assert.That(highConfidencePoints, Is.GreaterThan(lowConfidencePoints));
    }


    [Test]
    public void CalculatePoints_ShouldRemoveHalfPotentialPoints_WhenPredictionIsIncorrect()
    {
        var context = TestDataFactory.CreateIncorrectPredictionContext(5);

        var rule = new ConfidenceScoringRule();

        var points = rule.CalculatePoints(context);

        Assert.That(points, Is.EqualTo(-75));
    }


    [Test]
    public void CalculatePoints_ShouldAwardPoints_WhenAwayTeamPredictionIsCorrect()
    {
        var context = TestDataFactory.CreateCorrectAwayPredictionContext(
            confidenceMultiplier: 3
        );

        var rule = new ConfidenceScoringRule();

        var points = rule.CalculatePoints(context);

        Assert.That(points, Is.GreaterThan(0));
    }
}