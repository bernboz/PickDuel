using NUnit.Framework;
using PickDuel.Domain.ValueObjects;

namespace PickDuel.Tests.Domain;

public class ScoringSettingsTests
{
    [Test]
    public void NewScoringSettings_ShouldInitializeCorrectly()
    {
        var settings = CreateSettings();

        Assert.Multiple(() =>
        {
            Assert.That(settings.WinnerPoints, Is.EqualTo(10));
            Assert.That(settings.ExactScorePoints, Is.EqualTo(50));
            Assert.That(settings.ScoreAccuracyBonus, Is.EqualTo(25));
            Assert.That(settings.ScoreAccuracyPenalty, Is.EqualTo(-50));
            Assert.That(settings.ScoreTolerance, Is.EqualTo(5));
            Assert.That(settings.MaxScoreDifferencePenalty, Is.EqualTo(10));
        });
    }


    [Test]
    public void NewScoringSettings_ShouldThrow_WhenWinnerPointsAreNegative()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new ScoringSettings(
                -1,
                50,
                25,
                -50,
                5,
                10
            ));
    }


    [Test]
    public void NewScoringSettings_ShouldThrow_WhenExactScorePointsAreNegative()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new ScoringSettings(
                10,
                -50,
                25,
                -50,
                5,
                10
            ));
    }


    [Test]
    public void NewScoringSettings_ShouldThrow_WhenAccuracyBonusIsNegative()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new ScoringSettings(
                10,
                50,
                -25,
                -50,
                5,
                10
            ));
    }


    [Test]
    public void NewScoringSettings_ShouldThrow_WhenAccuracyPenaltyIsPositive()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new ScoringSettings(
                10,
                50,
                25,
                50,
                5,
                10
            ));
    }


    [Test]
    public void NewScoringSettings_ShouldThrow_WhenScoreToleranceIsNegative()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new ScoringSettings(
                10,
                50,
                25,
                -50,
                -1,
                10
            ));
    }


    [Test]
    public void NewScoringSettings_ShouldThrow_WhenMaxScoreDifferencePenaltyIsNegative()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new ScoringSettings(
                10,
                50,
                25,
                -50,
                5,
                -10
            ));
    }


    private static ScoringSettings CreateSettings()
    {
        return new ScoringSettings(
            10,
            50,
            25,
            -50,
            5,
            10
        );
    }
}