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
            new PickResultProcessor(
                null!,
                new ScoreEventFactory()
            ));
    }


    [Test]
    public void Constructor_ShouldThrow_WhenScoreEventFactoryIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new PickResultProcessor(
                CreateScoringService(10),
                null!
            ));
    }


    [Test]
    public void ProcessPickResult_ShouldCreateCorrectWinnerEvent_WhenPredictionIsCorrect()
    {
        var processor = CreateProcessor(10);
        var context = TestDataFactory.CreateCorrectPredictionContext();

        var result = processor.ProcessPickResult(context);

        Assert.Multiple(() =>
        {
            Assert.That(result.Points, Is.EqualTo(10));
            Assert.That(result.Type, Is.EqualTo(ScoreEventType.CorrectWinner));
            Assert.That(result.Description,
                Is.EqualTo("Correct winner prediction"));
        });
    }


    [Test]
    public void ProcessPickResult_ShouldCreateExactScoreEvent_WhenScorePredictionMatches()
    {
        var processor = CreateProcessor(50);
        var context = TestDataFactory.CreateExactScorePredictionContext();

        var result = processor.ProcessPickResult(context);

        Assert.Multiple(() =>
        {
            Assert.That(result.Points, Is.EqualTo(50));
            Assert.That(result.Type, Is.EqualTo(ScoreEventType.ExactScore));
            Assert.That(result.Description,
                Is.EqualTo("Exact score prediction"));
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
            Assert.That(result.Description,
                Is.EqualTo("Incorrect prediction penalty"));
        });
    }


    [Test]
    public void ProcessPickResult_ShouldCreateNeutralEvent_WhenPointsAreZero()
    {
        var processor = CreateProcessor(0);
        var context = TestDataFactory.CreateCorrectPredictionContext();

        var result = processor.ProcessPickResult(context);

        Assert.That(result.Type, Is.EqualTo(ScoreEventType.Neutral));
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
    public void ApplyScoreEvent_ShouldUpdatePoints()
    {
        var processor = CreateProcessor(10);
        var standing = CreateStanding();

        processor.ApplyScoreEvent(
            CreateScoreEvent(10, ScoreEventType.CorrectWinner),
            standing
        );

        Assert.That(
            standing.TotalPoints,
            Is.EqualTo(10)
        );
    }

    [Test]
    public void ApplyScoreEvent_ShouldOnlyUpdatePoints()
    {
        var processor = CreateProcessor(50);
        var standing = CreateStanding();

        processor.ApplyScoreEvent(
            CreateScoreEvent(
                50,
                ScoreEventType.ExactScore
            ),
            standing
        );

        Assert.Multiple(() =>
        {
            Assert.That(
                standing.TotalPoints,
                Is.EqualTo(50)
            );

            Assert.That(
                standing.MatchupWins,
                Is.Zero
            );

            Assert.That(
                standing.MatchupLosses,
                Is.Zero
            );
        });
    }
    
    [Test]
    public void ApplyScoreEvent_ShouldThrow_WhenScoreEventIsNull()
    {
        var processor = CreateProcessor(10);

        Assert.Throws<ArgumentNullException>(() =>
            processor.ApplyScoreEvent(
                null!,
                CreateStanding()
            ));
    }


    [Test]
    public void ApplyScoreEvent_ShouldThrow_WhenStandingIsNull()
    {
        var processor = CreateProcessor(10);

        Assert.Throws<ArgumentNullException>(() =>
            processor.ApplyScoreEvent(
                CreateScoreEvent(10, ScoreEventType.CorrectWinner),
                null!
            ));
    }


    private static PickResultProcessor CreateProcessor(int points)
    {
        return new PickResultProcessor(
            CreateScoringService(points),
            new ScoreEventFactory()
        );
    }


    private static PickScoringService CreateScoringService(int points)
    {
        var rule = Substitute.For<IPickScoringRule>();

        rule.CalculatePoints(Arg.Any<PickEvaluationContext>())
            .Returns(points);

        return new PickScoringService(
            new[] { rule }
        );
    }


    private static LeagueStanding CreateStanding()
    {
        var user = TestDataFactory.CreateUser();

        return new LeagueStanding(
            user,
            TestDataFactory.CreateLeague(user)
        );
    }


    private static ScoreEvent CreateScoreEvent(
        int points,
        ScoreEventType type)
    {
        var user = TestDataFactory.CreateUser();

        return new ScoreEvent(
            user,
            TestDataFactory.CreateLeague(user),
            points,
            type,
            "Test score event"
        );
    }
}