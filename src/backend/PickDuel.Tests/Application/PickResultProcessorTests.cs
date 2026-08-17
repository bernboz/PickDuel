using NUnit.Framework;
using PickDuel.Application.Scoring;
using PickDuel.Domain.Entities;
using PickDuel.Domain.Enums;

namespace PickDuel.Tests.Application;

public class PickResultProcessorTests
{
    [Test]
    public void Constructor_ShouldThrow_WhenScoringServiceIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new PickResultProcessor(null!)
        );
    }


    [Test]
    public void ProcessPickResult_ShouldCreatePositiveScoreEvent_WhenPickScoresPoints()
    {
        // Arrange
        var processor = CreateProcessor(5);

        var user = CreateUser();
        var league = CreateLeague(user);
        var game = CreateGame();

        var pick = new Pick(
            user,
            league,
            game,
            game.HomeTeam,
            3
        );

        var gameResult = new GameResult(
            game,
            GameOutcome.HomeWin,
            21,
            10
        );

        var odds = new GameOdds(
            game,
            0.75m,
            0.25m
        );

        var context = new PickEvaluationContext(
            pick,
            gameResult,
            odds
        );


        // Act
        var result = processor.ProcessPickResult(context);


        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.User, Is.EqualTo(user));
            Assert.That(result.League, Is.EqualTo(league));
            Assert.That(result.Points, Is.EqualTo(5));
            Assert.That(result.Type, Is.EqualTo(ScoreEventType.CorrectWinner));
        });
    }


    [Test]
    public void ProcessPickResult_ShouldCreateCorrectScoreEvent_WhenNoPointsAreEarned()
    {
        // Arrange
        var processor = CreateProcessor(0);

        var user = CreateUser();
        var league = CreateLeague(user);
        var game = CreateGame();

        var pick = new Pick(
            user,
            league,
            game,
            game.HomeTeam,
            3
        );

        var gameResult = new GameResult(
            game,
            GameOutcome.HomeWin,
            21,
            10
        );

        var odds = new GameOdds(
            game,
            0.75m,
            0.25m
        );

        var context = new PickEvaluationContext(
            pick,
            gameResult,
            odds
        );


        // Act
        var result = processor.ProcessPickResult(context);


        // Assert
        Assert.That(result.Points, Is.Zero);
    }


    [Test]
    public void ApplyScoreEvent_ShouldUpdateLeagueStanding()
    {
        // Arrange
        var processor = CreateProcessor(5);

        var user = CreateUser();
        var league = CreateLeague(user);

        var standing = new LeagueStanding(
            user,
            league
        );

        var scoreEvent = new ScoreEvent(
            user,
            league,
            5,
            ScoreEventType.CorrectWinner,
            "Correct winner prediction"
        );


        // Act
        processor.ApplyScoreEvent(
            scoreEvent,
            standing
        );


        // Assert
        Assert.That(
            standing.TotalPoints,
            Is.EqualTo(5)
        );
    }


    private static PickResultProcessor CreateProcessor(int points)
    {
        var rule = new FakeScoringRule(points);

        var scoringService = new PickScoringService(
            new List<IPickScoringRule>
            {
                rule
            });

        return new PickResultProcessor(scoringService);
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


    private static League CreateLeague(User user)
    {
        return new League(
            "NFL League",
            SportType.NFL,
            user
        );
    }


    private static Game CreateGame()
    {
        return new Game(
            "Chiefs",
            "Bills",
            DateTime.UtcNow.AddDays(1),
            DateTime.UtcNow.AddDays(1).AddHours(3)
        );
    }


    private class FakeScoringRule : IPickScoringRule
    {
        private readonly int _points;


        public FakeScoringRule(int points)
        {
            _points = points;
        }


        public int CalculatePoints(
            PickEvaluationContext context)
        {
            return _points;
        }
    }
}