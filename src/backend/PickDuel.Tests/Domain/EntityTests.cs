using NUnit.Framework;
using PickDuel.Domain.Entities;

namespace PickDuel.Tests.Domain;

public class EntityTests
{
    [Test]
    public void CreatingEntity_ShouldGenerateUniqueIdentifier()
    {
        // Arrange
        var user1 = CreateUser();
        var user2 = CreateUser();


        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(user1.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(user2.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(user1.Id, Is.Not.EqualTo(user2.Id));
        });
    }


    [Test]
    public void DifferentEntityTypes_ShouldHaveIndependentIdentifiers()
    {
        // Arrange
        var user = CreateUser();

        var league = new League(
            "NFL League",
            PickDuel.Domain.Enums.SportType.NFL,
            user
        );


        // Assert
        Assert.That(user.Id, Is.Not.EqualTo(league.Id));
    }


    [Test]
    public void EntityIdentifier_ShouldRemainConsistentAfterCreation()
    {
        // Arrange
        var user = CreateUser();

        var originalId = user.Id;


        // Act
        var retrievedId = user.Id;


        // Assert
        Assert.That(retrievedId, Is.EqualTo(originalId));
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
}