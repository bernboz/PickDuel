using NUnit.Framework;
using PickDuel.Application.Scoring;
using PickDuel.Domain.Entities;
using PickDuel.Domain.Enums;

namespace PickDuel.Tests.Application;

public class PickScoringServiceTests
{
    [Test]
    public void CalculateTotalPoints_ShouldReturnPointsFromSingleRule()
    {
        // Arrange
        var rule = new FakeScoringRule(5);

        var service = new PickScoringService(
            new List<IPickScoringRule>
            {
                rule
            });

        var context = CreateContext();

        // Act
        var points = service.CalculateTotalPoints(context);

        // Assert
        Assert.That(points, Is.EqualTo(5));
    }


    [Test]
    public void CalculateTotalPoints_ShouldAddPointsFromMultipleRules()
    {
        // Arrange
        var service = new PickScoringService(
            new List<IPickScoringRule>
            {
                new FakeScoringRule(5),
                new FakeScoringRule(10)
            });

        var context = CreateContext();

        // Act
        var points = service.CalculateTotalPoints(context);

        // Assert
        Assert.That(points, Is.EqualTo(15));
    }


    [Test]
    public void Constructor_ShouldThrow_WhenRulesAreNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new PickScoringService(null!));
    }


    [Test]
    public void CalculateTotalPoints_ShouldThrow_WhenContextIsNull()
    {
        var service = new PickScoringService(
            new List<IPickScoringRule>
            {
                new FakeScoringRule(5)
            });

        Assert.Throws<ArgumentNullException>(() =>
            service.CalculateTotalPoints(null!));
    }


    private static PickEvaluationContext CreateContext()
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
            GameOutcome.HomeWin,
            27,
            21
        );

        return new PickEvaluationContext(
            pick,
            result
        );
    }


    private class FakeScoringRule : IPickScoringRule
    {
        private readonly int _points;

        public FakeScoringRule(int points)
        {
            _points = points;
        }

        public int CalculatePoints(PickEvaluationContext context)
        {
            return _points;
        }
    }
}