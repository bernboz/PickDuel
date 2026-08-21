using NUnit.Framework;
using PickDuel.Domain.Entities;
using PickDuel.Domain.Entities.Standings;
using PickDuel.Domain.Entities.Matchups;
using PickDuel.Tests.Common;

namespace PickDuel.Tests.Domain.Standings;

public class SeasonStandingsTests
{
    [Test]
    public void Constructor_ShouldInitializeCorrectly()
    {
        var season = CreateSeason();

        var standings = new SeasonStandings(
            season
        );

        Assert.Multiple(() =>
        {
            Assert.That(
                standings.Season,
                Is.EqualTo(season)
            );

            Assert.That(
                standings.Standings,
                Is.Empty
            );

            Assert.That(
                standings.CreatedAt,
                Is.LessThanOrEqualTo(DateTime.UtcNow)
            );
        });
    }


    [Test]
    public void Constructor_ShouldThrow_WhenSeasonIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new SeasonStandings(
                null!
            ));
    }


    [Test]
    public void AddUser_ShouldCreateSeasonStanding()
    {
        var standings = CreateStandings();

        var user = TestDataFactory.CreateUser();

        standings.AddUser(user);

        Assert.Multiple(() =>
        {
            Assert.That(
                standings.Standings.Count,
                Is.EqualTo(1)
            );

            Assert.That(
                standings.Standings.First().User,
                Is.EqualTo(user)
            );
        });
    }


    [Test]
    public void AddUser_ShouldThrow_WhenUserIsNull()
    {
        var standings = CreateStandings();

        Assert.Throws<ArgumentNullException>(() =>
            standings.AddUser(
                null!
            ));
    }


    [Test]
    public void AddUser_ShouldThrow_WhenUserAlreadyExists()
    {
        var standings = CreateStandings();

        var user = TestDataFactory.CreateUser();

        standings.AddUser(user);

        Assert.Throws<InvalidOperationException>(() =>
            standings.AddUser(user)
        );
    }


    [Test]
    public void ProcessMatchup_ShouldUpdateBothUsersStatistics()
    {
        var standings = CreateStandings();

        var userOne = TestDataFactory.CreateUser();
        var userTwo = TestDataFactory.CreateUser();

        standings.AddUser(userOne);
        standings.AddUser(userTwo);

        var matchup = CreateCompletedMatchup(
            standings.Season.League,
            userOne,
            userTwo,
            50,
            25
        );

        standings.Season.AddMatchup(matchup);

        standings.ProcessMatchup(matchup);

        var userOneStanding =
            standings.Standings
                .First(x => x.User == userOne);

        var userTwoStanding =
            standings.Standings
                .First(x => x.User == userTwo);

        Assert.Multiple(() =>
        {
            Assert.That(
                userOneStanding.Wins,
                Is.EqualTo(1)
            );

            Assert.That(
                userOneStanding.TotalPoints,
                Is.EqualTo(50)
            );

            Assert.That(
                userTwoStanding.Losses,
                Is.EqualTo(1)
            );

            Assert.That(
                userTwoStanding.TotalPoints,
                Is.EqualTo(25)
            );
        });
    }


    [Test]
    public void ProcessMatchup_ShouldThrow_WhenUserIsNotInStandings()
    {
        var standings = CreateStandings();

        var userOne = TestDataFactory.CreateUser();
        var userTwo = TestDataFactory.CreateUser();

        standings.AddUser(userOne);

        var matchup = CreateCompletedMatchup(
            standings.Season.League,
            userOne,
            userTwo,
            50,
            25
        );

        standings.Season.AddMatchup(matchup);

        Assert.Throws<InvalidOperationException>(() =>
            standings.ProcessMatchup(matchup)
        );
    }


    [Test]
    public void ProcessMatchup_ShouldThrow_WhenMatchupBelongsToDifferentSeason()
    {
        var standings = CreateStandings();

        var userOne = TestDataFactory.CreateUser();
        var userTwo = TestDataFactory.CreateUser();

        standings.AddUser(userOne);
        standings.AddUser(userTwo);

        var differentSeason =
            CreateSeason();

        var matchup = CreateCompletedMatchup(
            differentSeason.League,
            userOne,
            userTwo,
            50,
            25
        );

        Assert.Throws<InvalidOperationException>(() =>
            standings.ProcessMatchup(matchup)
        );
    }


    [Test]
    public void GetLeader_ShouldReturnHighestRankedUser()
    {
        var standings = CreateStandings();

        var userOne = TestDataFactory.CreateUser();
        var userTwo = TestDataFactory.CreateUser();

        standings.AddUser(userOne);
        standings.AddUser(userTwo);

        var matchup = CreateCompletedMatchup(
            standings.Season.League,
            userOne,
            userTwo,
            50,
            25
        );

        standings.Season.AddMatchup(matchup);

        standings.ProcessMatchup(matchup);

        var leader = standings.GetLeader();

        Assert.That(
            leader.User,
            Is.EqualTo(userOne)
        );
    }


    [Test]
    public void AssignPlayoffQualification_ShouldMarkTopUsers()
    {
        var standings = CreateStandings();

        var userOne = TestDataFactory.CreateUser();
        var userTwo = TestDataFactory.CreateUser();

        standings.AddUser(userOne);
        standings.AddUser(userTwo);

        standings.AssignPlayoffQualification(1);

        var first =
            standings.Standings.First(
                x => x.User == userOne
            );

        Assert.That(
            first.MadePlayoffs,
            Is.True
        );
    }


    [Test]
    public void CrownChampion_ShouldMarkUserChampion()
    {
        var standings = CreateStandings();

        var user = TestDataFactory.CreateUser();

        standings.AddUser(user);

        standings.AssignPlayoffQualification(1);

        standings.CrownChampion(user);

        var standing =
            standings.Standings.First();

        Assert.Multiple(() =>
        {
            Assert.That(
                standing.IsChampion,
                Is.True
            );

            Assert.That(
                standing.MadePlayoffs,
                Is.True
            );
        });
    }


    [Test]
    public void CrownChampion_ShouldThrow_WhenUserNotInStandings()
    {
        var standings = CreateStandings();

        Assert.Throws<InvalidOperationException>(() =>
            standings.CrownChampion(
                TestDataFactory.CreateUser()
            ));
    }


    private static SeasonStandings CreateStandings()
    {
        return new SeasonStandings(
            CreateSeason()
        );
    }

    [Test]
    public void GetPlayoffQualifiers_ShouldReturnOnlyUsersWhoMadePlayoffs()
    {
        var standings = CreateStandings();

        var playoffUser = TestDataFactory.CreateUser();
        var nonPlayoffUser = TestDataFactory.CreateUser();

        standings.AddUser(playoffUser);
        standings.AddUser(nonPlayoffUser);

        standings.AssignPlayoffQualification(1);

        var qualifiers = standings.GetPlayoffQualifiers();

        Assert.Multiple(() =>
        {
            Assert.That(
                qualifiers.Count,
                Is.EqualTo(1)
            );

            Assert.That(
                qualifiers.First().User,
                Is.EqualTo(playoffUser)
            );

            Assert.That(
                qualifiers.First().MadePlayoffs,
                Is.True
            );
        });
    }

    [Test]
    public void GetPlayoffQualifiers_ShouldReturnEmpty_WhenNoUsersQualified()
    {
        var standings = CreateStandings();

        standings.AddUser(
            TestDataFactory.CreateUser()
        );

        var qualifiers = standings.GetPlayoffQualifiers();

        Assert.That(
            qualifiers,
            Is.Empty
        );
    }
    
    [Test]
    public void GetPlayoffQualifiers_ShouldReturnQualifiedUsersInRankOrder()
    {
        var standings = CreateStandings();

        var userOne = TestDataFactory.CreateUser();
        var userTwo = TestDataFactory.CreateUser();
        var userThree = TestDataFactory.CreateUser();

        standings.AddUser(userOne);
        standings.AddUser(userTwo);
        standings.AddUser(userThree);

        standings.Standings.First(x => x.User == userOne)
            .UpdateRank(2);

        standings.Standings.First(x => x.User == userTwo)
            .UpdateRank(1);

        standings.Standings.First(x => x.User == userThree)
            .UpdateRank(3);

        standings.Standings.First(x => x.User == userOne)
            .SetPlayoffStatus(true);

        standings.Standings.First(x => x.User == userTwo)
            .SetPlayoffStatus(true);

        var qualifiers = standings.GetPlayoffQualifiers();

        Assert.Multiple(() =>
        {
            Assert.That(qualifiers.Count, Is.EqualTo(2));

            Assert.That(
                qualifiers[0].User,
                Is.EqualTo(userTwo)
            );

            Assert.That(
                qualifiers[1].User,
                Is.EqualTo(userOne)
            );
        });
    }
    
    private static LeagueSeason CreateSeason()
    {
        var user = TestDataFactory.CreateUser();

        var league =
            TestDataFactory.CreateLeague(user);

        return new LeagueSeason(
            league,
            "2026 Season",
            2026,
            DateTime.UtcNow,
            DateTime.UtcNow.AddMonths(6)
        );
    }


    private static LeagueMatchup CreateCompletedMatchup(
        League league,
        User userOne,
        User userTwo,
        int userOnePoints,
        int userTwoPoints)
    {
        var matchup = new LeagueMatchup(
            league,
            userOne,
            userTwo,
            DateTime.UtcNow,
            DateTime.UtcNow.AddDays(7)
        );

        matchup.AddPickHistory(
            TestDataFactory.CreatePickHistory(
                userOne,
                league,
                userOnePoints
            ));

        matchup.AddPickHistory(
            TestDataFactory.CreatePickHistory(
                userTwo,
                league,
                userTwoPoints
            ));

        matchup.Lock();

        matchup.Complete();

        return matchup;
    }
}