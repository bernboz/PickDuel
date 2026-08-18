using NUnit.Framework;
using PickDuel.Domain.Entities;
using PickDuel.Domain.Entities.History;
using PickDuel.Domain.Entities.Matchups;
using PickDuel.Domain.Enums;
using PickDuel.Tests.Common;

namespace PickDuel.Tests.Domain.Matchups;

public class LeagueMatchupTests
{
    [Test]
    public void Constructor_ShouldInitializeCorrectly()
    {
        var league = TestDataFactory.CreateLeague(
            TestDataFactory.CreateUser()
        );

        var userOne = TestDataFactory.CreateUser();
        var userTwo = TestDataFactory.CreateUser();

        var start = DateTime.UtcNow;
        var end = start.AddDays(7);

        var matchup = new LeagueMatchup(
            league,
            userOne,
            userTwo,
            start,
            end
        );

        Assert.Multiple(() =>
        {
            Assert.That(matchup.League, Is.EqualTo(league));
            Assert.That(matchup.UserOne, Is.EqualTo(userOne));
            Assert.That(matchup.UserTwo, Is.EqualTo(userTwo));

            Assert.That(matchup.StartDate, Is.EqualTo(start));
            Assert.That(matchup.EndDate, Is.EqualTo(end));

            Assert.That(
                matchup.Status,
                Is.EqualTo(MatchupStatus.Scheduled)
            );

            Assert.That(
                matchup.Result,
                Is.EqualTo(MatchupResult.Pending)
            );

            Assert.That(matchup.UserOnePoints, Is.Zero);
            Assert.That(matchup.UserTwoPoints, Is.Zero);

            Assert.That(
                matchup.PickHistories,
                Is.Empty
            );
        });
    }


    [Test]
    public void Constructor_ShouldThrow_WhenUsersAreTheSame()
    {
        var user = TestDataFactory.CreateUser();

        Assert.Throws<ArgumentException>(() =>
            CreateMatchup(
                user,
                user
            ));
    }


    [Test]
    public void Constructor_ShouldThrow_WhenStartDateIsAfterEndDate()
    {
        var userOne = TestDataFactory.CreateUser();
        var userTwo = TestDataFactory.CreateUser();

        Assert.Throws<ArgumentException>(() =>
            new LeagueMatchup(
                TestDataFactory.CreateLeague(userOne),
                userOne,
                userTwo,
                DateTime.UtcNow.AddDays(5),
                DateTime.UtcNow
            ));
    }


    [Test]
    public void AddPickHistory_ShouldAddHistory_WhenScheduled()
    {
        var matchup = CreateMatchup();

        var history = CreateHistory(
            matchup.UserOne,
            matchup.League,
            50
        );

        matchup.AddPickHistory(history);

        Assert.That(
            matchup.PickHistories.Count,
            Is.EqualTo(1)
        );
    }


    [Test]
    public void AddPickHistory_ShouldCalculateUserOnePoints()
    {
        var matchup = CreateMatchup();

        matchup.AddPickHistory(
            CreateHistory(
                matchup.UserOne,
                matchup.League,
                50
            ));

        Assert.That(
            matchup.UserOnePoints,
            Is.EqualTo(50)
        );
    }


    [Test]
    public void AddPickHistory_ShouldCalculateUserTwoPoints()
    {
        var matchup = CreateMatchup();

        matchup.AddPickHistory(
            CreateHistory(
                matchup.UserTwo,
                matchup.League,
                25
            ));

        Assert.That(
            matchup.UserTwoPoints,
            Is.EqualTo(25)
        );
    }


    [Test]
    public void AddPickHistory_ShouldThrow_WhenMatchupIsLocked()
    {
        var matchup = CreateMatchup();

        matchup.Lock();

        Assert.Throws<InvalidOperationException>(() =>
            matchup.AddPickHistory(
                CreateHistory(
                    matchup.UserOne,
                    matchup.League,
                    10
                )));
    }


    [Test]
    public void AddPickHistory_ShouldThrow_WhenUserIsNotInMatchup()
    {
        var matchup = CreateMatchup();

        var outsider = TestDataFactory.CreateUser();

        var history = CreateHistory(
            outsider,
            matchup.League,
            10
        );

        Assert.Throws<InvalidOperationException>(() =>
            matchup.AddPickHistory(history)
        );
    }


    [Test]
    public void Complete_ShouldDetermineUserOneWin()
    {
        var matchup = CreateMatchup();

        matchup.AddPickHistory(
            CreateHistory(
                matchup.UserOne,
                matchup.League,
                50
            ));

        matchup.AddPickHistory(
            CreateHistory(
                matchup.UserTwo,
                matchup.League,
                25
            ));

        matchup.Lock();

        matchup.Complete();

        Assert.Multiple(() =>
        {
            Assert.That(
                matchup.Result,
                Is.EqualTo(MatchupResult.UserOneWin)
            );

            Assert.That(
                matchup.Status,
                Is.EqualTo(MatchupStatus.Completed)
            );
        });
    }


    [Test]
    public void Complete_ShouldDetermineUserTwoWin()
    {
        var matchup = CreateMatchup();

        matchup.AddPickHistory(
            CreateHistory(
                matchup.UserOne,
                matchup.League,
                10
            ));

        matchup.AddPickHistory(
            CreateHistory(
                matchup.UserTwo,
                matchup.League,
                50
            ));

        matchup.Lock();

        matchup.Complete();

        Assert.That(
            matchup.Result,
            Is.EqualTo(MatchupResult.UserTwoWin)
        );
    }


    [Test]
    public void Complete_ShouldAllowTie()
    {
        var matchup = CreateMatchup();

        matchup.AddPickHistory(
            CreateHistory(
                matchup.UserOne,
                matchup.League,
                25
            ));

        matchup.AddPickHistory(
            CreateHistory(
                matchup.UserTwo,
                matchup.League,
                25
            ));

        matchup.Lock();

        matchup.Complete();

        Assert.That(
            matchup.Result,
            Is.EqualTo(MatchupResult.Tie)
        );
    }


    [Test]
    public void AddPickHistory_ShouldThrow_WhenMatchupIsCompleted()
    {
        var matchup = CreateMatchup();

        matchup.Lock();
        matchup.Complete();

        Assert.Throws<InvalidOperationException>(() =>
            matchup.AddPickHistory(
                CreateHistory(
                    matchup.UserOne,
                    matchup.League,
                    10
                )));
    }

    [Test]
    public void Lock_ShouldChangeStatusToLocked()
    {
        var matchup = CreateMatchup();

        matchup.Lock();

        Assert.That(
            matchup.Status,
            Is.EqualTo(MatchupStatus.Locked)
        );
    }
    
    [Test]
    public void Lock_ShouldThrow_WhenAlreadyLocked()
    {
        var matchup = CreateMatchup();

        matchup.Lock();

        Assert.Throws<InvalidOperationException>(() =>
            matchup.Lock()
        );
    }
    
    [Test]
    public void Complete_ShouldThrow_WhenMatchupIsNotLocked()
    {
        var matchup = CreateMatchup();

        Assert.Throws<InvalidOperationException>(() =>
            matchup.Complete()
        );
    }
    
    [Test]
    public void Complete_ShouldSetCompletedAt()
    {
        var matchup = CreateMatchup();

        matchup.AddPickHistory(
            CreateHistory(
                matchup.UserOne,
                matchup.League,
                50
            ));

        matchup.Lock();

        var before = DateTime.UtcNow;

        matchup.Complete();

        var after = DateTime.UtcNow;

        Assert.That(
            matchup.CompletedAt,
            Is.InRange(before, after)
        );
    }
    
    [Test]
    public void Lock_ShouldThrow_WhenMatchupIsCompleted()
    {
        var matchup = CreateMatchup();

        matchup.Lock();
        matchup.Complete();

        Assert.Throws<InvalidOperationException>(() =>
            matchup.Lock()
        );
    }
    
    [Test]
    public void Complete_ShouldThrow_WhenAlreadyCompleted()
    {
        var matchup = CreateMatchup();

        matchup.Lock();
        matchup.Complete();

        Assert.Throws<InvalidOperationException>(() =>
            matchup.Complete()
        );
    }

    private static LeagueMatchup CreateMatchup(
        User? userOne = null,
        User? userTwo = null)
    {
        userOne ??= TestDataFactory.CreateUser();
        userTwo ??= TestDataFactory.CreateUser();

        return new LeagueMatchup(
            TestDataFactory.CreateLeague(userOne),
            userOne,
            userTwo,
            DateTime.UtcNow.AddHours(-1),
            DateTime.UtcNow.AddDays(7)
        );
    }


    private static PickHistory CreateHistory(
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