using NSubstitute;
using NUnit.Framework;
using PickDuel.Application.Repositories.Interfaces;
using PickDuel.Application.Users;
using PickDuel.Domain.Entities;
using PickDuel.Tests.Common;

namespace PickDuel.Tests.Application.Users;

public class UserServiceTests
{
    [Test]
    public void Constructor_ShouldThrow_WhenRepositoryIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new UserService(null!));
    }


    [Test]
    public async Task CreateUserAsync_ShouldAddUserAndSaveChanges()
    {
        var repository = CreateRepository();

        var service = new UserService(repository);

        var user = TestDataFactory.CreateUser();

        var result = await service.CreateUserAsync(user);

        Assert.That(result, Is.EqualTo(user));

        await repository.Received(1).AddAsync(user, Arg.Any<CancellationToken>());

        await repository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }


    [Test]
    public void CreateUserAsync_ShouldThrow_WhenUserIsNull()
    {
        var service = CreateService();

        Assert.ThrowsAsync<ArgumentNullException>(async () => await service.CreateUserAsync(null!));
    }


    [Test]
    public async Task GetUserAsync_ShouldReturnUser_WhenFound()
    {
        var repository = CreateRepository();

        var user = TestDataFactory.CreateUser();

        repository.GetByIdAsync(user.Id, Arg.Any<CancellationToken>()).Returns(user);

        var service = new UserService(repository);

        var result = await service.GetUserAsync(user.Id);

        Assert.That(result, Is.EqualTo(user));

        await repository.Received(1).GetByIdAsync(user.Id, Arg.Any<CancellationToken>());
    }


    [Test]
    public async Task GetUserAsync_ShouldReturnNull_WhenUserDoesNotExist()
    {
        var repository = CreateRepository();

        repository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns((User?)null);

        var service = new UserService(repository);

        var result = await service.GetUserAsync(Guid.NewGuid());

        Assert.That(result, Is.Null);
    }


    [Test]
    public void GetUserAsync_ShouldThrow_WhenIdIsEmpty()
    {
        var service = CreateService();

        Assert.ThrowsAsync<ArgumentException>(async () => await service.GetUserAsync(Guid.Empty));
    }


    [Test]
    public async Task GetUserByEmailAsync_ShouldReturnUser_WhenFound()
    {
        var repository = CreateRepository();

        var user = TestDataFactory.CreateUser();

        repository.GetByEmailAsync(user.Email, Arg.Any<CancellationToken>()).Returns(user);

        var service = new UserService(repository);

        var result = await service.GetUserByEmailAsync(user.Email);

        Assert.That(result, Is.EqualTo(user));

        await repository.Received(1).GetByEmailAsync(user.Email, Arg.Any<CancellationToken>());
    }


    [Test]
    public async Task GetUserByEmailAsync_ShouldReturnNull_WhenUserDoesNotExist()
    {
        var repository = CreateRepository();

        repository.GetByEmailAsync(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns((User?)null);

        var service = new UserService(repository);

        var result = await service.GetUserByEmailAsync("missing@example.com");

        Assert.That(result, Is.Null);
    }


    [Test]
    public void GetUserByEmailAsync_ShouldThrow_WhenEmailIsEmpty()
    {
        var service = CreateService();

        Assert.ThrowsAsync<ArgumentException>(async () => await service.GetUserByEmailAsync(string.Empty));
    }


    [Test]
    public async Task GetUserByUsernameAsync_ShouldReturnUser_WhenFound()
    {
        var repository = CreateRepository();

        var user = TestDataFactory.CreateUser();

        repository.GetByUsernameAsync(user.Username, Arg.Any<CancellationToken>()).Returns(user);

        var service = new UserService(repository);

        var result = await service.GetUserByUsernameAsync(user.Username);

        Assert.That(result, Is.EqualTo(user));

        await repository.Received(1).GetByUsernameAsync(user.Username, Arg.Any<CancellationToken>());
    }


    [Test]
    public async Task GetUserByUsernameAsync_ShouldReturnNull_WhenUserDoesNotExist()
    {
        var repository = CreateRepository();

        repository.GetByUsernameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns((User?)null);

        var service = new UserService(repository);

        var result = await service.GetUserByUsernameAsync("unknown");

        Assert.That(result, Is.Null);
    }


    [Test]
    public void GetUserByUsernameAsync_ShouldThrow_WhenUsernameIsEmpty()
    {
        var service = CreateService();

        Assert.ThrowsAsync<ArgumentException>(async () => await service.GetUserByUsernameAsync(string.Empty));
    }
    
        [Test]
    public async Task UpdateUserAsync_ShouldUpdateUserAndSaveChanges()
    {
        var repository = CreateRepository();

        var service = new UserService(repository);

        var user = TestDataFactory.CreateUser();

        var result = await service.UpdateUserAsync(user);

        Assert.That(result, Is.EqualTo(user));

        await repository.Received(1).UpdateAsync(user, Arg.Any<CancellationToken>());

        await repository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }


    [Test]
    public void UpdateUserAsync_ShouldThrow_WhenUserIsNull()
    {
        var service = CreateService();

        Assert.ThrowsAsync<ArgumentNullException>(async () => await service.UpdateUserAsync(null!));
    }


    [Test]
    public async Task DeleteUserAsync_ShouldDeleteUserAndSaveChanges()
    {
        var repository = CreateRepository();

        var user = TestDataFactory.CreateUser();

        repository.GetByIdAsync(user.Id, Arg.Any<CancellationToken>()).Returns(user);

        var service = new UserService(repository);

        await service.DeleteUserAsync(user.Id);

        await repository.Received(1).GetByIdAsync(user.Id, Arg.Any<CancellationToken>());

        await repository.Received(1).DeleteAsync(user, Arg.Any<CancellationToken>());

        await repository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }


    [Test]
    public void DeleteUserAsync_ShouldThrow_WhenUserDoesNotExist()
    {
        var repository = CreateRepository();

        repository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns((User?)null);

        var service = new UserService(repository);

        Assert.ThrowsAsync<KeyNotFoundException>(async () => await service.DeleteUserAsync(Guid.NewGuid()));
    }


    [Test]
    public void DeleteUserAsync_ShouldThrow_WhenIdIsEmpty()
    {
        var service = CreateService();

        Assert.ThrowsAsync<ArgumentException>(async () => await service.DeleteUserAsync(Guid.Empty));
    }


    private static UserService CreateService()
    {
        return new UserService(CreateRepository());
    }


    private static IUserRepository CreateRepository()
    {
        var repository = Substitute.For<IUserRepository>();

        repository.AddAsync(Arg.Any<User>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        repository.UpdateAsync(Arg.Any<User>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        repository.DeleteAsync(Arg.Any<User>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        repository.SaveChangesAsync(Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        return repository;
    }
}