using NSubstitute;
using NUnit.Framework;
using PickDuel.Application.Picks;
using PickDuel.Domain.Entities;
using PickDuel.Infrastructure.Repositories.Interfaces;
using PickDuel.Tests.Common;

namespace PickDuel.Tests.Application.Picks;

public class PickServiceTests
{
    [Test]
    public void Constructor_ShouldThrow_WhenRepositoryIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new PickService(null!)
        );
    }


    [Test]
    public async Task CreatePickAsync_ShouldAddPickAndSaveChanges()
    {
        var repository = CreateRepository();

        var service = new PickService(repository);

        var pick = TestDataFactory.CreateFuturePick();

        var result = await service.CreatePickAsync(pick);

        Assert.That(
            result,
            Is.EqualTo(pick)
        );

        await repository.Received(1)
            .AddAsync(pick);

        await repository.Received(1)
            .SaveChangesAsync();
    }


    [Test]
    public void CreatePickAsync_ShouldThrow_WhenPickIsNull()
    {
        var service = CreateService();

        Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await service.CreatePickAsync(null!)
        );
    }


    [Test]
    public async Task GetUserPickForGameAsync_ShouldReturnPick_WhenFound()
    {
        var repository = CreateRepository();

        var pick = TestDataFactory.CreateFuturePick();

        repository.GetUserPickForGameAsync(
                pick.User.Id,
                pick.Game.Id)
            .Returns(pick);

        var service = new PickService(repository);

        var result =
            await service.GetUserPickForGameAsync(
                pick.User.Id,
                pick.Game.Id
            );

        Assert.That(
            result,
            Is.EqualTo(pick)
        );

        await repository.Received(1)
            .GetUserPickForGameAsync(
                pick.User.Id,
                pick.Game.Id
            );
    }


    [Test]
    public async Task GetUserPickForGameAsync_ShouldReturnNull_WhenNotFound()
    {
        var repository = CreateRepository();

        repository.GetUserPickForGameAsync(
                Arg.Any<Guid>(),
                Arg.Any<Guid>())
            .Returns((Pick?)null);

        var service = new PickService(repository);

        var result =
            await service.GetUserPickForGameAsync(
                Guid.NewGuid(),
                Guid.NewGuid()
            );

        Assert.That(
            result,
            Is.Null
        );
    }


    [Test]
    public void GetUserPickForGameAsync_ShouldThrow_WhenUserIdIsEmpty()
    {
        var service = CreateService();

        Assert.ThrowsAsync<ArgumentException>(async () =>
            await service.GetUserPickForGameAsync(
                Guid.Empty,
                Guid.NewGuid()
            )
        );
    }


    [Test]
    public async Task GetUserPicksAsync_ShouldReturnUsersPicks()
    {
        var repository = CreateRepository();

        var picks =
            new List<Pick>
            {
                TestDataFactory.CreateFuturePick()
            };

        repository.GetByUserIdAsync(
                Arg.Any<Guid>())
            .Returns(picks);

        var service = new PickService(repository);

        var result =
            await service.GetUserPicksAsync(
                picks[0].User.Id
            );

        Assert.That(
            result,
            Is.EqualTo(picks)
        );

        await repository.Received(1)
            .GetByUserIdAsync(
                picks[0].User.Id
            );
    }


    [Test]
    public async Task UpdatePickAsync_ShouldUpdatePickAndSaveChanges()
    {
        var repository = CreateRepository();

        var service = new PickService(repository);

        var pick = TestDataFactory.CreateFuturePick();

        var result =
            await service.UpdatePickAsync(pick);

        Assert.That(
            result,
            Is.EqualTo(pick)
        );

        await repository.Received(1)
            .UpdateAsync(pick);

        await repository.Received(1)
            .SaveChangesAsync();
    }


    [Test]
    public void UpdatePickAsync_ShouldThrow_WhenPickIsNull()
    {
        var service = CreateService();

        Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await service.UpdatePickAsync(null!)
        );
    }


    [Test]
    public async Task DeletePickAsync_ShouldDeleteExistingPick()
    {
        var repository = CreateRepository();

        var pick =
            TestDataFactory.CreateFuturePick();

        repository.GetByIdAsync(
                pick.Id)
            .Returns(pick);

        var service =
            new PickService(repository);

        await service.DeletePickAsync(
            pick.Id
        );

        await repository.Received(1)
            .GetByIdAsync(
                pick.Id
            );

        await repository.Received(1)
            .DeleteAsync(
                pick
            );

        await repository.Received(1)
            .SaveChangesAsync();
    }


    [Test]
    public void DeletePickAsync_ShouldThrow_WhenPickDoesNotExist()
    {
        var repository = CreateRepository();

        repository.GetByIdAsync(
                Arg.Any<Guid>())
            .Returns((Pick?)null);

        var service =
            new PickService(repository);

        Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            await service.DeletePickAsync(
                Guid.NewGuid()
            )
        );
    }


    [Test]
    public void DeletePickAsync_ShouldThrow_WhenIdIsEmpty()
    {
        var service = CreateService();

        Assert.ThrowsAsync<ArgumentException>(async () =>
            await service.DeletePickAsync(
                Guid.Empty
            )
        );
    }


    private static PickService CreateService()
    {
        return new PickService(
            CreateRepository()
        );
    }


    private static IPickRepository CreateRepository()
    {
        var repository =
            Substitute.For<IPickRepository>();

        repository.SaveChangesAsync()
            .Returns(Task.CompletedTask);

        repository.AddAsync(
                Arg.Any<Pick>())
            .Returns(Task.CompletedTask);

        repository.UpdateAsync(
                Arg.Any<Pick>())
            .Returns(Task.CompletedTask);

        repository.DeleteAsync(
                Arg.Any<Pick>())
            .Returns(Task.CompletedTask);

        return repository;
    }
}