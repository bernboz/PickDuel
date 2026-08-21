using NSubstitute;
using NUnit.Framework;
using PickDuel.Application.Scoring;
using PickDuel.Application.Scoring.Factories;
using PickDuel.Domain.Entities;
using PickDuel.Domain.Enums;
using PickDuel.Tests.Common;
using PickDuel.Application.Scoring.Services;
using PickDuel.Domain.Entities.Standings;
using PickDuel.Infrastructure.Data;

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
    
    [Test]
    public void ProcessPickResult_ShouldUseScoringService()
    {
        var rule =
            Substitute.For<IPickScoringRule>();

        rule.CalculatePoints(
                Arg.Any<PickEvaluationContext>())
            .Returns(25);

        var factory =
            Substitute.For<IScoringRuleFactory>();

        factory.GetRules(
                Arg.Any<PickEvaluationContext>()
            )
            .Returns(new[] { rule });


        var service =
            new PickScoringService(factory);

        var processor =
            new PickResultProcessor(
                service,
                new ScoreEventFactory()
            );

        var context =
            TestDataFactory.CreateCorrectPredictionContext();

        processor.ProcessPickResult(context);

        rule.Received(1)
            .CalculatePoints(context);
    }

    [Test]
    public void ApplyScoreEvent_ShouldAccumulatePoints()
    {
        var processor =
            CreateProcessor(10);

        var standing =
            CreateStanding();

        processor.ApplyScoreEvent(
            CreateScoreEvent(
                10,
                ScoreEventType.CorrectWinner
            ),
            standing
        );

        processor.ApplyScoreEvent(
            CreateScoreEvent(
                25,
                ScoreEventType.ExactScore
            ),
            standing
        );

        Assert.That(
            standing.TotalPoints,
            Is.EqualTo(35)
        );
    }
    
    [Test]
    public void ApplyScoreEvent_ShouldApplyNegativePoints()
    {
        var processor =
            CreateProcessor(-25);

        var standing =
            CreateStanding();

        processor.ApplyScoreEvent(
            CreateScoreEvent(
                -25,
                ScoreEventType.Penalty
            ),
            standing
        );

        Assert.That(
            standing.TotalPoints,
            Is.EqualTo(-25)
        );
    }

    /// <summary>
    /// Creates a PickResultProcessor with a configured scoring service.
    /// </summary>
    /// <param name="points">
    /// Points returned by the scoring service rules.
    /// </param>
    /// <returns>
    /// PickResultProcessor configured for testing.
    /// </returns>
    private static PickResultProcessor CreateProcessor(int points)
    {
        return new PickResultProcessor(
            CreateScoringService(points),
            new ScoreEventFactory()
        );
    }


    /// <summary>
    /// Creates a PickScoringService with a mocked scoring rule factory.
    /// </summary>
    /// <param name="points">
    /// Points returned by the mocked scoring rule.
    /// </param>
    /// <returns>
    /// PickScoringService configured with test scoring behavior.
    /// </returns>
    private static PickScoringService CreateScoringService(int points)
    {
        var rule = Substitute.For<IPickScoringRule>();

        rule.CalculatePoints(Arg.Any<PickEvaluationContext>())
            .Returns(points);

        var factory = Substitute.For<IScoringRuleFactory>();

        factory.GetRules(Arg.Any<PickEvaluationContext>())
            .Returns(new[] { rule });


        return new PickScoringService(factory);
    }


    /// <summary>
    /// Creates a league standing for testing.
    /// </summary>
    /// <returns>
    /// New LeagueStanding entity with test user and league.
    /// </returns>
    private static LeagueStanding CreateStanding()
    {
        var user = TestDataFactory.CreateUser();

        return new LeagueStanding(
            user,
            TestDataFactory.CreateLeague(user)
        );
    }


    /// <summary>
    /// Creates a score event for testing.
    /// </summary>
    /// <param name="points">
    /// Points assigned to the score event.
    /// </param>
    /// <param name="type">
    /// Type of scoring event created.
    /// </param>
    /// <returns>
    /// ScoreEvent configured with test data.
    /// </returns>
    private static ScoreEvent CreateScoreEvent(int points, ScoreEventType type)
    {
        var user = TestDataFactory.CreateUser();

        var league = TestDataFactory.CreateLeague(user);

        var game = TestDataFactory.CreateGame();

        var pick = TestDataFactory.CreatePick(
            user,
            league,
            game,
            game.HomeTeam,
            1
        );

        return new ScoreEvent(
            user,
            league,
            points,
            type,
            "Test score event",
            pick
        );
    }
}