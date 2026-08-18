using NUnit.Framework;
using PickDuel.Domain.Entities;
using PickDuel.Domain.Entities.History;
using PickDuel.Domain.Entities.Matchups;
using PickDuel.Domain.Enums;
using PickDuel.Tests.Common;

namespace PickDuel.Tests.Domain.Standings;

public class SeasonStandingTests
{
    [Test]
    public void Constructor_ShouldInitializeCorrectly()
    {
        var user = TestDataFactory.CreateUser();
        var season = CreateSeason(user);

        var standing = new SeasonStanding(
            season,
            user
        );

        Assert.Multiple(() =>
        {
            Assert.That(standing.Season, Is.EqualTo(season));
            Assert.That(standing.User, Is.EqualTo(user));

            Assert.That(standing.Rank, Is.EqualTo(1));

            Assert.That(standing.TotalPoints, Is.Zero);
            Assert.That(standing.PointsFor, Is.Zero);
            Assert.That(standing.PointsAgainst, Is.Zero);

            Assert.That(standing.Wins, Is.Zero);
            Assert.That(standing.Losses, Is.Zero);
            Assert.That(standing.Ties, Is.Zero);

            Assert.That(standing.MatchupsPlayed, Is.Zero);

            Assert.That(standing.MadePlayoffs, Is.False);
            Assert.That(standing.IsChampion, Is.False);

            Assert.That(
                standing.Matchups,
                Is.Empty
            );
        });
    }


    [Test]
    public void Constructor_ShouldThrow_WhenSeasonIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new SeasonStanding(
                null!,
                TestDataFactory.CreateUser()
            ));
    }


    [Test]
    public void Constructor_ShouldThrow_WhenUserIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new SeasonStanding(
                CreateSeason(
                    TestDataFactory.CreateUser()
                ),
                null!
            ));
    }


    [Test]
    public void UpdateFromMatchup_ShouldUpdateWinStatistics()
    {
        var standing = CreateStanding();

        var matchup = CreateCompletedMatchup(
            standing.Season,
            standing.User,
            50,
            25
        );

        standing.UpdateFromMatchup(matchup);

        Assert.Multiple(() =>
        {
            Assert.That(standing.Wins, Is.EqualTo(1));
            Assert.That(standing.Losses, Is.Zero);
            Assert.That(standing.Ties, Is.Zero);
        });
    }


    [Test]
    public void UpdateFromMatchup_ShouldUpdateLossStatistics()
    {
        var standing = CreateStanding();

        var matchup = CreateCompletedMatchup(
            standing.Season,
            standing.User,
            25,
            50
        );

        standing.UpdateFromMatchup(matchup);

        Assert.Multiple(() =>
        {
            Assert.That(standing.Losses, Is.EqualTo(1));
            Assert.That(standing.Wins, Is.Zero);
        });
    }


    [Test]
    public void UpdateFromMatchup_ShouldUpdateTieStatistics()
    {
        var standing = CreateStanding();

        var matchup = CreateCompletedMatchup(
            standing.Season,
            standing.User,
            25,
            25
        );

        standing.UpdateFromMatchup(matchup);

        Assert.Multiple(() =>
        {
            Assert.That(standing.Ties, Is.EqualTo(1));
            Assert.That(standing.Wins, Is.Zero);
            Assert.That(standing.Losses, Is.Zero);
        });
    }


    [Test]
    public void UpdateFromMatchup_ShouldUpdatePoints()
    {
        var standing = CreateStanding();

        var matchup = CreateCompletedMatchup(
            standing.Season,
            standing.User,
            75,
            40
        );

        standing.UpdateFromMatchup(matchup);

        Assert.Multiple(() =>
        {
            Assert.That(
                standing.TotalPoints,
                Is.EqualTo(75)
            );

            Assert.That(
                standing.PointsFor,
                Is.EqualTo(75)
            );

            Assert.That(
                standing.PointsAgainst,
                Is.EqualTo(40)
            );
        });
    }


    [Test]
    public void UpdateFromMatchup_ShouldIncrementMatchupsPlayed()
    {
        var standing = CreateStanding();

        var matchup = CreateCompletedMatchup(
            standing.Season,
            standing.User,
            50,
            25
        );

        standing.UpdateFromMatchup(matchup);

        Assert.That(
            standing.MatchupsPlayed,
            Is.EqualTo(1)
        );
    }


    [Test]
    public void UpdateFromMatchup_ShouldStoreMatchup()
    {
        var standing = CreateStanding();

        var matchup = CreateCompletedMatchup(
            standing.Season,
            standing.User,
            50,
            25
        );

        standing.UpdateFromMatchup(matchup);

        Assert.That(
            standing.Matchups.Count,
            Is.EqualTo(1)
        );
    }


    [Test]
    public void UpdateFromMatchup_ShouldThrow_WhenMatchupIsNotCompleted()
    {
        var standing = CreateStanding();

        var matchup = new LeagueMatchup(
            standing.Season.League,
            standing.User,
            TestDataFactory.CreateUser(),
            DateTime.UtcNow,
            DateTime.UtcNow.AddDays(7)
        );

        Assert.Throws<InvalidOperationException>(() =>
            standing.UpdateFromMatchup(matchup)
        );
    }


    [Test]
    public void UpdateFromMatchup_ShouldThrow_WhenUserDidNotParticipate()
    {
        var standing = CreateStanding();

        var outsider = TestDataFactory.CreateUser();

        var matchup = CreateCompletedMatchup(
            standing.Season,
            outsider,
            50,
            25
        );

        Assert.Throws<InvalidOperationException>(() =>
            standing.UpdateFromMatchup(matchup)
        );
    }


    [Test]
    public void UpdateFromMatchup_ShouldThrow_WhenMatchupAlreadyApplied()
    {
        var standing = CreateStanding();

        var matchup = CreateCompletedMatchup(
            standing.Season,
            standing.User,
            50,
            25
        );

        standing.UpdateFromMatchup(matchup);

        Assert.Throws<InvalidOperationException>(() =>
            standing.UpdateFromMatchup(matchup)
        );
    }


    [Test]
    public void UpdateRank_ShouldUpdateRank()
    {
        var standing = CreateStanding();

        standing.UpdateRank(5);

        Assert.That(
            standing.Rank,
            Is.EqualTo(5)
        );
    }


    [Test]
    public void UpdateRank_ShouldThrow_WhenRankIsInvalid()
    {
        var standing = CreateStanding();

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            standing.UpdateRank(0)
        );
    }


    [Test]
    public void SetPlayoffStatus_ShouldUpdateStatus()
    {
        var standing = CreateStanding();

        standing.SetPlayoffStatus(true);

        Assert.That(
            standing.MadePlayoffs,
            Is.True
        );
    }


    [Test]
    public void CrownChampion_ShouldThrow_WhenUserDidNotMakePlayoffs()
    {
        var standing = CreateStanding();

        Assert.Throws<InvalidOperationException>(() =>
            standing.CrownChampion()
        );
    }


    [Test]
    public void CrownChampion_ShouldMarkUserAsChampion()
    {
        var standing = CreateStanding();

        standing.SetPlayoffStatus(true);

        standing.CrownChampion();

        Assert.That(
            standing.IsChampion,
            Is.True
        );
    }


    private static SeasonStanding CreateStanding()
    {
        var user = TestDataFactory.CreateUser();

        return new SeasonStanding(
            CreateSeason(user),
            user
        );
    }


    private static LeagueSeason CreateSeason(
        User user)
    {
        var league = TestDataFactory.CreateLeague(user);

        return new LeagueSeason(
            league,
            "2026 Season",
            2026,
            DateTime.UtcNow,
            DateTime.UtcNow.AddMonths(6)
        );
    }


    private static LeagueMatchup CreateCompletedMatchup(
        LeagueSeason season,
        User user,
        int userPoints,
        int opponentPoints)
    {
        var opponent = TestDataFactory.CreateUser();

        var matchup = new LeagueMatchup(
            season.League,
            user,
            opponent,
            DateTime.UtcNow,
            DateTime.UtcNow.AddDays(7)
        );


        matchup.AddPickHistory(
            CreatePickHistory(
                user,
                season.League,
                userPoints
            ));


        matchup.AddPickHistory(
            CreatePickHistory(
                opponent,
                season.League,
                opponentPoints
            ));


        season.AddMatchup(matchup);


        matchup.Lock();

        matchup.Complete();


        return matchup;
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