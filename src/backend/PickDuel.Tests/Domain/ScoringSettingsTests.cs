using NUnit.Framework;
using PickDuel.Domain.ValueObjects;

namespace PickDuel.Tests.Domain;

public class ScoringSettingsTests
{
    [Test]
    public void NewScoringSettings_ShouldInitializeCorrectly()
    {
        var settings = new ScoringSettings(
            2,
            5
        );

        Assert.That(settings.WinnerPoints, Is.EqualTo(2));
        Assert.That(settings.ExactScorePoints, Is.EqualTo(5));
    }

    [Test]
    public void NewScoringSettings_ShouldThrow_WhenWinnerPointsNegative()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new ScoringSettings(
                -1,
                5
            ));
    }

    [Test]
    public void NewScoringSettings_ShouldThrow_WhenExactScoreNegative()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new ScoringSettings(
                1,
                -5
            ));
    }
}