using NUnit.Framework;
using PickDuel.Domain.Entities;
using PickDuel.Domain.Enums;
using PickDuel.Tests.Common;

namespace PickDuel.Tests.Domain;

public class LeagueTests
{
    [Test]
    public void NewLeague_ShouldInitializeCorrectly()
    {
        var owner = TestDataFactory.CreateUser();

        var league = new League(
            "Test League",
            SportType.NFL,
            owner
        );

        Assert.Multiple(() =>
        {
            Assert.That(league.Name, Is.EqualTo("Test League"));
            Assert.That(league.Sport, Is.EqualTo(SportType.NFL));
            Assert.That(league.Owner, Is.EqualTo(owner));
            Assert.That(league.Members.Count, Is.EqualTo(1));
            Assert.That(league.Members.First(), Is.EqualTo(owner));
            Assert.That(league.CreatedAt, Is.LessThanOrEqualTo(DateTime.UtcNow));
        });
    }


    [Test]
    public void NewLeague_ShouldThrow_WhenOwnerIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new League(
                "Test League",
                SportType.NFL,
                null!
            ));
    }


    [Test]
    public void AddMember_ShouldIncreaseMemberCount()
    {
        var owner = TestDataFactory.CreateUser();
        var member = TestDataFactory.CreateUser();

        var league = CreateLeague(owner);

        league.AddMember(member);

        Assert.That(
            league.Members.Count,
            Is.EqualTo(2)
        );
    }


    [Test]
    public void AddMember_ShouldAddCorrectUser()
    {
        var owner = TestDataFactory.CreateUser();
        var member = TestDataFactory.CreateUser();

        var league = CreateLeague(owner);

        league.AddMember(member);

        Assert.That(
            league.Members,
            Does.Contain(member)
        );
    }


    [Test]
    public void AddMember_ShouldThrow_WhenUserAlreadyExists()
    {
        var owner = TestDataFactory.CreateUser();

        var league = CreateLeague(owner);

        Assert.Throws<InvalidOperationException>(() =>
            league.AddMember(owner)
        );
    }


    [Test]
    public void AddMember_ShouldThrow_WhenMaximumMembersReached()
    {
        var owner = TestDataFactory.CreateUser();

        var league = CreateLeague(owner);

        for (var i = 0; i < 31; i++)
        {
            league.AddMember(
                TestDataFactory.CreateUser()
            );
        }

        Assert.That(
            league.Members.Count,
            Is.EqualTo(32)
        );

        Assert.Throws<InvalidOperationException>(() =>
            league.AddMember(
                TestDataFactory.CreateUser()
            ));
    }


    [Test]
    public void NewLeague_ShouldAddOwnerAsFirstMember()
    {
        var owner = TestDataFactory.CreateUser();

        var league = CreateLeague(owner);

        Assert.Multiple(() =>
        {
            Assert.That(league.Members.Count, Is.EqualTo(1));
            Assert.That(league.Members.First(), Is.EqualTo(owner));
        });
    }


    [Test]
    public void NewLeague_ShouldStoreCorrectSport()
    {
        var league = CreateLeague(
            TestDataFactory.CreateUser()
        );

        Assert.That(
            league.Sport,
            Is.EqualTo(SportType.NFL)
        );
    }


    [Test]
    public void NewLeague_ShouldStoreOwner()
    {
        var owner = TestDataFactory.CreateUser();

        var league = CreateLeague(owner);

        Assert.That(
            league.Owner,
            Is.EqualTo(owner)
        );
    }


    private static League CreateLeague(User owner)
    {
        return new League(
            "Test League",
            SportType.NFL,
            owner
        );
    }
}