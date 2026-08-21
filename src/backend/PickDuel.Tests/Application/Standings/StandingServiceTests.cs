using NSubstitute;
using NUnit.Framework;
using PickDuel.Application.Repositories.Interfaces;
using PickDuel.Application.Standings;
using PickDuel.Domain.Entities.Standings;
using PickDuel.Tests.Common;

namespace PickDuel.Tests.Application.Standings;

public class StandingServiceTests
{
    [Test]
    public void Constructor_ShouldThrow_WhenRepositoryIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new StandingService(null!));
    }


    [Test]
    public async Task CreateStandingAsync_ShouldAddStandingAndSaveChanges()
    {
        var repository = CreateRepository();

        var service = new StandingService(repository);

        var standing = TestDataFactory.CreateLeagueStanding();

        var result = await service.CreateStandingAsync(standing);

        Assert.That(result, Is.EqualTo(standing));

        await repository.Received(1).AddAsync(standing, Arg.Any<CancellationToken>());

        await repository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }


    [Test]
    public void CreateStandingAsync_ShouldThrow_WhenStandingIsNull()
    {
        var service = CreateService();

        Assert.ThrowsAsync<ArgumentNullException>(async () => await service.CreateStandingAsync(null!));
    }


    [Test]
    public async Task GetStandingAsync_ShouldReturnStanding_WhenFound()
    {
        var repository = CreateRepository();

        var standing = TestDataFactory.CreateLeagueStanding();

        repository.GetByIdAsync(standing.Id, Arg.Any<CancellationToken>())
            .Returns(standing);

        var service = new StandingService(repository);

        var result = await service.GetStandingAsync(standing.Id);

        Assert.That(result, Is.EqualTo(standing));

        await repository.Received(1).GetByIdAsync(standing.Id, Arg.Any<CancellationToken>());
    }


    [Test]
    public async Task GetStandingAsync_ShouldReturnNull_WhenStandingDoesNotExist()
    {
        var repository = CreateRepository();

        repository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns((LeagueStanding?)null);

        var service = new StandingService(repository);

        var result = await service.GetStandingAsync(Guid.NewGuid());

        Assert.That(result, Is.Null);
    }


    [Test]
    public void GetStandingAsync_ShouldThrow_WhenIdIsEmpty()
    {
        var service = CreateService();

        Assert.ThrowsAsync<ArgumentException>(async () => await service.GetStandingAsync(Guid.Empty));
    }


    [Test]
    public async Task GetLeagueStandingsAsync_ShouldReturnLeagueStandings()
    {
        var repository = CreateRepository();

        var standings = new List<LeagueStanding>
        {
            TestDataFactory.CreateLeagueStanding()
        };

        repository.GetByLeagueIdAsync(standings[0].League.Id, Arg.Any<CancellationToken>())
            .Returns(standings);

        var service = new StandingService(repository);

        var result = await service.GetLeagueStandingsAsync(standings[0].League.Id);

        Assert.That(result, Is.EqualTo(standings));

        await repository.Received(1).GetByLeagueIdAsync(standings[0].League.Id, Arg.Any<CancellationToken>());
    }


    [Test]
    public void GetLeagueStandingsAsync_ShouldThrow_WhenLeagueIdIsEmpty()
    {
        var service = CreateService();

        Assert.ThrowsAsync<ArgumentException>(async () => await service.GetLeagueStandingsAsync(Guid.Empty));
    }


    [Test]
    public async Task GetUserStandingAsync_ShouldReturnStanding_WhenFound()
    {
        var repository = CreateRepository();

        var standing = TestDataFactory.CreateLeagueStanding();

        repository.GetByLeagueAndUserAsync(standing.League.Id, standing.User.Id, Arg.Any<CancellationToken>())
            .Returns(standing);

        var service = new StandingService(repository);

        var result = await service.GetUserStandingAsync(standing.League.Id, standing.User.Id);

        Assert.That(result, Is.EqualTo(standing));

        await repository.Received(1).GetByLeagueAndUserAsync(standing.League.Id, standing.User.Id, Arg.Any<CancellationToken>());
    }


    [Test]
    public void GetUserStandingAsync_ShouldThrow_WhenLeagueIdIsEmpty()
    {
        var service = CreateService();

        Assert.ThrowsAsync<ArgumentException>(async () => await service.GetUserStandingAsync(Guid.Empty, Guid.NewGuid()));
    }


    [Test]
    public void GetUserStandingAsync_ShouldThrow_WhenUserIdIsEmpty()
    {
        var service = CreateService();

        Assert.ThrowsAsync<ArgumentException>(async () => await service.GetUserStandingAsync(Guid.NewGuid(), Guid.Empty));
    }


    [Test]
    public async Task UpdateStandingAsync_ShouldUpdateStandingAndSaveChanges()
    {
        var repository = CreateRepository();

        var service = new StandingService(repository);

        var standing = TestDataFactory.CreateLeagueStanding();

        var result = await service.UpdateStandingAsync(standing);

        Assert.That(result, Is.EqualTo(standing));

        await repository.Received(1).UpdateAsync(standing, Arg.Any<CancellationToken>());

        await repository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }


    [Test]
    public void UpdateStandingAsync_ShouldThrow_WhenStandingIsNull()
    {
        var service = CreateService();

        Assert.ThrowsAsync<ArgumentNullException>(async () => await service.UpdateStandingAsync(null!));
    }


    [Test]
    public async Task DeleteStandingAsync_ShouldDeleteStandingAndSaveChanges()
    {
        var repository = CreateRepository();

        var standing = TestDataFactory.CreateLeagueStanding();

        repository.GetByIdAsync(standing.Id, Arg.Any<CancellationToken>())
            .Returns(standing);

        var service = new StandingService(repository);

        await service.DeleteStandingAsync(standing.Id);

        await repository.Received(1).GetByIdAsync(standing.Id, Arg.Any<CancellationToken>());

        await repository.Received(1).DeleteAsync(standing, Arg.Any<CancellationToken>());

        await repository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }


    [Test]
    public void DeleteStandingAsync_ShouldThrow_WhenStandingDoesNotExist()
    {
        var repository = CreateRepository();

        repository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns((LeagueStanding?)null);

        var service = new StandingService(repository);

        Assert.ThrowsAsync<KeyNotFoundException>(async () => await service.DeleteStandingAsync(Guid.NewGuid()));
    }


    [Test]
    public void DeleteStandingAsync_ShouldThrow_WhenIdIsEmpty()
    {
        var service = CreateService();

        Assert.ThrowsAsync<ArgumentException>(async () => await service.DeleteStandingAsync(Guid.Empty));
    }


    private static StandingService CreateService()
    {
        return new StandingService(CreateRepository());
    }


    private static IStandingRepository CreateRepository()
    {
        var repository = Substitute.For<IStandingRepository>();

        repository.AddAsync(Arg.Any<LeagueStanding>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        repository.UpdateAsync(Arg.Any<LeagueStanding>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        repository.DeleteAsync(Arg.Any<LeagueStanding>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        repository.SaveChangesAsync(Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        return repository;
    }
}