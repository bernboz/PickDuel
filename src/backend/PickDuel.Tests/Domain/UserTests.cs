using NUnit.Framework;
using PickDuel.Domain.Entities;
using PickDuel.Tests.Common;

namespace PickDuel.Tests.Domain;

public class UserTests
{
    [Test]
    public void NewUser_ShouldInitializeCorrectly()
    {
        var user = TestDataFactory.CreateUser();

        Assert.Multiple(() =>
        {
            Assert.That(user.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(user.FirstName, Is.EqualTo("Bob"));
            Assert.That(user.LastName, Is.EqualTo("Smith"));
            Assert.That(user.Email, Does.Contain("@"));
            Assert.That(user.Username, Is.Not.Empty);
            Assert.That(user.CreatedAt, Is.LessThanOrEqualTo(DateTime.UtcNow));
        });
    }


    [Test]
    public void NewUser_ShouldGenerateUniqueIdentifier()
    {
        var firstUser = TestDataFactory.CreateUser();
        var secondUser = TestDataFactory.CreateUser();

        Assert.That(
            firstUser.Id,
            Is.Not.EqualTo(secondUser.Id)
        );
    }


    [Test]
    public void NewUser_ShouldStoreProvidedValues()
    {
        var user = new User(
            "John",
            "Doe",
            "john@test.com",
            "johndoe"
        );

        Assert.Multiple(() =>
        {
            Assert.That(user.FirstName, Is.EqualTo("John"));
            Assert.That(user.LastName, Is.EqualTo("Doe"));
            Assert.That(user.Email, Is.EqualTo("john@test.com"));
            Assert.That(user.Username, Is.EqualTo("johndoe"));
        });
    }


    [Test]
    public void NewUser_ShouldThrow_WhenUsernameIsEmpty()
    {
        Assert.Throws<ArgumentException>(() =>
            new User(
                "Bob",
                "Smith",
                "bob@test.com",
                string.Empty
            ));
    }


    [Test]
    public void NewUser_ShouldThrow_WhenUsernameIsWhitespace()
    {
        Assert.Throws<ArgumentException>(() =>
            new User(
                "Bob",
                "Smith",
                "bob@test.com",
                "   "
            ));
    }


    [Test]
    public void NewUser_ShouldThrow_WhenEmailIsEmpty()
    {
        Assert.Throws<ArgumentException>(() =>
            new User(
                "Bob",
                "Smith",
                string.Empty,
                "bob123"
            ));
    }


    [Test]
    public void NewUser_ShouldThrow_WhenFirstNameIsEmpty()
    {
        Assert.Throws<ArgumentException>(() =>
            new User(
                string.Empty,
                "Smith",
                "bob@test.com",
                "bob123"
            ));
    }


    [Test]
    public void NewUser_ShouldThrow_WhenLastNameIsEmpty()
    {
        Assert.Throws<ArgumentException>(() =>
            new User(
                "Bob",
                string.Empty,
                "bob@test.com",
                "bob123"
            ));
    }
}