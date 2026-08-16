using NUnit.Framework;
using PickDuel.Domain.Entities;

namespace PickDuel.Tests.Domain;

public class GameTests
{
    [Test]
    public void NewGame_ShouldInitializeCorrectly()
    {
        var startTime = DateTime.UtcNow.AddDays(1);
        var endTime = DateTime.UtcNow.AddDays(2);

        var game = new Game(
            "Clemson",
            "Florida State",
            startTime,
            endTime
        );

        Assert.That(game.Id, Is.Not.EqualTo(Guid.Empty));
        Assert.That(game.HomeTeam, Is.EqualTo("Clemson"));
        Assert.That(game.AwayTeam, Is.EqualTo("Florida State"));
        Assert.That(game.StartTime, Is.EqualTo(startTime));
    }


    [Test]
    public void NewGame_ShouldThrowException_WhenTeamIsMissing()
    {
        Assert.Throws<ArgumentException>(() =>
            new Game(
                "",
                "Florida State",
                DateTime.UtcNow,
                DateTime.UtcNow
            ));
    }
}