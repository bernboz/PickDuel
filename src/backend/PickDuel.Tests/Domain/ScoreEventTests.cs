using NUnit.Framework;
using PickDuel.Domain.Entities;
using PickDuel.Domain.Enums;
using PickDuel.Tests.Common;

namespace PickDuel.Tests.Domain;

public class ScoreEventTests
{
    [Test]
    public void NewScoreEvent_ShouldInitializeCorrectly()
    {
        var user = TestDataFactory.CreateUser();
        var league = TestDataFactory.CreateLeague(user);

        var scoreEvent = new ScoreEvent(
            user,
            league,
            5,
            ScoreEventType.CorrectWinner,
            "Correctly predicted winner"
        );

        Assert.Multiple(() =>
        {
            Assert.That(scoreEvent.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(scoreEvent.User, Is.EqualTo(user));
            Assert.That(scoreEvent.League, Is.EqualTo(league));
            Assert.That(scoreEvent.Points, Is.EqualTo(5));
            Assert.That(scoreEvent.Type, Is.EqualTo(ScoreEventType.CorrectWinner));
            Assert.That(scoreEvent.Description, Is.EqualTo("Correctly predicted winner"));
            Assert.That(scoreEvent.CreatedAt, Is.LessThanOrEqualTo(DateTime.UtcNow));
        });
    }


    [Test]
    public void NewScoreEvent_ShouldSupportNegativePointPenalties()
    {
        var user = TestDataFactory.CreateUser();
        var league = TestDataFactory.CreateLeague(user);

        var scoreEvent = new ScoreEvent(
            user,
            league,
            -10,
            ScoreEventType.Penalty,
            "Incorrect high confidence prediction"
        );

        Assert.Multiple(() =>
        {
            Assert.That(scoreEvent.Points, Is.EqualTo(-10));
            Assert.That(scoreEvent.Type, Is.EqualTo(ScoreEventType.Penalty));
            Assert.That(scoreEvent.Description, Is.EqualTo("Incorrect high confidence prediction"));
        });
    }


    [Test]
    public void NewScoreEvent_ShouldAllowZeroPointEvents()
    {
        var user = TestDataFactory.CreateUser();
        var league = TestDataFactory.CreateLeague(user);

        var scoreEvent = new ScoreEvent(
            user,
            league,
            0,
            ScoreEventType.CorrectWinner,
            "Prediction recorded"
        );

        Assert.That(
            scoreEvent.Points,
            Is.Zero
        );
    }


    [Test]
    public void NewScoreEvent_ShouldThrow_WhenUserIsNull()
    {
        var league = TestDataFactory.CreateLeague(
            TestDataFactory.CreateUser()
        );

        Assert.Throws<ArgumentNullException>(() =>
            new ScoreEvent(
                null!,
                league,
                5,
                ScoreEventType.CorrectWinner,
                "Test"
            ));
    }


    [Test]
    public void NewScoreEvent_ShouldThrow_WhenLeagueIsNull()
    {
        var user = TestDataFactory.CreateUser();

        Assert.Throws<ArgumentNullException>(() =>
            new ScoreEvent(
                user,
                null!,
                5,
                ScoreEventType.CorrectWinner,
                "Test"
            ));
    }


    [Test]
    public void NewScoreEvent_ShouldThrow_WhenDescriptionIsEmpty()
    {
        var user = TestDataFactory.CreateUser();
        var league = TestDataFactory.CreateLeague(user);

        Assert.Throws<ArgumentException>(() =>
            new ScoreEvent(
                user,
                league,
                5,
                ScoreEventType.CorrectWinner,
                string.Empty
            ));
    }


    [Test]
    public void NewScoreEvent_ShouldThrow_WhenDescriptionIsWhitespace()
    {
        var user = TestDataFactory.CreateUser();
        var league = TestDataFactory.CreateLeague(user);

        Assert.Throws<ArgumentException>(() =>
            new ScoreEvent(
                user,
                league,
                5,
                ScoreEventType.CorrectWinner,
                "   "
            ));
    }


    [Test]
    public void NewScoreEvent_ShouldAllowDifferentScoreEventTypes()
    {
        var user = TestDataFactory.CreateUser();
        var league = TestDataFactory.CreateLeague(user);

        var scoreEvent = new ScoreEvent(
            user,
            league,
            25,
            ScoreEventType.CorrectWinner,
            "Correct prediction"
        );

        Assert.That(
            scoreEvent.Type,
            Is.EqualTo(ScoreEventType.CorrectWinner)
        );
    }
}