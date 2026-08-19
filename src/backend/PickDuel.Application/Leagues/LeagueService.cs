using PickDuel.Application.Repositories.Interfaces;
using PickDuel.Domain.Entities;

namespace PickDuel.Application.Leagues;

/// <summary>
/// Provides application workflows for creating,
/// retrieving, updating, and deleting leagues.
/// </summary>
public class LeagueService : ILeagueService
{
    private readonly ILeagueRepository _leagueRepository;
    
    public LeagueService(ILeagueRepository leagueRepository)
    {
        ArgumentNullException.ThrowIfNull(leagueRepository);

        _leagueRepository = leagueRepository;
    }

    public async Task<League> CreateLeagueAsync(League league, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(league);

        await _leagueRepository.AddAsync(league, cancellationToken);

        await _leagueRepository.SaveChangesAsync(cancellationToken);

        return league;
    }
    
    public async Task<League?> GetLeagueAsync(Guid leagueId, CancellationToken cancellationToken = default)
    {
        ValidateId(leagueId, nameof(leagueId));

        return await _leagueRepository.GetByIdAsync(leagueId, cancellationToken);
    }

    public async Task<IReadOnlyCollection<League>> GetUserLeaguesAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        ValidateId(userId, nameof(userId));

        var owned = await _leagueRepository.GetByOwnerIdAsync(userId, cancellationToken);

        var member = await _leagueRepository.GetByMemberIdAsync(userId, cancellationToken);

        return owned
            .Concat(member)
            .Distinct()
            .ToList()
            .AsReadOnly();
    }

    public async Task AddMemberAsync(Guid leagueId, User user, CancellationToken cancellationToken = default)
    {
        ValidateId(leagueId, nameof(leagueId));

        ArgumentNullException.ThrowIfNull(user);

        var league = await _leagueRepository.GetByIdAsync(leagueId, cancellationToken);

        if (league == null)
        {
            throw new KeyNotFoundException("League was not found.");
        }

        league.AddMember(user);

        await _leagueRepository.UpdateAsync(league, cancellationToken);

        await _leagueRepository.SaveChangesAsync(cancellationToken);
    }
    
    public async Task<League> UpdateLeagueAsync(League league, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(league);

        await _leagueRepository.UpdateAsync(league, cancellationToken);

        await _leagueRepository.SaveChangesAsync(cancellationToken);

        return league;
    }

    public async Task DeleteLeagueAsync(Guid leagueId, CancellationToken cancellationToken = default)
    {
        ValidateId(leagueId, nameof(leagueId));

        var league = await _leagueRepository.GetByIdAsync(leagueId, cancellationToken);

        if (league == null)
        {
            throw new KeyNotFoundException("League was not found.");
        }

        await _leagueRepository.DeleteAsync(league, cancellationToken);

        await _leagueRepository.SaveChangesAsync(cancellationToken);
    }

    private static void ValidateId(Guid id, string parameterName)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException(
                "Identifier cannot be empty.",
                parameterName);
        }
    }
}