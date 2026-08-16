using NUnit.Framework;
using PickDuel.Domain.Entities;

namespace PickDuel.Tests.Domain;

public class EntityTests
{
    [Test]
    public void Entities_ShouldHaveUniqueIds()
    {
        var user1 = new User(
            "Bob",
            "Smith",
            "bob@test.com",
            "bob"
        );

        var user2 = new User(
            "Sarah",
            "Jones",
            "sarah@test.com",
            "sarah"
        );

        Assert.That(user1.Id, Is.Not.EqualTo(user2.Id));
    }


    [Test]
    public void Entity_ShouldEqualItself()
    {
        var user = new User(
            "Bob",
            "Smith",
            "bob@test.com",
            "bob"
        );

        Assert.That(user, Is.EqualTo(user));
    }
}