using NUnit.Framework;
using PickDuel.Domain.Entities;
using PickDuel.Domain.Entities.Matchups;
using PickDuel.Tests.Common;

namespace PickDuel.Tests.Domain;

public class LeagueSeasonTests
{
    [Test]
    public void Constructor_ShouldInitializeCorrectly()
    {
        var league = TestDataFactory.CreateLeague(
            TestDataFactory.CreateUser()
        );

        var start = DateTime.UtcNow;
        var end = start.AddMonths(6);

        var season = new LeagueSeason(
            league,
            "2026 NFL Season",
            2026,
            start,
            end
        );

        Assert.Multiple(() =>
        {
            Assert.That(season.League, Is.EqualTo(league));

            Assert.That(
                season.Name,
                Is.EqualTo("2026 NFL Season")
            );

            Assert.That(
                season.Year,
                Is.EqualTo(2026)
            );

            Assert.That(
                season.StartDate,
                Is.EqualTo(start)
            );

            Assert.That(
                season.EndDate,
                Is.EqualTo(end)
            );

            Assert.That(
                season.Matchups,
                Is.Empty
            );

            Assert.That(
                season.MatchupCount,
                Is.Zero
            );

            Assert.That(
                season.IsCompleted,
                Is.False
            );

            Assert.That(
                season.CreatedAt,
                Is.LessThanOrEqualTo(DateTime.UtcNow)
            );
        });
    }


    [Test]
    public void Constructor_ShouldThrow_WhenLeagueIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new LeagueSeason(
                null!,
                "2026 Season",
                2026,
                DateTime.UtcNow,
                DateTime.UtcNow.AddMonths(6)
            ));
    }


    [Test]
    public void Constructor_ShouldThrow_WhenNameIsEmpty()
    {
        Assert.Throws<ArgumentException>(() =>
            new LeagueSeason(
                TestDataFactory.CreateLeague(
                    TestDataFactory.CreateUser()
                ),
                "",
                2026,
                DateTime.UtcNow,
                DateTime.UtcNow.AddMonths(6)
            ));
    }


    [Test]
    public void Constructor_ShouldThrow_WhenYearIsInvalid()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new LeagueSeason(
                TestDataFactory.CreateLeague(
                    TestDataFactory.CreateUser()
                ),
                "Invalid Season",
                0,
                DateTime.UtcNow,
                DateTime.UtcNow.AddMonths(6)
            ));
    }


    [Test]
    public void Constructor_ShouldThrow_WhenStartDateIsAfterEndDate()
    {
        Assert.Throws<ArgumentException>(() =>
            new LeagueSeason(
                TestDataFactory.CreateLeague(
                    TestDataFactory.CreateUser()
                ),
                "2026 Season",
                2026,
                DateTime.UtcNow.AddDays(5),
                DateTime.UtcNow
            ));
    }


    [Test]
    public void Constructor_ShouldThrow_WhenStartDateEqualsEndDate()
    {
        var date = DateTime.UtcNow;

        Assert.Throws<ArgumentException>(() =>
            new LeagueSeason(
                TestDataFactory.CreateLeague(
                    TestDataFactory.CreateUser()
                ),
                "2026 Season",
                2026,
                date,
                date
            ));
    }


    [Test]
    public void AddMatchup_ShouldAddMatchup_WhenValid()
    {
        var season = CreateSeason();

        var matchup = CreateMatchup(
            season.League
        );

        season.AddMatchup(matchup);

        Assert.Multiple(() =>
        {
            Assert.That(
                season.Matchups.Count,
                Is.EqualTo(1)
            );

            Assert.That(
                season.MatchupCount,
                Is.EqualTo(1)
            );
        });
    }


    [Test]
    public void AddMatchup_ShouldThrow_WhenMatchupIsNull()
    {
        var season = CreateSeason();

        Assert.Throws<ArgumentNullException>(() =>
            season.AddMatchup(null!)
        );
    }


    [Test]
    public void AddMatchup_ShouldThrow_WhenMatchupBelongsToDifferentLeague()
    {
        var season = CreateSeason();

        var differentLeague =
            TestDataFactory.CreateLeague(
                TestDataFactory.CreateUser()
            );

        var matchup = CreateMatchup(
            differentLeague
        );

        Assert.Throws<InvalidOperationException>(() =>
            season.AddMatchup(matchup)
        );
    }


    [Test]
    public void AddMatchup_ShouldThrow_WhenDuplicateMatchupAdded()
    {
        var season = CreateSeason();

        var matchup = CreateMatchup(
            season.League
        );

        season.AddMatchup(matchup);

        Assert.Throws<InvalidOperationException>(() =>
            season.AddMatchup(matchup)
        );
    }


    [Test]
    public void AddMatchup_ShouldThrow_WhenSeasonIsCompleted()
    {
        var season = CreateSeason();

        season.Complete();

        Assert.Throws<InvalidOperationException>(() =>
            season.AddMatchup(
                CreateMatchup(season.League)
            ));
    }


    [Test]
    public void Complete_ShouldMarkSeasonCompleted()
    {
        var season = CreateSeason();

        season.Complete();

        Assert.That(
            season.IsCompleted,
            Is.True
        );
    }


    [Test]
    public void Complete_ShouldThrow_WhenAlreadyCompleted()
    {
        var season = CreateSeason();

        season.Complete();

        Assert.Throws<InvalidOperationException>(() =>
            season.Complete()
        );
    }


    private static LeagueSeason CreateSeason()
    {
        var league = TestDataFactory.CreateLeague(
            TestDataFactory.CreateUser()
        );

        return new LeagueSeason(
            league,
            "2026 Season",
            2026,
            DateTime.UtcNow,
            DateTime.UtcNow.AddMonths(6)
        );
    }


    private static LeagueMatchup CreateMatchup(
        League league)
    {
        return new LeagueMatchup(
            league,
            TestDataFactory.CreateUser(),
            TestDataFactory.CreateUser(),
            DateTime.UtcNow,
            DateTime.UtcNow.AddDays(7)
        );
    }
}