using NUnit.Framework;
using PickDuel.Domain.Entities;
using PickDuel.Tests.Common;

namespace PickDuel.Tests.Domain;

public class LeagueStandingTests
{
    [Test]
    public void NewLeagueStanding_ShouldInitializeWithZeroStatistics()
    {
        var standing = CreateStanding();

        Assert.That(standing.TotalPoints, Is.Zero);
        Assert.That(standing.TotalWins, Is.Zero);
        Assert.That(standing.TotalLosses, Is.Zero);
        Assert.That(standing.TotalPicks, Is.Zero);
    }


    [Test]
    public void AddPoints_ShouldIncreaseTotalPoints()
    {
        var standing = CreateStanding();

        standing.AddPoints(50);

        Assert.That(standing.TotalPoints, Is.EqualTo(50));
    }


    [Test]
    public void AddPoints_ShouldAllowNegativeScores()
    {
        var standing = CreateStanding();

        standing.AddPoints(100);
        standing.AddPoints(-25);

        Assert.That(standing.TotalPoints, Is.EqualTo(75));
    }


    [Test]
    public void AddPoints_ShouldHandleMultipleScoreEvents()
    {
        var standing = CreateStanding();

        standing.AddPoints(50);
        standing.AddPoints(25);
        standing.AddPoints(-10);

        Assert.That(standing.TotalPoints, Is.EqualTo(65));
    }


    [Test]
    public void RecordPredictionResult_ShouldIncreaseWins_WhenPredictionIsCorrect()
    {
        var standing = CreateStanding();

        standing.RecordPredictionResult(true);

        Assert.That(standing.TotalWins, Is.EqualTo(1));
        Assert.That(standing.TotalLosses, Is.Zero);
        Assert.That(standing.TotalPicks, Is.EqualTo(1));
    }


    [Test]
    public void RecordPredictionResult_ShouldIncreaseLosses_WhenPredictionIsIncorrect()
    {
        var standing = CreateStanding();

        standing.RecordPredictionResult(false);

        Assert.That(standing.TotalWins, Is.Zero);
        Assert.That(standing.TotalLosses, Is.EqualTo(1));
        Assert.That(standing.TotalPicks, Is.EqualTo(1));
    }


    [Test]
    public void RecordPredictionResult_ShouldTrackMultiplePredictions()
    {
        var standing = CreateStanding();

        standing.RecordPredictionResult(true);
        standing.RecordPredictionResult(true);
        standing.RecordPredictionResult(false);

        Assert.That(standing.TotalWins, Is.EqualTo(2));
        Assert.That(standing.TotalLosses, Is.EqualTo(1));
        Assert.That(standing.TotalPicks, Is.EqualTo(3));
    }


    [Test]
    public void GetWinPercentage_ShouldReturnCorrectPercentage()
    {
        var standing = CreateStanding();

        standing.RecordPredictionResult(true);
        standing.RecordPredictionResult(true);
        standing.RecordPredictionResult(false);
        standing.RecordPredictionResult(true);

        var winPercentage = standing.GetWinPercentage();

        Assert.That(winPercentage, Is.EqualTo(75));
    }


    [Test]
    public void GetWinPercentage_ShouldReturnZero_WhenNoPredictionsExist()
    {
        var standing = CreateStanding();

        var winPercentage = standing.GetWinPercentage();

        Assert.That(winPercentage, Is.Zero);
    }


    [Test]
    public void CreatingStanding_ShouldRequireUser()
    {
        var league = TestDataFactory.CreateLeague(
            TestDataFactory.CreateUser()
        );

        Assert.Throws<ArgumentNullException>(() =>
            new LeagueStanding(
                null!,
                league
            ));
    }


    [Test]
    public void CreatingStanding_ShouldRequireLeague()
    {
        var user = TestDataFactory.CreateUser();

        Assert.Throws<ArgumentNullException>(() =>
            new LeagueStanding(
                user,
                null!
            ));
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
}