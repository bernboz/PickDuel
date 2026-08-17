using NUnit.Framework;
using PickDuel.Application.Scoring;
using PickDuel.Domain.Entities;
using PickDuel.Domain.Enums;

namespace PickDuel.Tests.Application;

public class PickEvaluationContextTests
{
    [Test]
    public void NewContext_ShouldInitializeCorrectly()
    {
        var user = new User(
            "Bob",
            "Smith",
            "bob@test.com",
            "bob"
        );

        var league = new League(
            "NFL",
            SportType.NFL,
            user
        );

        var game = new Game(
            "Chiefs",
            "Bills",
            DateTime.UtcNow,
            DateTime.UtcNow.AddHours(3)
        );

        var pick = new Pick(
            user,
            league,
            game,
            "Chiefs",
            3
        );

        var result = new GameResult(
            game,
            GameOutcome.HomeWin,
            27,
            24
        );

        var odds = new GameOdds(
            game,
            0.75m,
            0.25m
        );

        var context = new PickEvaluationContext(
            pick,
            result,
            odds
        );

        Assert.That(context.Pick, Is.EqualTo(pick));
        Assert.That(context.GameResult, Is.EqualTo(result));
    }

    [Test]
    public void NewContext_ShouldThrow_WhenGamesDoNotMatch()
    {
        var user = new User(
            "Bob",
            "Smith",
            "bob@test.com",
            "bob"
        );

        var league = new League(
            "NFL",
            SportType.NFL,
            user
        );

        var game1 = new Game(
            "Chiefs",
            "Bills",
            DateTime.UtcNow,
            DateTime.UtcNow.AddHours(3)
        );

        var game2 = new Game(
            "Packers",
            "Bears",
            DateTime.UtcNow,
            DateTime.UtcNow.AddHours(3)
        );

        var pick = new Pick(
            user,
            league,
            game1,
            "Chiefs",
            3
        );

        var result = new GameResult(
            game2,
            GameOutcome.HomeWin,
            21,
            17
        );
        var odds = new GameOdds(
            game1,
            0.75m,
            0.25m
        );
        Assert.Throws<ArgumentException>(() =>
            new PickEvaluationContext(
                pick,
                result,
                odds
            ));
    }
    
    [Test]
    public void PickEvaluationContext_ShouldThrow_WhenPickIsNull()
    {
        var user = CreateUser();
        var league = CreateLeague(user);
        var game = CreateGame();
        var result = CreateGameResult(game);
        var odds = CreateGameOdds(game);

        Assert.Throws<ArgumentNullException>(() =>
            new PickEvaluationContext(
                null!,
                result,
                odds
            ));
    }


    [Test]
    public void PickEvaluationContext_ShouldThrow_WhenGameOddsIsNull()
    {
        var user = CreateUser();
        var league = CreateLeague(user);
        var game = CreateGame();
        var pick = CreatePick(user, league, game);
        var result = CreateGameResult(game);

        Assert.Throws<ArgumentNullException>(() =>
            new PickEvaluationContext(
                pick,
                result,
                null!
            ));
    }

    private static User CreateUser()
    {
        return new User(
            "Bob",
            "Smith",
            Guid.NewGuid() + "@test.com",
            "bob" + Guid.NewGuid()
        );
    }


    private static League CreateLeague(User user)
    {
        return new League(
            "Test League",
            SportType.NFL,
            user
        );
    }


    private static Game CreateGame()
    {
        return new Game(
            "Chiefs",
            "Bills",
            DateTime.UtcNow.AddHours(1),
            DateTime.UtcNow.AddHours(3)
        );
    }


    private static Pick CreatePick(User user, League league, Game game)
    {
        return new Pick(
            user,
            league,
            game,
            game.HomeTeam,
            3
        );
    }


    private static GameResult CreateGameResult(Game game)
    {
        return new GameResult(
            game,
            GameOutcome.HomeWin,
            24,
            14
        );
    }


    private static GameOdds CreateGameOdds(Game game)
    {
        return new GameOdds(
            game,
            0.75m,
            0.25m
        );
    }
}