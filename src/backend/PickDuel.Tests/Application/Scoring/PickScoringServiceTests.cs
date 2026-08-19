using NSubstitute;
using NUnit.Framework;
using PickDuel.Application.Scoring;
using PickDuel.Domain.Entities;
using PickDuel.Tests.Common;

namespace PickDuel.Tests.Application;

public class PickScoringServiceTests
{
    [Test]
    public void Constructor_ShouldThrow_WhenFactoryIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new PickScoringService(null!)
        );
    }


    [Test]
    public void CalculateTotalPoints_ShouldReturnPointsFromSingleRule()
    {
        var rule = CreateMockRule(25);

        var service = CreateService(rule);

        var context = TestDataFactory.CreateCorrectPredictionContext();

        var points = service.CalculateTotalPoints(context);

        Assert.That(points, Is.EqualTo(25));
    }
    
    [Test]
    public void CalculateTotalPoints_ShouldReturnZero_WhenNoRulesExist()
    {
        var service = CreateService();

        var context = TestDataFactory.CreateCorrectPredictionContext();

        var points = service.CalculateTotalPoints(context);

        Assert.That(points, Is.Zero);
    }

    [Test]
    public void CalculateTotalPoints_ShouldSumPointsFromMultipleRules()
    {
        var firstRule = CreateMockRule(25);
        var secondRule = CreateMockRule(50);

        var service = CreateService(
            firstRule,
            secondRule
        );

        var context = TestDataFactory.CreateCorrectPredictionContext();

        var points = service.CalculateTotalPoints(context);

        Assert.That(points, Is.EqualTo(75));
    }


    [Test]
    public void CalculateTotalPoints_ShouldSupportNegativeScoringRules()
    {
        var rewardRule = CreateMockRule(50);
        var penaltyRule = CreateMockRule(-25);

        var service = CreateService(
            rewardRule,
            penaltyRule
        );

        var context = TestDataFactory.CreateCorrectPredictionContext();

        var points = service.CalculateTotalPoints(context);

        Assert.That(points, Is.EqualTo(25));
    }


    [Test]
    public void CalculateTotalPoints_ShouldReturnZero_WhenAllRulesReturnZero()
    {
        var rule = CreateMockRule(0);

        var service = CreateService(rule);

        var context = TestDataFactory.CreateCorrectPredictionContext();

        var points = service.CalculateTotalPoints(context);

        Assert.That(points, Is.Zero);
    }


    [Test]
    public void CalculateTotalPoints_ShouldThrow_WhenContextIsNull()
    {
        var service = CreateService(
            CreateMockRule(10)
        );

        Assert.Throws<ArgumentNullException>(() =>
            service.CalculateTotalPoints(null!)
        );
    }


    [Test]
    public void CalculateTotalPoints_ShouldEvaluateEveryRule()
    {
        var firstRule = CreateMockRule(10);
        var secondRule = CreateMockRule(20);

        var service = CreateService(
            firstRule,
            secondRule
        );

        var context = TestDataFactory.CreateCorrectPredictionContext();

        service.CalculateTotalPoints(context);

        firstRule.Received(1)
            .CalculatePoints(context);

        secondRule.Received(1)
            .CalculatePoints(context);
    }
    
    [Test]
    public void CalculateTotalPoints_ShouldRequestRulesFromFactory()
    {
        var rule =
            Substitute.For<IPickScoringRule>();

        rule.CalculatePoints(
                Arg.Any<PickEvaluationContext>()
            )
            .Returns(10);


        var factory =
            Substitute.For<IScoringRuleFactory>();

        factory.GetRules(
                Arg.Any<PickEvaluationContext>()
            )
            .Returns(new[] { rule });


        var service =
            new PickScoringService(factory);


        var context =
            TestDataFactory.CreateCorrectPredictionContext();


        service.CalculateTotalPoints(context);


        factory.Received(1)
            .GetRules(context);
    }


    private static IPickScoringService CreateService(params IPickScoringRule[] rules)
    {
        var factory =
            Substitute.For<IScoringRuleFactory>();

        factory.GetRules(
                Arg.Any<PickEvaluationContext>()
            )
            .Returns(rules);

        return new PickScoringService(factory);
    }


    private static IPickScoringRule CreateMockRule(int points)
    {
        var rule = Substitute.For<IPickScoringRule>();

        rule.CalculatePoints(Arg.Any<PickEvaluationContext>())
            .Returns(points);

        return rule;
    }
}