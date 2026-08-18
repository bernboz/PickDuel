using PickDuel.Application.Scoring;
using PickDuel.Domain.Entities;
using PickDuel.Domain.Enums;
using PickDuel.Domain.Entities.Predictions;
using PickDuel.Domain.Entities.History;
using PickDuel.Domain.ValueObjects;

namespace PickDuel.Tests.Common;

public static class TestDataFactory
{
    /// <summary>
    /// Creates a test user with unique identifying information.
    /// </summary>
    /// <returns>A new User entity for testing purposes.</returns>
    public static User CreateUser()
    {
        return new User(
            "Bob",
            "Smith",
            $"bob-{Guid.NewGuid()}@test.com",
            $"bob-{Guid.NewGuid()}"
        );
    }


    /// <summary>
    /// Creates a test league owned by the provided user.
    /// </summary>
    /// <param name="user">The owner of the league.</param>
    /// <returns>A new League entity for testing purposes.</returns>
    public static League CreateLeague(User user)
    {
        return new League(
            "Test League",
            SportType.NFL,
            user
        );
    }


    /// <summary>
    /// Creates a future game between two teams.
    /// </summary>
    /// <returns>A game scheduled in the future.</returns>
    public static Game CreateGame()
    {
        return new Game(
            "Chiefs",
            "Bills",
            DateTime.UtcNow.AddHours(1),
            DateTime.UtcNow.AddHours(4)
        );
    }


    /// <summary>
    /// Creates a pick for the specified team and confidence multiplier.
    /// </summary>
    /// <param name="user">User making the prediction.</param>
    /// <param name="league">League where the prediction exists.</param>
    /// <param name="game">Game being predicted.</param>
    /// <param name="selectedTeam">Team selected by the user.</param>
    /// <param name="confidenceMultiplier">Confidence value from 1-5.</param>
    /// <returns>A configured Pick entity.</returns>
    public static Pick CreatePick(User user, League league, Game game, string selectedTeam, int confidenceMultiplier = 3)
    {
        return new Pick(
            user,
            league,
            game,
            selectedTeam,
            confidenceMultiplier
        );
    }


    /// <summary>
    /// Creates a completed game result where the home team wins.
    /// </summary>
    /// <param name="game">Game associated with the result.</param>
    /// <returns>A completed GameResult entity.</returns>
    public static GameResult CreateHomeWinResult(Game game)
    {
        return new GameResult(
            game,
            GameOutcome.HomeWin,
            24,
            14
        );
    }


    /// <summary>
    /// Creates a completed game result where the away team wins.
    /// </summary>
    /// <param name="game">Game associated with the result.</param>
    /// <returns>A completed GameResult entity.</returns>
    public static GameResult CreateAwayWinResult(Game game)
    {
        return new GameResult(
            game,
            GameOutcome.AwayWin,
            14,
            24
        );
    }


    /// <summary>
    /// Creates game odds using implied probabilities.
    /// </summary>
    /// <param name="game">Game associated with the odds.</param>
    /// <param name="homeProbability">Probability of the home team winning.</param>
    /// <param name="awayProbability">Probability of the away team winning.</param>
    /// <returns>A GameOdds entity.</returns>
    public static GameOdds CreateGameOdds(Game game, decimal homeProbability = 0.75m, decimal awayProbability = 0.25m)
    {
        return new GameOdds(
            game,
            homeProbability,
            awayProbability
        );
    }


    /// <summary>
    /// Creates a scoring context where the home team prediction is correct.
    /// </summary>
    /// <param name="confidenceMultiplier">Confidence assigned to the prediction.</param>
    /// <param name="homeProbability">Probability of the home team winning.</param>
    /// <param name="awayProbability">Probability of the away team winning.</param>
    /// <returns>A PickEvaluationContext containing a correct home prediction.</returns>
    public static PickEvaluationContext CreateCorrectPredictionContext(
        int confidenceMultiplier = 3,
        decimal homeProbability = 0.75m,
        decimal awayProbability = 0.25m)
    {
        var user = CreateUser();
        var league = CreateLeague(user);
        var game = CreateGame();

        var pick = CreatePick(
            user,
            league,
            game,
            game.HomeTeam,
            confidenceMultiplier
        );

        var result = CreateHomeWinResult(game);
        var odds = CreateGameOdds(game, homeProbability, awayProbability);

        return new PickEvaluationContext(
            pick,
            result,
            odds
        );
    }


    /// <summary>
    /// Creates a scoring context where the away team prediction is correct.
    /// </summary>
    /// <param name="confidenceMultiplier">Confidence assigned to the prediction.</param>
    /// <param name="homeProbability">Probability of the home team winning.</param>
    /// <param name="awayProbability">Probability of the away team winning.</param>
    /// <returns>A PickEvaluationContext containing a correct away prediction.</returns>
    public static PickEvaluationContext CreateCorrectAwayPredictionContext(
        int confidenceMultiplier = 3,
        decimal homeProbability = 0.25m,
        decimal awayProbability = 0.75m)
    {
        var user = CreateUser();
        var league = CreateLeague(user);
        var game = CreateGame();

        var pick = CreatePick(
            user,
            league,
            game,
            game.AwayTeam,
            confidenceMultiplier
        );

        var result = CreateAwayWinResult(game);
        var odds = CreateGameOdds(game, homeProbability, awayProbability);

        return new PickEvaluationContext(
            pick,
            result,
            odds
        );
    }


    /// <summary>
    /// Creates a scoring context where the user's prediction is incorrect.
    /// </summary>
    /// <param name="confidenceMultiplier">Confidence assigned to the prediction.</param>
    /// <returns>A PickEvaluationContext containing an incorrect prediction.</returns>
    public static PickEvaluationContext CreateIncorrectPredictionContext(int confidenceMultiplier = 3)
    {
        var user = CreateUser();
        var league = CreateLeague(user);
        var game = CreateGame();

        var pick = CreatePick(user, league, game, game.AwayTeam, confidenceMultiplier);

        var result = CreateHomeWinResult(game);
        var odds = CreateGameOdds(game);

        return new PickEvaluationContext(pick, result, odds);
    }
    
    /// <summary>
    /// Creates a future pick with default test values.
    /// </summary>
    /// <param name="confidenceMultiplier">Confidence value from 1-5.</param>
    /// <param name="selectedTeam">Team selected by the user.</param>
    /// <returns>A Pick entity scheduled before game start.</returns>
    public static Pick CreateFuturePick(int confidenceMultiplier = 3, string? selectedTeam = null)
    {
        var user = CreateUser();
        var league = CreateLeague(user);
        var game = CreateFutureGame();

        return new Pick(
            user,
            league,
            game,
            selectedTeam ?? game.HomeTeam,
            confidenceMultiplier
        );
    }


    /// <summary>
    /// Creates a pick for a game that has already started.
    /// </summary>
    /// <returns>A locked Pick entity.</returns>
    public static Pick CreateStartedPick()
    {
        var user = CreateUser();
        var league = CreateLeague(user);
        var game = CreateStartedGame();

        return new Pick(
            user,
            league,
            game,
            game.HomeTeam,
            3
        );
    }


    /// <summary>
    /// Creates a future game for testing pick behavior.
    /// </summary>
    /// <returns>A game scheduled in the future.</returns>
    public static Game CreateFutureGame()
    {
        return new Game(
            "Chiefs",
            "Bills",
            DateTime.UtcNow.AddHours(1),
            DateTime.UtcNow.AddHours(4)
        );
    }


    /// <summary>
    /// Creates a game that has already started.
    /// </summary>
    /// <returns>A started game.</returns>
    public static Game CreateStartedGame()
    {
        return new Game(
            "Chiefs",
            "Bills",
            DateTime.UtcNow.AddHours(-4),
            DateTime.UtcNow.AddHours(-1)
        );
    }
        
        /// <summary>
    /// Creates a scoring context where the user's predicted score exactly matches the final game result.
    /// Used for testing exact score bonus calculations.
    /// </summary>
    /// <returns>A PickEvaluationContext containing an exact score prediction.</returns>
    public static PickEvaluationContext CreateExactScorePredictionContext()
    {
        var user = CreateUser();
        var league = CreateLeague(user);
        var game = CreateGame();

        var pick = CreatePick(
            user,
            league,
            game,
            game.HomeTeam
        );

        pick.UpdateScorePrediction(
            new ScorePrediction(24, 14)
        );

        var result = new GameResult(
            game,
            GameOutcome.HomeWin,
            24,
            14
        );

        var odds = CreateGameOdds(game);

        return new PickEvaluationContext(
            pick,
            result,
            odds
        );
    }


    /// <summary>
    /// Creates a scoring context where the predicted score is within the acceptable accuracy range.
    /// Used for testing partial score accuracy bonuses.
    /// </summary>
    /// <returns>A PickEvaluationContext containing a close score prediction.</returns>
    public static PickEvaluationContext CreateCloseScorePredictionContext()
    {
        var user = CreateUser();
        var league = CreateLeague(user);
        var game = CreateGame();

        var pick = CreatePick(
            user,
            league,
            game,
            game.HomeTeam
        );

        // Within 5 points of both scores
        pick.UpdateScorePrediction(
            new ScorePrediction(27, 18)
        );

        var result = new GameResult(
            game,
            GameOutcome.HomeWin,
            24,
            14
        );

        var odds = CreateGameOdds(game);

        return new PickEvaluationContext(
            pick,
            result,
            odds
        );
    }


    /// <summary>
    /// Creates a scoring context where the predicted score is significantly different from the final result.
    /// Used for testing cases where no accuracy bonus should be awarded.
    /// </summary>
    /// <returns>A PickEvaluationContext containing an inaccurate score prediction.</returns>
    public static PickEvaluationContext CreateIncorrectScoreContext()
    {
        var user = CreateUser();
        var league = CreateLeague(user);
        var game = CreateGame();

        var pick = CreatePick(
            user,
            league,
            game,
            game.HomeTeam
        );

        // More than tolerance but not necessarily max penalty
        pick.UpdateScorePrediction(
            new ScorePrediction(40, 3)
        );

        var result = new GameResult(
            game,
            GameOutcome.HomeWin,
            24,
            14
        );

        var odds = CreateGameOdds(game);

        return new PickEvaluationContext(
            pick,
            result,
            odds
        );
    }

    /// <summary>
    /// Creates a scoring context where only the home team's predicted score differs within the accuracy range.
    /// Used for testing home score accuracy calculations.
    /// </summary>
    /// <returns>A PickEvaluationContext containing a home score mismatch.</returns>
    public static PickEvaluationContext CreateHomeScoreMismatchContext()
    {
        var user = CreateUser();
        var league = CreateLeague(user);
        var game = CreateGame();

        var pick = CreatePick(
            user,
            league,
            game,
            game.HomeTeam
        );

        pick.UpdateScorePrediction(
            new ScorePrediction(27, 14)
        );

        var result = new GameResult(
            game,
            GameOutcome.HomeWin,
            24,
            14
        );

        var odds = CreateGameOdds(game);

        return new PickEvaluationContext(
            pick,
            result,
            odds
        );
    }

    /// <summary>
    /// Creates a scoring context where only the away team's predicted score differs within the accuracy range.
    /// Used for testing away score accuracy calculations.
    /// </summary>
    /// <returns>A PickEvaluationContext containing an away score mismatch.</returns>
    public static PickEvaluationContext CreateAwayScoreMismatchContext()
    {
        var user = CreateUser();
        var league = CreateLeague(user);
        var game = CreateGame();

        var pick = CreatePick(
            user,
            league,
            game,
            game.HomeTeam
        );

        pick.UpdateScorePrediction(
            new ScorePrediction(24, 18)
        );

        var result = new GameResult(
            game,
            GameOutcome.HomeWin,
            24,
            14
        );

        var odds = CreateGameOdds(game);

        return new PickEvaluationContext(
            pick,
            result,
            odds
        );
    }
    
    /// <summary>
    /// Creates a completed pick history record for a user within a league.
    /// Used for testing matchup completion and season standing calculations.
    /// </summary>
    /// <param name="user">User associated with the pick history.</param>
    /// <param name="league">League where the pick occurred.</param>
    /// <param name="points">Points earned from the prediction.</param>
    /// <returns>A completed PickHistory entity for testing purposes.</returns>
    public static PickHistory CreatePickHistory(User user, League league, int points)
    {
        var game = CreateGame();

        return new PickHistory(
            user,
            league,
            game,
            game.HomeTeam,
            null,
            GameOutcome.HomeWin,
            24,
            14,
            points,
            ScoreEventType.CorrectWinner
        );
    }
}