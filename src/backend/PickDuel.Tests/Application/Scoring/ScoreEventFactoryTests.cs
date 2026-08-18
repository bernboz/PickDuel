using NUnit.Framework;
using PickDuel.Application.Scoring;
using PickDuel.Domain.Enums;
using PickDuel.Tests.Common;

namespace PickDuel.Tests.Application;

public class ScoreEventFactoryTests
{
    [Test]
    public void Create_ShouldThrow_WhenContextIsNull()
    {
        var factory = new ScoreEventFactory();

        Assert.Throws<ArgumentNullException>(() =>
            factory.Create(
                null!,
                10
            ));
    }


    [Test]
    public void Create_ShouldCreateCorrectWinnerEvent_WhenPredictionIsCorrect()
    {
        var factory = new ScoreEventFactory();
        var context = TestDataFactory.CreateCorrectPredictionContext();

        var result = factory.Create(
            context,
            10
        );

        Assert.Multiple(() =>
        {
            Assert.That(result.Type,
                Is.EqualTo(ScoreEventType.CorrectWinner));

            Assert.That(result.Description,
                Is.EqualTo("Correct winner prediction"));
        });
    }


    [Test]
    public void Create_ShouldCreateExactScoreEvent_WhenScoreMatches()
    {
        var factory = new ScoreEventFactory();
        var context = TestDataFactory.CreateExactScorePredictionContext();

        var result = factory.Create(
            context,
            50
        );

        Assert.That(
            result.Type,
            Is.EqualTo(ScoreEventType.ExactScore)
        );
    }


    [Test]
    public void Create_ShouldCreatePenaltyEvent_WhenPredictionIsWrong()
    {
        var factory = new ScoreEventFactory();
        var context = TestDataFactory.CreateIncorrectPredictionContext();

        var result = factory.Create(
            context,
            -25
        );

        Assert.That(
            result.Type,
            Is.EqualTo(ScoreEventType.Penalty)
        );
    }


    [Test]
    public void Create_ShouldCreateNeutralEvent_WhenPointsAreZero()
    {
        var factory = new ScoreEventFactory();
        var context = TestDataFactory.CreateCorrectPredictionContext();

        var result = factory.Create(
            context,
            0
        );

        Assert.That(
            result.Type,
            Is.EqualTo(ScoreEventType.Neutral)
        );
    }
}