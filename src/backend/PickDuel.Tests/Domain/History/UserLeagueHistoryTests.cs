using NUnit.Framework;
using PickDuel.Domain.Entities;
using PickDuel.Domain.Entities.History;
using PickDuel.Domain.Entities.Matchups;
using PickDuel.Domain.Enums;
using PickDuel.Tests.Common;

namespace PickDuel.Tests.Domain.History;

public class UserLeagueHistoryTests
{
    [Test]
    public void Constructor_ShouldInitializeCorrectly()
    {
        var user = TestDataFactory.CreateUser();
        var league = TestDataFactory.CreateLeague(user);

        var history = new UserLeagueHistory(
            user,
            league
        );

        Assert.Multiple(() =>
        {
            Assert.That(history.User, Is.EqualTo(user));
            Assert.That(history.League, Is.EqualTo(league));

            Assert.That(history.Matchups, Is.Empty);

            Assert.That(history.TotalPoints, Is.Zero);

            Assert.That(history.MatchupWins, Is.Zero);
            Assert.That(history.MatchupLosses, Is.Zero);
            Assert.That(history.MatchupTies, Is.Zero);

            Assert.That(history.CurrentWinStreak, Is.Zero);
            Assert.That(history.LongestWinStreak, Is.Zero);

            Assert.That(
                history.CreatedAt,
                Is.LessThanOrEqualTo(DateTime.UtcNow)
            );
        });
    }


    [Test]
    public void Constructor_ShouldThrow_WhenUserIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new UserLeagueHistory(
                null!,
                TestDataFactory.CreateLeague(
                    TestDataFactory.CreateUser()
                )
            ));
    }


    [Test]
    public void Constructor_ShouldThrow_WhenLeagueIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new UserLeagueHistory(
                TestDataFactory.CreateUser(),
                null!
            ));
    }


    [Test]
    public void AddMatchup_ShouldAddCompletedMatchup()
    {
        var history = CreateHistory();

        var matchup = CreateCompletedMatchup(
            history.User,
            50,
            25,
            history.League
        );

        history.AddMatchup(matchup);

        Assert.That(
            history.Matchups.Count,
            Is.EqualTo(1)
        );
    }


    [Test]
    public void AddMatchup_ShouldUpdateTotalPoints()
    {
        var history = CreateHistory();

        var matchup = CreateCompletedMatchup(
            history.User,
            75,
            25,
            history.League
        );

        history.AddMatchup(matchup);

        Assert.That(
            history.TotalPoints,
            Is.EqualTo(75)
        );
    }


    [Test]
    public void AddMatchup_ShouldRecordWin()
    {
        var history = CreateHistory();

        var matchup = CreateCompletedMatchup(
            history.User,
            50,
            25,
            history.League
        );

        history.AddMatchup(matchup);

        Assert.Multiple(() =>
        {
            Assert.That(history.MatchupWins, Is.EqualTo(1));
            Assert.That(history.MatchupLosses, Is.Zero);
            Assert.That(history.MatchupTies, Is.Zero);
        });
    }


    [Test]
    public void AddMatchup_ShouldRecordLoss()
    {
        var history = CreateHistory();

        var matchup = CreateCompletedMatchup(
            history.User,
            25,
            50,
            history.League
        );

        history.AddMatchup(matchup);

        Assert.Multiple(() =>
        {
            Assert.That(history.MatchupLosses, Is.EqualTo(1));
            Assert.That(history.MatchupWins, Is.Zero);
        });
    }


    [Test]
    public void AddMatchup_ShouldRecordTie()
    {
        var history = CreateHistory();

        var matchup = CreateCompletedTieMatchup(
            history.User,
            history.League
        );

        history.AddMatchup(matchup);

        Assert.That(
            history.MatchupTies,
            Is.EqualTo(1)
        );
    }


    [Test]
    public void AddMatchup_ShouldIncreaseCurrentWinStreak()
    {
        var history = CreateHistory();

        var matchup = CreateCompletedMatchup(
            history.User,
            50,
            25,
            history.League
        );

        history.AddMatchup(matchup);

        Assert.That(
            history.CurrentWinStreak,
            Is.EqualTo(1)
        );
    }


    [Test]
    public void AddMatchup_ShouldTrackLongestWinStreak()
    {
        var history = CreateHistory();

        history.AddMatchup(
            CreateCompletedMatchup(
                history.User,
                50,
                25,
                history.League
            ));

        history.AddMatchup(
            CreateCompletedMatchup(
                history.User,
                75,
                25,
                history.League
            ));

        Assert.That(
            history.LongestWinStreak,
            Is.EqualTo(2)
        );
    }


    [Test]
    public void AddMatchup_ShouldResetCurrentStreakAfterLoss()
    {
        var history = CreateHistory();

        history.AddMatchup(
            CreateCompletedMatchup(
                history.User,
                50,
                25,
                history.League
            ));

        history.AddMatchup(
            CreateCompletedMatchup(
                history.User,
                25,
                50,
                history.League
            ));

        Assert.That(
            history.CurrentWinStreak,
            Is.Zero
        );
    }


    [Test]
    public void AddMatchup_ShouldResetCurrentStreakAfterTie()
    {
        var history = CreateHistory();

        history.AddMatchup(
            CreateCompletedMatchup(
                history.User,
                50,
                25,
                history.League
            ));

        history.AddMatchup(
            CreateCompletedTieMatchup(
                history.User,
                history.League
            ));

        Assert.That(
            history.CurrentWinStreak,
            Is.Zero
        );
    }


    [Test]
    public void AddMatchup_ShouldThrow_WhenMatchupIsNotCompleted()
    {
        var history = CreateHistory();

        var matchup = CreateScheduledMatchup(
            history.User,
            history.League
        );

        Assert.Throws<InvalidOperationException>(() =>
            history.AddMatchup(matchup)
        );
    }


    [Test]
    public void AddMatchup_ShouldThrow_WhenMatchupBelongsToDifferentLeague()
    {
        var history = CreateHistory();

        var differentLeagueUser =
            TestDataFactory.CreateUser();

        var differentLeague =
            TestDataFactory.CreateLeague(
                differentLeagueUser
            );

        var matchup = CreateCompletedMatchup(
            history.User,
            50,
            25,
            differentLeague
        );

        Assert.Throws<InvalidOperationException>(() =>
            history.AddMatchup(matchup)
        );
    }


    [Test]
    public void AddMatchup_ShouldThrow_WhenUserIsNotInMatchup()
    {
        var history = CreateHistory();

        var outsider = TestDataFactory.CreateUser();

        var matchup = CreateCompletedMatchup(
            outsider,
            50,
            25,
            history.League
        );

        Assert.Throws<InvalidOperationException>(() =>
            history.AddMatchup(matchup)
        );
    }


    [Test]
    public void AddMatchup_ShouldThrow_WhenDuplicateMatchupAdded()
    {
        var history = CreateHistory();

        var matchup = CreateCompletedMatchup(
            history.User,
            50,
            25,
            history.League
        );

        history.AddMatchup(matchup);

        Assert.Throws<InvalidOperationException>(() =>
            history.AddMatchup(matchup)
        );
    }


    private static UserLeagueHistory CreateHistory()
    {
        var user = TestDataFactory.CreateUser();

        var league = TestDataFactory.CreateLeague(user);

        return new UserLeagueHistory(
            user,
            league
        );
    }


    private static LeagueMatchup CreateCompletedMatchup(
        User user,
        int userPoints,
        int opponentPoints,
        League league)
    {
        var opponent = TestDataFactory.CreateUser();

        var matchup = new LeagueMatchup(
            league,
            user,
            opponent,
            DateTime.UtcNow,
            DateTime.UtcNow.AddDays(7)
        );

        matchup.AddPickHistory(
            CreatePickHistory(
                user,
                league,
                userPoints
            ));

        matchup.AddPickHistory(
            CreatePickHistory(
                opponent,
                league,
                opponentPoints
            ));

        matchup.Lock();

        matchup.Complete();

        return matchup;
    }


    private static LeagueMatchup CreateCompletedTieMatchup(
        User user,
        League league)
    {
        return CreateCompletedMatchup(
            user,
            25,
            25,
            league
        );
    }


    private static LeagueMatchup CreateScheduledMatchup(
        User user,
        League league)
    {
        return new LeagueMatchup(
            league,
            user,
            TestDataFactory.CreateUser(),
            DateTime.UtcNow,
            DateTime.UtcNow.AddDays(7)
        );
    }


    private static PickHistory CreatePickHistory(
        User user,
        League league,
        int points)
    {
        var game = TestDataFactory.CreateGame();

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