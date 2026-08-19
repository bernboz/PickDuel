using NUnit.Framework;
using PickDuel.Domain.Entities;
using PickDuel.Domain.Entities.Predictions;
using PickDuel.Domain.ValueObjects;
using PickDuel.Tests.Common;

namespace PickDuel.Tests.Domain;

public class PickTests
{
    [Test]
    public void NewPick_ShouldInitializeWithDefaultValues()
    {
        var pick = TestDataFactory.CreateFuturePick();

        Assert.Multiple(() =>
        {
            Assert.That(pick.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(pick.ConfidenceMultiplier, Is.EqualTo(3));
            Assert.That(pick.ScorePrediction, Is.Null);
            Assert.That(pick.IsLocked, Is.False);
            Assert.That(pick.IsScored, Is.False);
        });
    }


    [Test]
    public void NewPick_ShouldStoreSelectedTeam()
    {
        var pick = TestDataFactory.CreateFuturePick();

        Assert.That(
            pick.SelectedTeam,
            Is.EqualTo(pick.Game.HomeTeam)
        );
    }


    [Test]
    public void NewPick_ShouldThrow_WhenConfidenceIsBelowMinimum()
    {
        var user = TestDataFactory.CreateUser();
        var league = TestDataFactory.CreateLeague(user);
        var game = TestDataFactory.CreateFutureGame();

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new Pick(
                user,
                league,
                game,
                game.HomeTeam,
                0
            )
        );
    }


    [Test]
    public void NewPick_ShouldThrow_WhenConfidenceIsAboveMaximum()
    {
        var user = TestDataFactory.CreateUser();
        var league = TestDataFactory.CreateLeague(user);
        var game = TestDataFactory.CreateFutureGame();

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new Pick(
                user,
                league,
                game,
                game.HomeTeam,
                6
            )
        );
    }


    [Test]
    public void ChangeConfidence_ShouldUpdateValue_WhenGameHasNotStarted()
    {
        var pick = TestDataFactory.CreateFuturePick();

        pick.ChangeConfidence(5);

        Assert.That(
            pick.ConfidenceMultiplier,
            Is.EqualTo(5)
        );
    }


    [Test]
    public void ChangeConfidence_ShouldThrow_WhenGameHasStarted()
    {
        var pick = TestDataFactory.CreateStartedPick();

        Assert.Throws<InvalidOperationException>(() =>
            pick.ChangeConfidence(5)
        );
    }


    [Test]
    public void ChangeConfidence_ShouldThrow_WhenPickIsLocked()
    {
        var pick = TestDataFactory.CreateFuturePick();

        pick.Lock();

        Assert.Throws<InvalidOperationException>(() =>
            pick.ChangeConfidence(5)
        );
    }


    [Test]
    public void ChangeSelection_ShouldUpdateTeam_WhenGameHasNotStarted()
    {
        var pick = TestDataFactory.CreateFuturePick();

        pick.ChangeSelection(
            pick.Game.AwayTeam
        );

        Assert.That(
            pick.SelectedTeam,
            Is.EqualTo(pick.Game.AwayTeam)
        );
    }


    [Test]
    public void ChangeSelection_ShouldThrow_WhenGameHasStarted()
    {
        var pick = TestDataFactory.CreateStartedPick();

        Assert.Throws<InvalidOperationException>(() =>
            pick.ChangeSelection(
                pick.Game.AwayTeam
            )
        );
    }


    [Test]
    public void ChangeSelection_ShouldThrow_WhenPickIsLocked()
    {
        var pick = TestDataFactory.CreateFuturePick();

        pick.Lock();

        Assert.Throws<InvalidOperationException>(() =>
            pick.ChangeSelection(
                pick.Game.AwayTeam
            )
        );
    }


    [Test]
    public void ChangeSelection_ShouldThrow_WhenTeamDoesNotExistInGame()
    {
        var pick = TestDataFactory.CreateFuturePick();

        Assert.Throws<ArgumentException>(() =>
            pick.ChangeSelection("Cowboys")
        );
    }


    [Test]
    public void UpdateScorePrediction_ShouldStorePrediction()
    {
        var pick = TestDataFactory.CreateFuturePick();

        var prediction = new ScorePrediction(24, 17);

        pick.UpdateScorePrediction(prediction);

        Assert.That(
            pick.ScorePrediction,
            Is.EqualTo(prediction)
        );
    }


    [Test]
    public void UpdateScorePrediction_ShouldReplaceExistingPrediction()
    {
        var pick = TestDataFactory.CreateFuturePick();

        pick.UpdateScorePrediction(
            new ScorePrediction(24,17)
        );

        var updatedPrediction = new ScorePrediction(31,21);

        pick.UpdateScorePrediction(
            updatedPrediction
        );

        Assert.That(
            pick.ScorePrediction,
            Is.EqualTo(updatedPrediction)
        );
    }


    [Test]
    public void UpdateScorePrediction_ShouldThrow_WhenGameHasStarted()
    {
        var pick = TestDataFactory.CreateStartedPick();

        Assert.Throws<InvalidOperationException>(() =>
            pick.UpdateScorePrediction(
                new ScorePrediction(24,17)
            )
        );
    }


    [Test]
    public void UpdateScorePrediction_ShouldThrow_WhenPredictionIsNull()
    {
        var pick = TestDataFactory.CreateFuturePick();

        Assert.Throws<ArgumentNullException>(() =>
            pick.UpdateScorePrediction(null!)
        );
    }


    [Test]
    public void Lock_ShouldLockPick_WhenGameHasNotStarted()
    {
        var pick = TestDataFactory.CreateFuturePick();

        pick.Lock();

        Assert.That(
            pick.IsLocked,
            Is.True
        );
    }


    [Test]
    public void Lock_ShouldThrow_WhenPickIsAlreadyLocked()
    {
        var pick = TestDataFactory.CreateFuturePick();

        pick.Lock();

        Assert.Throws<InvalidOperationException>(() =>
            pick.Lock()
        );
    }


    [Test]
    public void Lock_ShouldThrow_WhenGameHasStarted()
    {
        var pick = TestDataFactory.CreateStartedPick();

        Assert.Throws<InvalidOperationException>(() =>
            pick.Lock()
        );
    }


    [Test]
    public void MarkAsScored_ShouldThrow_WhenPickIsNotLocked()
    {
        var pick = TestDataFactory.CreateFuturePick();

        Assert.Throws<InvalidOperationException>(() =>
            pick.MarkAsScored()
        );
    }


    [Test]
    public void MarkAsScored_ShouldMarkPickAsScored_WhenLocked()
    {
        var pick = TestDataFactory.CreateFuturePick();

        pick.Lock();

        pick.MarkAsScored();

        Assert.That(
            pick.IsScored,
            Is.True
        );
    }


    [Test]
    public void MarkAsScored_ShouldThrow_WhenAlreadyScored()
    {
        var pick = TestDataFactory.CreateFuturePick();

        pick.Lock();

        pick.MarkAsScored();

        Assert.Throws<InvalidOperationException>(() =>
            pick.MarkAsScored()
        );
    }
}