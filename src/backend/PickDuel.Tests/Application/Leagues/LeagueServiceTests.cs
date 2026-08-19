using NSubstitute;
using NUnit.Framework;
using PickDuel.Application.Leagues;
using PickDuel.Application.Repositories.Interfaces;
using PickDuel.Domain.Entities;
using PickDuel.Tests.Common;

namespace PickDuel.Tests.Application.Leagues;

public class LeagueServiceTests
{
    [Test]
    public void Constructor_ShouldThrow_WhenRepositoryIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new LeagueService(null!));
    }


    [Test]
    public async Task CreateLeagueAsync_ShouldAddLeagueAndSaveChanges()
    {
        var repository = CreateRepository();

        var service = new LeagueService(repository);

        var league = TestDataFactory.CreateLeague(TestDataFactory.CreateUser());

        var result = await service.CreateLeagueAsync(league);

        Assert.That(result, Is.EqualTo(league));

        await repository.Received(1).AddAsync(league, Arg.Any<CancellationToken>());

        await repository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }


    [Test]
    public void CreateLeagueAsync_ShouldThrow_WhenLeagueIsNull()
    {
        var service = CreateService();

        Assert.ThrowsAsync<ArgumentNullException>(async () => await service.CreateLeagueAsync(null!));
    }


    [Test]
    public async Task GetLeagueAsync_ShouldReturnLeague_WhenFound()
    {
        var repository = CreateRepository();

        var league = TestDataFactory.CreateLeague(TestDataFactory.CreateUser());

        repository.GetByIdAsync(league.Id, Arg.Any<CancellationToken>())
            .Returns(league);

        var service = new LeagueService(repository);

        var result = await service.GetLeagueAsync(league.Id);

        Assert.That(result, Is.EqualTo(league));

        await repository.Received(1).GetByIdAsync(league.Id, Arg.Any<CancellationToken>());
    }


    [Test]
    public async Task GetLeagueAsync_ShouldReturnNull_WhenLeagueDoesNotExist()
    {
        var repository = CreateRepository();

        repository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns((League?)null);

        var service = new LeagueService(repository);

        var result = await service.GetLeagueAsync(Guid.NewGuid());

        Assert.That(result, Is.Null);
    }


    [Test]
    public void GetLeagueAsync_ShouldThrow_WhenIdIsEmpty()
    {
        var service = CreateService();

        Assert.ThrowsAsync<ArgumentException>(async () => await service.GetLeagueAsync(Guid.Empty));
    }


    [Test]
    public async Task GetUserLeaguesAsync_ShouldReturnOwnedAndMemberLeagues()
    {
        var repository = CreateRepository();

        var user = TestDataFactory.CreateUser();

        var ownedLeague = TestDataFactory.CreateLeague(user);

        var memberLeague = TestDataFactory.CreateLeague(user);

        repository.GetByOwnerIdAsync(user.Id, Arg.Any<CancellationToken>())
            .Returns([ownedLeague]);

        repository.GetByMemberIdAsync(user.Id, Arg.Any<CancellationToken>())
            .Returns([memberLeague]);

        var service = new LeagueService(repository);

        var result = await service.GetUserLeaguesAsync(user.Id);

        Assert.That(result, Has.Count.EqualTo(2));
    }


    [Test]
    public void GetUserLeaguesAsync_ShouldThrow_WhenUserIdIsEmpty()
    {
        var service = CreateService();

        Assert.ThrowsAsync<ArgumentException>(async () => await service.GetUserLeaguesAsync(Guid.Empty));
    }


    [Test]
    public async Task AddMemberAsync_ShouldAddMemberAndSaveChanges()
    {
        var repository = CreateRepository();

        var owner = TestDataFactory.CreateUser();

        var member = TestDataFactory.CreateUser();

        var league = TestDataFactory.CreateLeague(owner);

        repository.GetByIdAsync(league.Id, Arg.Any<CancellationToken>())
            .Returns(league);

        var service = new LeagueService(repository);

        await service.AddMemberAsync(league.Id, member);

        Assert.That(league.Members, Contains.Item(member));

        await repository.Received(1).UpdateAsync(league, Arg.Any<CancellationToken>());

        await repository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }


    [Test]
    public void AddMemberAsync_ShouldThrow_WhenLeagueDoesNotExist()
    {
        var repository = CreateRepository();

        repository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns((League?)null);

        var service = new LeagueService(repository);

        Assert.ThrowsAsync<KeyNotFoundException>(async () => await service.AddMemberAsync(Guid.NewGuid(), TestDataFactory.CreateUser()));
    }


    [Test]
    public void AddMemberAsync_ShouldThrow_WhenUserIsNull()
    {
        var service = CreateService();

        Assert.ThrowsAsync<ArgumentNullException>(async () => await service.AddMemberAsync(Guid.NewGuid(), null!));
    }
        [Test]
    public async Task UpdateLeagueAsync_ShouldUpdateLeagueAndSaveChanges()
    {
        var repository = CreateRepository();

        var service = new LeagueService(repository);

        var league = TestDataFactory.CreateLeague(TestDataFactory.CreateUser());

        var result = await service.UpdateLeagueAsync(league);

        Assert.That(result, Is.EqualTo(league));

        await repository.Received(1).UpdateAsync(league, Arg.Any<CancellationToken>());

        await repository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }


    [Test]
    public void UpdateLeagueAsync_ShouldThrow_WhenLeagueIsNull()
    {
        var service = CreateService();

        Assert.ThrowsAsync<ArgumentNullException>(async () => await service.UpdateLeagueAsync(null!));
    }


    [Test]
    public async Task DeleteLeagueAsync_ShouldDeleteLeagueAndSaveChanges()
    {
        var repository = CreateRepository();

        var league = TestDataFactory.CreateLeague(TestDataFactory.CreateUser());

        repository.GetByIdAsync(league.Id, Arg.Any<CancellationToken>())
            .Returns(league);

        var service = new LeagueService(repository);

        await service.DeleteLeagueAsync(league.Id);

        await repository.Received(1).GetByIdAsync(league.Id, Arg.Any<CancellationToken>());

        await repository.Received(1).DeleteAsync(league, Arg.Any<CancellationToken>());

        await repository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }


    [Test]
    public void DeleteLeagueAsync_ShouldThrow_WhenLeagueDoesNotExist()
    {
        var repository = CreateRepository();

        repository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns((League?)null);

        var service = new LeagueService(repository);

        Assert.ThrowsAsync<KeyNotFoundException>(async () => await service.DeleteLeagueAsync(Guid.NewGuid()));
    }


    [Test]
    public void DeleteLeagueAsync_ShouldThrow_WhenIdIsEmpty()
    {
        var service = CreateService();

        Assert.ThrowsAsync<ArgumentException>(async () => await service.DeleteLeagueAsync(Guid.Empty));
    }


    private static LeagueService CreateService()
    {
        return new LeagueService(CreateRepository());
    }


    private static ILeagueRepository CreateRepository()
    {
        var repository = Substitute.For<ILeagueRepository>();

        repository.AddAsync(Arg.Any<League>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        repository.UpdateAsync(Arg.Any<League>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        repository.DeleteAsync(Arg.Any<League>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        repository.SaveChangesAsync(Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        return repository;
    }
}