using NUnit.Framework;
using PickDuel.Domain.Entities;
using PickDuel.Domain.Enums;

namespace PickDuel.Tests.Domain;

public class ScoreEventTests
{
    [Test]
    public void CreatingScoreEvent_ShouldRecordScoringAdjustment()
    {
        // Arrange
        var user = CreateUser();
        var league = CreateLeague();

        // Act
        var scoreEvent = new ScoreEvent(
            user,
            league,
            5,
            ScoreEventType.CorrectWinner,
            "Correctly predicted winner"
        );

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(scoreEvent.User, Is.EqualTo(user));
            Assert.That(scoreEvent.League, Is.EqualTo(league));
            Assert.That(scoreEvent.Points, Is.EqualTo(5));
            Assert.That(scoreEvent.Type, Is.EqualTo(ScoreEventType.CorrectWinner));
            Assert.That(scoreEvent.CreatedAt, Is.LessThanOrEqualTo(DateTime.UtcNow));
        });
    }


    [Test]
    public void CreatingScoreEvent_ShouldSupportPenalties()
    {
        // Arrange
        var user = CreateUser();
        var league = CreateLeague();

        // Act
        var scoreEvent = new ScoreEvent(
            user,
            league,
            -10,
            ScoreEventType.Penalty,
            "Incorrect high confidence prediction"
        );

        // Assert
        Assert.That(scoreEvent.Points, Is.EqualTo(-10));
        Assert.That(scoreEvent.Type, Is.EqualTo(ScoreEventType.Penalty));
    }


    [Test]
    public void CreatingScoreEvent_ShouldAllowZeroPointEvents()
    {
        // Arrange
        var user = CreateUser();
        var league = CreateLeague();

        // Act
        var scoreEvent = new ScoreEvent(
            user,
            league,
            0,
            ScoreEventType.CorrectWinner,
            "Prediction outcome recorded"
        );

        // Assert
        Assert.That(scoreEvent.Points, Is.Zero);
    }


    [Test]
    public void CreatingScoreEvent_ShouldRejectMissingUser()
    {
        // Arrange
        var league = CreateLeague();

        // Act & Assert
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
    public void CreatingScoreEvent_ShouldRejectMissingLeague()
    {
        // Arrange
        var user = CreateUser();

        // Act & Assert
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
    public void CreatingScoreEvent_ShouldRejectBlankDescriptions()
    {
        // Arrange
        var user = CreateUser();
        var league = CreateLeague();

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            new ScoreEvent(
                user,
                league,
                5,
                ScoreEventType.CorrectWinner,
                " "
            ));
    }


    private static User CreateUser()
    {
        return new User(
            "Bob",
            "Smith",
            "bob@test.com",
            "bob"
        );
    }


    private static League CreateLeague()
    {
        return new League(
            "NFL League",
            SportType.NFL,
            CreateUser()
        );
    }
}