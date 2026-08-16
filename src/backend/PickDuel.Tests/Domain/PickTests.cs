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

        var pick = new Pick(
            user,
            league,
            game,
            "Chiefs"
        );

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
            new Pick(
                user,
                league,
                game,
                "Cowboys"
            ));
    }
}