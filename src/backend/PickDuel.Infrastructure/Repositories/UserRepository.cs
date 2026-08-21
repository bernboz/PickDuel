using Microsoft.EntityFrameworkCore;
using PickDuel.Application.Repositories.Interfaces;
using PickDuel.Domain.Entities;
using PickDuel.Infrastructure.Data;

namespace PickDuel.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly PickDuelDbContext _context;


    public UserRepository(PickDuelDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        _context = context;
    }


    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(user);

        await _context.Users.AddAsync(user, cancellationToken);
    }


    public async Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);
    }


    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(email);

        return await _context.Users
            .FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
    }


    public async Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(username);

        return await _context.Users
            .FirstOrDefaultAsync(x => x.Username == username, cancellationToken);
    }


    public Task UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(user);

        _context.Users.Update(user);

        return Task.CompletedTask;
    }


    public Task DeleteAsync(User user, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(user);

        _context.Users.Remove(user);

        return Task.CompletedTask;
    }


    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}