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

        Assert.Multiple(() =>
        {
            Assert.That(
                settings.WinnerPoints,
                Is.EqualTo(2)
            );

            Assert.That(
                settings.ExactScorePoints,
                Is.EqualTo(5)
            );
        });
    }


    [Test]
    public void NewScoringSettings_ShouldAllowZeroPointValues()
    {
        var settings = new ScoringSettings(
            0,
            0
        );

        Assert.Multiple(() =>
        {
            Assert.That(
                settings.WinnerPoints,
                Is.Zero
            );

            Assert.That(
                settings.ExactScorePoints,
                Is.Zero
            );
        });
    }


    [Test]
    public void NewScoringSettings_ShouldThrow_WhenWinnerPointsAreNegative()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new ScoringSettings(
                -1,
                5
            ));
    }


    [Test]
    public void NewScoringSettings_ShouldThrow_WhenExactScorePointsAreNegative()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new ScoringSettings(
                1,
                -5
            ));
    }


    [Test]
    public void NewScoringSettings_ShouldStoreLargeValidValues()
    {
        var settings = new ScoringSettings(
            1000,
            5000
        );

        Assert.Multiple(() =>
        {
            Assert.That(
                settings.WinnerPoints,
                Is.EqualTo(1000)
            );

            Assert.That(
                settings.ExactScorePoints,
                Is.EqualTo(5000)
            );
        });
    }
}