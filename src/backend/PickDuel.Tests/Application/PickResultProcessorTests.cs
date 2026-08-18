using NSubstitute;
using NUnit.Framework;
using PickDuel.Application.Scoring;
using PickDuel.Domain.Entities;
using PickDuel.Domain.Enums;
using PickDuel.Tests.Common;

namespace PickDuel.Tests.Application;

public class PickResultProcessorTests
{
    [Test]
    public void Constructor_ShouldThrow_WhenScoringServiceIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new PickResultProcessor(null!)
        );
    }


    [Test]
    public void ProcessPickResult_ShouldCreateCorrectWinnerEvent_WhenPredictionIsCorrect()
    {
        var processor = CreateProcessor(50);
        var context = TestDataFactory.CreateCorrectPredictionContext();

        var result = processor.ProcessPickResult(context);

        Assert.Multiple(() =>
        {
            Assert.That(result.User, Is.EqualTo(context.Pick.User));
            Assert.That(result.League, Is.EqualTo(context.Pick.League));
            Assert.That(result.Points, Is.EqualTo(50));
            Assert.That(result.Type, Is.EqualTo(ScoreEventType.CorrectWinner));
            Assert.That(result.Description, Is.EqualTo("Correct winner prediction"));
        });
    }


    [Test]
    public void ProcessPickResult_ShouldCreatePenaltyEvent_WhenPredictionIsIncorrect()
    {
        var processor = CreateProcessor(-25);
        var context = TestDataFactory.CreateIncorrectPredictionContext();

        var result = processor.ProcessPickResult(context);

        Assert.Multiple(() =>
        {
            Assert.That(result.Points, Is.EqualTo(-25));
            Assert.That(result.Type, Is.EqualTo(ScoreEventType.Penalty));
            Assert.That(result.Description, Is.EqualTo("Incorrect prediction penalty"));
        });
    }


    [Test]
    public void ProcessPickResult_ShouldCreateScoreEvent_WhenPointsAreZero()
    {
        var processor = CreateProcessor(0);
        var context = TestDataFactory.CreateCorrectPredictionContext();

        var result = processor.ProcessPickResult(context);

        Assert.Multiple(() =>
        {
            Assert.That(result.Points, Is.Zero);
            Assert.That(result.User, Is.EqualTo(context.Pick.User));
            Assert.That(result.League, Is.EqualTo(context.Pick.League));
        });
    }


    [Test]
    public void ProcessPickResult_ShouldThrow_WhenContextIsNull()
    {
        var processor = CreateProcessor(10);

        Assert.Throws<ArgumentNullException>(() =>
            processor.ProcessPickResult(null!)
        );
    }


    [Test]
    public void ApplyScoreEvent_ShouldIncreaseStandingPoints_WhenEventIsPositive()
    {
        var processor = CreateProcessor(50);

        var standing = CreateStanding();

        var scoreEvent = CreateScoreEvent(50);

        processor.ApplyScoreEvent(
            scoreEvent,
            standing
        );

        Assert.That(
            standing.TotalPoints,
            Is.EqualTo(50)
        );
    }


    [Test]
    public void ApplyScoreEvent_ShouldDecreaseStandingPoints_WhenEventIsNegative()
    {
        var processor = CreateProcessor(-25);

        var standing = CreateStanding();

        var scoreEvent = CreateScoreEvent(-25);

        processor.ApplyScoreEvent(
            scoreEvent,
            standing
        );

        Assert.That(
            standing.TotalPoints,
            Is.EqualTo(-25)
        );
    }


    [Test]
    public void ApplyScoreEvent_ShouldThrow_WhenScoreEventIsNull()
    {
        var processor = CreateProcessor(10);

        var standing = CreateStanding();

        Assert.Throws<ArgumentNullException>(() =>
            processor.ApplyScoreEvent(
                null!,
                standing
            ));
    }


    [Test]
    public void ApplyScoreEvent_ShouldThrow_WhenStandingIsNull()
    {
        var processor = CreateProcessor(10);

        var scoreEvent = CreateScoreEvent(10);

        Assert.Throws<ArgumentNullException>(() =>
            processor.ApplyScoreEvent(
                scoreEvent,
                null!
            ));
    }


    [Test]
    public void ProcessPickResult_ShouldUseScoringServiceResult()
    {
        var rule = Substitute.For<IPickScoringRule>();

        var context = TestDataFactory.CreateCorrectPredictionContext();

        rule.CalculatePoints(context)
            .Returns(75);

        var service = new PickScoringService(
            new List<IPickScoringRule>
            {
                rule
            });

        var processor = new PickResultProcessor(service);

        var result = processor.ProcessPickResult(context);

        Assert.That(
            result.Points,
            Is.EqualTo(75)
        );

        rule.Received(1)
            .CalculatePoints(context);
    }


    private static PickResultProcessor CreateProcessor(int points)
    {
        var rule = Substitute.For<IPickScoringRule>();

        rule.CalculatePoints(Arg.Any<PickEvaluationContext>())
            .Returns(points);

        var scoringService = new PickScoringService(
            new List<IPickScoringRule>
            {
                rule
            });

        return new PickResultProcessor(scoringService);
    }


    private static LeagueStanding CreateStanding()
    {
        var user = TestDataFactory.CreateUser();
        var league = TestDataFactory.CreateLeague(user);

        return new LeagueStanding(
            user,
            league
        );
    }


    private static ScoreEvent CreateScoreEvent(int points)
    {
        var user = TestDataFactory.CreateUser();
        var league = TestDataFactory.CreateLeague(user);

        return new ScoreEvent(
            user,
            league,
            points,
            points >= 0
                ? ScoreEventType.CorrectWinner
                : ScoreEventType.Penalty,
            points >= 0
                ? "Correct winner prediction"
                : "Incorrect prediction penalty"
        );
    }
}