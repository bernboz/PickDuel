using NUnit.Framework;
using PickDuel.Application.Scoring;
using PickDuel.Domain.Enums;
using PickDuel.Domain.ValueObjects;
using PickDuel.Tests.Common;

namespace PickDuel.Tests.Application.Scoring;

public class ScoringCalculatorTests
{
    private ScoringCalculator _calculator = null!;


    [SetUp]
    public void Setup()
    {
        _calculator = new ScoringCalculator();
    }


    [Test]
    public void Calculate_ShouldThrow_WhenPickIsNull()
    {
        var settings = CreateSettings();

        Assert.Throws<ArgumentNullException>(() =>
            _calculator.Calculate(null!, settings)
        );
    }

    [Test]
    public void Calculate_ShouldThrow_WhenSettingsAreNull()
    {
        var pick = TestDataFactory.CreateUncompletedPick();

        Assert.Throws<ArgumentNullException>(() => _calculator.Calculate(pick, null!));
    }

    [Test]
    public void Calculate_ShouldThrow_WhenGameIsNotCompleted()
    {
        var pick = TestDataFactory.CreateUncompletedPick();

        var settings = CreateSettings();

        Assert.Throws<InvalidOperationException>(() => _calculator.Calculate(pick, settings));
    }


    [Test]
    public void Calculate_ShouldReturnCorrectWinnerPoints_WhenPredictionIsCorrect()
    {
        var pick = TestDataFactory.CreateCompletedPick(
            "Clemson",
            24,
            10
        );

        var settings = CreateSettings();

        var result = _calculator.Calculate(
            pick,
            settings
        );

        Assert.That(result, Has.Count.EqualTo(1));

        Assert.Multiple(() =>
        {
            Assert.That(result.First().Points, Is.EqualTo(10));
            Assert.That(result.First().Type, Is.EqualTo(ScoreEventType.CorrectWinner));
        });
    }


    [Test]
    public void Calculate_ShouldReturnPenalty_WhenWinnerPredictionIsIncorrect()
    {
        var pick = TestDataFactory.CreateCompletedPick(
            "Florida State",
            24,
            10
        );

        var settings = CreateSettings();

        var result = _calculator.Calculate(
            pick,
            settings
        );

        Assert.Multiple(() =>
        {
            Assert.That(result.First().Points, Is.EqualTo(-5));
            Assert.That(result.First().Type, Is.EqualTo(ScoreEventType.Penalty));
        });
    }


    [Test]
    public void Calculate_ShouldApplyConfidenceMultiplier_WhenWinnerPredictionIsCorrect()
    {
        var pick = TestDataFactory.CreateCompletedPick(
            "Clemson",
            24,
            10,
            confidenceMultiplier: 5
        );

        var settings = CreateSettings();

        var result = _calculator.Calculate(
            pick,
            settings
        );

        Assert.That(
            result.First().Points,
            Is.EqualTo(50)
        );
    }


    [Test]
    public void Calculate_ShouldReturnExactScorePoints_WhenScorePredictionMatches()
    {
        var pick = TestDataFactory.CreateCompletedPickWithScorePrediction(
            "Clemson",
            24,
            10,
            24,
            10
        );

        var settings = CreateSettings();

        var result = _calculator.Calculate(
            pick,
            settings
        );

        Assert.That(result, Has.Count.EqualTo(2));

        Assert.That(
            result.Last().Type,
            Is.EqualTo(ScoreEventType.ExactScore)
        );
    }


    [Test]
    public void Calculate_ShouldReturnScoreDifferenceBonus_WhenPredictionIsWithinTolerance()
    {
        var pick = TestDataFactory.CreateCompletedPickWithScorePrediction(
            "Clemson",
            24,
            10,
            22,
            11
        );

        var settings = CreateSettings();

        var result = _calculator.Calculate(
            pick,
            settings
        );

        Assert.That(
            result.Last().Type,
            Is.EqualTo(ScoreEventType.ScoreDifference)
        );
    }


    [Test]
    public void Calculate_ShouldReturnPenalty_WhenScorePredictionIsOutsideThreshold()
    {
        var pick = TestDataFactory.CreateCompletedPickWithScorePrediction(
            "Clemson",
            50,
            50,
            0,
            0
        );

        var settings = CreateSettings();

        var result = _calculator.Calculate(
            pick,
            settings
        );

        Assert.That(
            result.Last().Type,
            Is.EqualTo(ScoreEventType.Penalty)
        );
    }
    
    [Test]
    public void Calculate_ShouldReturnNeutral_WhenScorePredictionDoesNotQualifyForBonusOrPenalty()
    {
        var pick = TestDataFactory.CreateCompletedPickWithScorePrediction(
            "Clemson",
            24,
            10,
            26,
            12
        );

        var settings = CreateSettings();

        var result = _calculator.Calculate(pick, settings);

        Assert.That(result.Last().Type, Is.EqualTo(ScoreEventType.Neutral));
    }

    [Test]
    public void Calculate_ShouldOnlyReturnWinnerResult_WhenNoScorePredictionExists()
    {
        var pick = TestDataFactory.CreateCompletedPick(
            "Clemson",
            24,
            10
        );

        var settings = CreateSettings();

        var result = _calculator.Calculate(
            pick,
            settings
        );

        Assert.That(
            result,
            Has.Count.EqualTo(1)
        );
    }
    
    [Test]
    public void Calculate_ShouldThrow_WhenPickHasAlreadyBeenScored()
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

        pick.Lock();

        game.CompleteGame(
            24,
            10
        );

        pick.MarkAsScored();

        var settings = CreateSettings();

        Assert.Throws<InvalidOperationException>(() => _calculator.Calculate(pick, settings));
    }

    private static ScoringSettings CreateSettings()
    {
        return new ScoringSettings(
            winnerPoints: 10,
            exactScorePoints: 25,
            scoreAccuracyBonus: 5,
            scoreAccuracyPenalty: -5,
            scoreTolerance: 3,
            maxScoreDifferencePenalty: 15
        );
    }
}