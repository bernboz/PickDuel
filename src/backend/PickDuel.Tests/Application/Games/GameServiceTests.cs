using NSubstitute;
using NUnit.Framework;
using PickDuel.Application.Games;
using PickDuel.Application.Repositories.Interfaces;
using PickDuel.Domain.Entities;
using PickDuel.Tests.Common;

namespace PickDuel.Tests.Application.Games;

public class GameServiceTests
{
    [Test]
    public void Constructor_ShouldThrow_WhenRepositoryIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new GameService(null!));
    }


    [Test]
    public async Task CreateGameAsync_ShouldAddGameAndSaveChanges()
    {
        var repository = CreateRepository();

        var service = new GameService(repository);

        var game = TestDataFactory.CreateGame();

        var result = await service.CreateGameAsync(game);

        Assert.That(result, Is.EqualTo(game));

        await repository.Received(1).AddAsync(
            game,
            Arg.Any<CancellationToken>());

        await repository.Received(1).SaveChangesAsync(
            Arg.Any<CancellationToken>());
    }


    [Test]
    public void CreateGameAsync_ShouldThrow_WhenGameIsNull()
    {
        var service = CreateService();

        Assert.ThrowsAsync<ArgumentNullException>(
            async () => await service.CreateGameAsync(null!));
    }


    [Test]
    public async Task GetGameAsync_ShouldReturnGame_WhenFound()
    {
        var repository = CreateRepository();

        var game = TestDataFactory.CreateGame();

        repository.GetByIdAsync(
                game.Id,
                Arg.Any<CancellationToken>())
            .Returns(game);

        var service = new GameService(repository);

        var result = await service.GetGameAsync(game.Id);

        Assert.That(result, Is.EqualTo(game));

        await repository.Received(1).GetByIdAsync(
            game.Id,
            Arg.Any<CancellationToken>());
    }


    [Test]
    public async Task GetGameAsync_ShouldReturnNull_WhenGameDoesNotExist()
    {
        var repository = CreateRepository();

        repository.GetByIdAsync(
                Arg.Any<Guid>(),
                Arg.Any<CancellationToken>())
            .Returns((Game?)null);

        var service = new GameService(repository);

        var result = await service.GetGameAsync(Guid.NewGuid());

        Assert.That(result, Is.Null);
    }


    [Test]
    public void GetGameAsync_ShouldThrow_WhenIdIsEmpty()
    {
        var service = CreateService();

        Assert.ThrowsAsync<ArgumentException>(
            async () => await service.GetGameAsync(Guid.Empty));
    }


    [Test]
    public async Task GetGamesByDateRangeAsync_ShouldReturnGamesWithinRange()
    {
        var repository = CreateRepository();

        var games = new List<Game>
        {
            TestDataFactory.CreateGame()
        };

        var startDate = DateTime.UtcNow;

        var endDate = startDate.AddDays(7);

        repository.GetByDateRangeAsync(
                startDate,
                endDate,
                Arg.Any<CancellationToken>())
            .Returns(games);

        var service = new GameService(repository);

        var result = await service.GetGamesByDateRangeAsync(
            startDate,
            endDate);

        Assert.That(result, Is.EqualTo(games));

        await repository.Received(1).GetByDateRangeAsync(
            startDate,
            endDate,
            Arg.Any<CancellationToken>());
    }


    [Test]
    public void GetGamesByDateRangeAsync_ShouldThrow_WhenStartDateAfterEndDate()
    {
        var service = CreateService();

        Assert.ThrowsAsync<ArgumentException>(
            async () => await service.GetGamesByDateRangeAsync(
                DateTime.UtcNow.AddDays(5),
                DateTime.UtcNow));
    }


    [Test]
    public async Task UpdateGameAsync_ShouldUpdateGameAndSaveChanges()
    {
        var repository = CreateRepository();

        var service = new GameService(repository);

        var game = TestDataFactory.CreateGame();

        var result = await service.UpdateGameAsync(game);

        Assert.That(result, Is.EqualTo(game));

        await repository.Received(1).UpdateAsync(
            game,
            Arg.Any<CancellationToken>());

        await repository.Received(1).SaveChangesAsync(
            Arg.Any<CancellationToken>());
    }


    [Test]
    public void UpdateGameAsync_ShouldThrow_WhenGameIsNull()
    {
        var service = CreateService();

        Assert.ThrowsAsync<ArgumentNullException>(
            async () => await service.UpdateGameAsync(null!));
    }


    [Test]
    public async Task DeleteGameAsync_ShouldDeleteGameAndSaveChanges()
    {
        var repository = CreateRepository();

        var game = TestDataFactory.CreateGame();

        repository.GetByIdAsync(
                game.Id,
                Arg.Any<CancellationToken>())
            .Returns(game);

        var service = new GameService(repository);

        await service.DeleteGameAsync(game.Id);

        await repository.Received(1).DeleteAsync(
            game,
            Arg.Any<CancellationToken>());

        await repository.Received(1).SaveChangesAsync(
            Arg.Any<CancellationToken>());
    }


    [Test]
    public void DeleteGameAsync_ShouldThrow_WhenGameDoesNotExist()
    {
        var repository = CreateRepository();

        repository.GetByIdAsync(
                Arg.Any<Guid>(),
                Arg.Any<CancellationToken>())
            .Returns((Game?)null);

        var service = new GameService(repository);

        Assert.ThrowsAsync<KeyNotFoundException>(
            async () => await service.DeleteGameAsync(Guid.NewGuid()));
    }


    [Test]
    public void DeleteGameAsync_ShouldThrow_WhenIdIsEmpty()
    {
        var service = CreateService();

        Assert.ThrowsAsync<ArgumentException>(
            async () => await service.DeleteGameAsync(Guid.Empty));
    }


    private static GameService CreateService()
    {
        return new GameService(CreateRepository());
    }


    private static IGameRepository CreateRepository()
    {
        var repository = Substitute.For<IGameRepository>();

        repository.AddAsync(
                Arg.Any<Game>(),
                Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        repository.UpdateAsync(
                Arg.Any<Game>(),
                Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        repository.DeleteAsync(
                Arg.Any<Game>(),
                Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        repository.SaveChangesAsync(
                Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        return repository;
    }
}