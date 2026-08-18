using NUnit.Framework;
using PickDuel.Domain.Entities;
using PickDuel.Tests.Common;

namespace PickDuel.Tests.Domain;

public class EntityTests
{
    [Test]
    public void NewEntity_ShouldGenerateIdentifier()
    {
        var user = TestDataFactory.CreateUser();

        Assert.That(
            user.Id,
            Is.Not.EqualTo(Guid.Empty)
        );
    }


    [Test]
    public void NewEntities_ShouldGenerateUniqueIdentifiers()
    {
        var userOne = TestDataFactory.CreateUser();
        var userTwo = TestDataFactory.CreateUser();

        Assert.That(
            userOne.Id,
            Is.Not.EqualTo(userTwo.Id)
        );
    }


    [Test]
    public void DifferentEntityTypes_ShouldGenerateIndependentIdentifiers()
    {
        var user = TestDataFactory.CreateUser();
        var league = TestDataFactory.CreateLeague(user);

        Assert.Multiple(() =>
        {
            Assert.That(user.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(league.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(user.Id, Is.Not.EqualTo(league.Id));
        });
    }


    [Test]
    public void CreatingManyEntities_ShouldGenerateUniqueIdentifiers()
    {
        var users = Enumerable.Range(0, 100)
            .Select(_ => TestDataFactory.CreateUser())
            .ToList();

        var uniqueIds = users
            .Select(user => user.Id)
            .Distinct()
            .Count();

        Assert.That(
            uniqueIds,
            Is.EqualTo(users.Count)
        );
    }


    [Test]
    public void EntityIdentifier_ShouldRemainConsistentAfterCreation()
    {
        var user = TestDataFactory.CreateUser();

        var initialId = user.Id;

        Assert.That(
            user.Id,
            Is.EqualTo(initialId)
        );
    }
}