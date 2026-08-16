using NUnit.Framework;
using PickDuel.Domain.Entities;

namespace PickDuel.Tests.Domain;

public class UserTests
{
    [Test]
    public void NewUser_ShouldInitializeCorrectly()
    {
        // Arrange
        var user = new User(
            "Bob",
            "Bozic",
            "bob@test.com",
            "bob123"
        );

        // Assert
        Assert.That(user.Id, Is.Not.EqualTo(Guid.Empty));
        Assert.That(user.FirstName, Is.EqualTo("Bob"));
        Assert.That(user.LastName, Is.EqualTo("Bozic"));
        Assert.That(user.Email, Is.EqualTo("bob@test.com"));
        Assert.That(user.Username, Is.EqualTo("bob123"));
        Assert.That(user.CreatedAt, Is.LessThanOrEqualTo(DateTime.UtcNow));
    }


    [Test]
    public void NewUser_ShouldThrowException_WhenUsernameIsEmpty()
    {
        Assert.Throws<ArgumentException>(() =>
            new User(
                "Bob",
                "Bozic",
                "bob@test.com",
                ""
            ));
    }
}