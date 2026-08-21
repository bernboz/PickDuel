using NUnit.Framework;
using PickDuel.Domain.Entities;
using PickDuel.Domain.Entities.History;
using PickDuel.Domain.ValueObjects;
using PickDuel.Domain.Enums;
using PickDuel.Tests.Common;

namespace PickDuel.Tests.Domain.History;

public class PickHistoryTests
{
    [Test]
    public void Constructor_ShouldInitializeCorrectly()
    {
        var user = TestDataFactory.CreateUser();
        var league = TestDataFactory.CreateLeague(user);
        var game = TestDataFactory.CreateGame();

        var prediction = new ScorePrediction(
            27,
            20
        );

        var history = new PickHistory(
            user,
            league,
            game,
            game.HomeTeam,
            prediction,
            GameOutcome.HomeWin,
            24,
            17,
            50,
            ScoreEventType.ExactScore
        );

        Assert.Multiple(() =>
        {
            Assert.That(history.User, Is.EqualTo(user));
            Assert.That(history.League, Is.EqualTo(league));
            Assert.That(history.Game, Is.EqualTo(game));

            Assert.That(history.PredictedTeam,
                Is.EqualTo(game.HomeTeam));

            Assert.That(history.PredictedScore!.HomeScore,
                Is.EqualTo(27));

            Assert.That(history.PredictedScore.AwayScore,
                Is.EqualTo(20));

            Assert.That(history.ActualOutcome,
                Is.EqualTo(GameOutcome.HomeWin));

            Assert.That(history.ActualHomeScore,
                Is.EqualTo(24));

            Assert.That(history.ActualAwayScore,
                Is.EqualTo(17));

            Assert.That(history.PointsEarned,
                Is.EqualTo(50));

            Assert.That(history.ResultType,
                Is.EqualTo(ScoreEventType.ExactScore));
        });
    }


    [Test]
    public void Constructor_ShouldSetCompletedAt()
    {
        var before = DateTime.UtcNow;

        var history = CreateHistory();

        var after = DateTime.UtcNow;

        Assert.That(
            history.CompletedAt,
            Is.InRange(before, after)
        );
    }


    [Test]
    public void Constructor_ShouldThrow_WhenUserIsNull()
    {
        var user = TestDataFactory.CreateUser();

        Assert.Throws<ArgumentNullException>(() =>
            new PickHistory(
                null!,
                TestDataFactory.CreateLeague(user),
                TestDataFactory.CreateGame(),
                "Chiefs",
                CreatePrediction(),
                GameOutcome.HomeWin,
                24,
                17,
                10,
                ScoreEventType.CorrectWinner
            ));
    }


    [Test]
    public void Constructor_ShouldThrow_WhenLeagueIsNull()
    {
        var user = TestDataFactory.CreateUser();

        Assert.Throws<ArgumentNullException>(() =>
            new PickHistory(
                user,
                null!,
                TestDataFactory.CreateGame(),
                "Chiefs",
                CreatePrediction(),
                GameOutcome.HomeWin,
                24,
                17,
                10,
                ScoreEventType.CorrectWinner
            ));
    }


    [Test]
    public void Constructor_ShouldThrow_WhenGameIsNull()
    {
        var user = TestDataFactory.CreateUser();

        Assert.Throws<ArgumentNullException>(() =>
            new PickHistory(
                user,
                TestDataFactory.CreateLeague(user),
                null!,
                "Chiefs",
                CreatePrediction(),
                GameOutcome.HomeWin,
                24,
                17,
                10,
                ScoreEventType.CorrectWinner
            ));
    }


    [Test]
    public void Constructor_ShouldThrow_WhenSelectedTeamIsEmpty()
    {
        Assert.Throws<ArgumentException>(() =>
            CreateHistory(
                selectedTeam: ""
            ));
    }


    [Test]
    public void Constructor_ShouldSupportNegativePoints()
    {
        var history = CreateHistory(
            points: -25,
            resultType: ScoreEventType.Penalty
        );

        Assert.That(
            history.PointsEarned,
            Is.EqualTo(-25)
        );
    }


    [Test]
    public void Constructor_ShouldAllowNullPredictedScore()
    {
        var history = CreateHistory(
            prediction: null
        );

        Assert.That(
            history.PredictedScore,
            Is.Null
        );
    }


    [Test]
    public void Constructor_ShouldThrow_WhenActualHomeScoreIsNegative()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new PickHistory(
                TestDataFactory.CreateUser(),
                TestDataFactory.CreateLeague(
                    TestDataFactory.CreateUser()
                ),
                TestDataFactory.CreateGame(),
                "Chiefs",
                CreatePrediction(),
                GameOutcome.HomeWin,
                -1,
                20,
                10,
                ScoreEventType.CorrectWinner
            ));
    }


    [Test]
    public void Constructor_ShouldThrow_WhenActualAwayScoreIsNegative()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new PickHistory(
                TestDataFactory.CreateUser(),
                TestDataFactory.CreateLeague(
                    TestDataFactory.CreateUser()
                ),
                TestDataFactory.CreateGame(),
                "Chiefs",
                CreatePrediction(),
                GameOutcome.HomeWin,
                20,
                -1,
                10,
                ScoreEventType.CorrectWinner
            ));
    }


    [Test]
    public void Constructor_ShouldStoreWinnerOnlyPrediction()
    {
        var history = CreateHistory(
            prediction: null,
            resultType: ScoreEventType.CorrectWinner
        );

        Assert.Multiple(() =>
        {
            Assert.That(
                history.PredictedTeam,
                Is.EqualTo("Chiefs")
            );

            Assert.That(
                history.PredictedScore,
                Is.Null
            );

            Assert.That(
                history.ResultType,
                Is.EqualTo(ScoreEventType.CorrectWinner)
            );
        });
    }

    [Test]
    public void Constructor_ShouldThrow_WhenOutcomeDoesNotMatchHomeWinScore()
    {
        Assert.Throws<ArgumentException>(() =>
            new PickHistory(
                TestDataFactory.CreateUser(),
                TestDataFactory.CreateLeague(
                    TestDataFactory.CreateUser()
                ),
                TestDataFactory.CreateGame(),
                "Chiefs",
                CreatePrediction(),
                GameOutcome.AwayWin,
                24,
                17,
                10,
                ScoreEventType.CorrectWinner
            ));
    }


    [Test]
    public void Constructor_ShouldThrow_WhenOutcomeDoesNotMatchAwayWinScore()
    {
        Assert.Throws<ArgumentException>(() =>
            new PickHistory(
                TestDataFactory.CreateUser(),
                TestDataFactory.CreateLeague(
                    TestDataFactory.CreateUser()
                ),
                TestDataFactory.CreateGame(),
                "Chiefs",
                CreatePrediction(),
                GameOutcome.HomeWin,
                14,
                24,
                10,
                ScoreEventType.CorrectWinner
            ));
    }


    [Test]
    public void Constructor_ShouldThrow_WhenOutcomeDoesNotMatchTieScore()
    {
        Assert.Throws<ArgumentException>(() =>
            new PickHistory(
                TestDataFactory.CreateUser(),
                TestDataFactory.CreateLeague(
                    TestDataFactory.CreateUser()
                ),
                TestDataFactory.CreateGame(),
                "Chiefs",
                CreatePrediction(),
                GameOutcome.HomeWin,
                20,
                20,
                10,
                ScoreEventType.CorrectWinner
            ));
    }

    private static PickHistory CreateHistory(
        int points = 10,
        ScoreEventType resultType = ScoreEventType.CorrectWinner,
        string selectedTeam = "Chiefs",
        ScorePrediction? prediction = null)
    {
        var user = TestDataFactory.CreateUser();

        return new PickHistory(
            user,
            TestDataFactory.CreateLeague(user),
            TestDataFactory.CreateGame(),
            selectedTeam,
            prediction,
            GameOutcome.HomeWin,
            24,
            17,
            points,
            resultType
        );
    }


    private static ScorePrediction CreatePrediction()
    {
        return new ScorePrediction(
            27,
            20
        );
    }
}