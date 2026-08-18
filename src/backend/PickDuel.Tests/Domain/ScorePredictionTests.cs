using NUnit.Framework;
using PickDuel.Domain.ValueObjects;
using PickDuel.Domain.Entities.Predictions;


namespace PickDuel.Tests.Domain;

public class ScorePredictionTests
{
    [Test]
    public void Constructor_ShouldStoreScores()
    {
        var prediction = new ScorePrediction(24, 17);

        Assert.That(prediction.HomeScore, Is.EqualTo(24));
        Assert.That(prediction.AwayScore, Is.EqualTo(17));
    }


    [Test]
    public void Constructor_ShouldThrow_WhenHomeScoreIsNegative()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new ScorePrediction(-1, 17)
        );
    }


    [Test]
    public void Constructor_ShouldThrow_WhenAwayScoreIsNegative()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new ScorePrediction(24, -1)
        );
    }


    [Test]
    public void Constructor_ShouldAllowZeroScores()
    {
        var prediction = new ScorePrediction(0, 0);

        Assert.That(prediction.HomeScore, Is.EqualTo(0));
        Assert.That(prediction.AwayScore, Is.EqualTo(0));
    }
    
}