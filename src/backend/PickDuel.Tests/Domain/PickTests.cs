using NUnit.Framework;
using PickDuel.Domain.Entities;
using PickDuel.Domain.Enums;

namespace PickDuel.Tests.Domain;

public class PickTests
{
    [Test]
    public void NewPick_ShouldInitializeCorrectly()
    {
        var user = new User(
            "Bob",
            "Smith",
            "bob@test.com",
            "bob"
        );

        var league = new League(
            "NFL League",
            SportType.NFL,
            user
        );

        var game = new Game(
            "Chiefs",
            "Bills",
            DateTime.UtcNow.AddDays(1),
            DateTime.UtcNow.AddDays(1).AddHours(3)
        );

        var pick = new Pick(user, league, game, "Chiefs", 3);

        Assert.That(pick.Id, Is.Not.EqualTo(Guid.Empty));
        Assert.That(pick.User, Is.EqualTo(user));
        Assert.That(pick.League, Is.EqualTo(league));
        Assert.That(pick.Game, Is.EqualTo(game));
        Assert.That(pick.SelectedTeam, Is.EqualTo("Chiefs"));
    }


    [Test]
    public void NewPick_ShouldThrowException_WhenSelectingInvalidTeam()
    {
        var user = new User(
            "Bob",
            "Smith",
            "bob@test.com",
            "bob"
        );

        var league = new League(
            "NFL League",
            SportType.NFL,
            user
        );

        var game = new Game(
            "Chiefs",
            "Bills",
            DateTime.UtcNow.AddDays(1),
            DateTime.UtcNow.AddDays(1).AddHours(3)
        );

        Assert.Throws<ArgumentException>(() =>
            new Pick(user, league, game, "Cowboys", 3));
    }
    
    [Test]
    public void NewPick_ShouldStoreConfidenceMultiplier()
    {
        var user = CreateUser();
        var league = CreateLeague(user);
        var game = CreateGame();

        var pick = new Pick(
            user,
            league,
            game,
            game.HomeTeam,
            5
        );

        Assert.That(
            pick.ConfidenceMultiplier,
            Is.EqualTo(5)
        );
    }


    [Test]
    public void NewPick_ShouldThrow_WhenConfidenceIsBelowMinimum()
    {
        var user = CreateUser();
        var league = CreateLeague(user);
        var game = CreateGame();

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new Pick(
                user,
                league,
                game,
                game.HomeTeam,
                0
            ));
    }


    [Test]
    public void NewPick_ShouldThrow_WhenConfidenceIsAboveMaximum()
    {
        var user = CreateUser();
        var league = CreateLeague(user);
        var game = CreateGame();

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new Pick(
                user,
                league,
                game,
                game.HomeTeam,
                6
            ));
    }


    [Test]
    public void NewPick_ShouldAllowMinimumConfidence()
    {
        var user = CreateUser();
        var league = CreateLeague(user);
        var game = CreateGame();

        var pick = new Pick(
            user,
            league,
            game,
            game.HomeTeam,
            1
        );

        Assert.That(
            pick.ConfidenceMultiplier,
            Is.EqualTo(1)
        );
    }


    [Test]
    public void NewPick_ShouldAllowMaximumConfidence()
    {
        var user = CreateUser();
        var league = CreateLeague(user);
        var game = CreateGame();

        var pick = new Pick(
            user,
            league,
            game,
            game.HomeTeam,
            5
        );

        Assert.That(
            pick.ConfidenceMultiplier,
            Is.EqualTo(5)
        );
    }
    
    [Test]
    public void ChangeConfidence_ShouldUpdateConfidence_WhenGameHasNotStarted()
    {
        var pick = CreateFuturePick();

        pick.ChangeConfidence(5);

        Assert.That(
            pick.ConfidenceMultiplier,
            Is.EqualTo(5)
        );
    }
    
    [Test]
    public void ChangeConfidence_ShouldThrow_WhenGameHasStarted()
    {
        var pick = CreateStartedPick();

        Assert.Throws<InvalidOperationException>(() =>
            pick.ChangeConfidence(5)
        );
    }
    
    [Test]
    public void ChangeSelection_ShouldUpdateTeam_WhenGameHasNotStarted()
    {
        var pick = CreateFuturePick();

        pick.ChangeSelection("Bills");

        Assert.That(
            pick.SelectedTeam,
            Is.EqualTo("Bills")
        );
    }
    
    [Test]
    public void ChangeSelection_ShouldThrow_WhenGameHasStarted()
    {
        var pick = CreateStartedPick();

        Assert.Throws<InvalidOperationException>(() =>
            pick.ChangeSelection("Bills")
        );
    }
    
    [Test]
    public void ChangeSelection_ShouldThrow_WhenTeamIsNotInGame()
    {
        var pick = CreateFuturePick();

        Assert.Throws<ArgumentException>(() =>
            pick.ChangeSelection("Cowboys")
        );
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
    
    private static Pick CreateFuturePick()
    {
        var user = CreateUser();
        var league = CreateLeague(user);
        var game = CreateFutureGame();

        return new Pick(
            user,
            league,
            game,
            game.HomeTeam,
            3
        );
    }


    private static Pick CreateStartedPick()
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
    
    private static Game CreateFutureGame()
    {
        return new Game(
            "Chiefs",
            "Bills",
            DateTime.UtcNow.AddHours(1),
            DateTime.UtcNow.AddHours(4)
        );
    }

    private static Game CreateStartedGame()
    {
        return new Game(
            "Chiefs",
            "Bills",
            DateTime.UtcNow.AddHours(-4),
            DateTime.UtcNow.AddHours(-1)
        );
    }
}