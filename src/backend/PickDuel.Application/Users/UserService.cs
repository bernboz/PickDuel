using PickDuel.Application.Repositories.Interfaces;
using PickDuel.Domain.Entities;
using PickDuel.Application.Common;

namespace PickDuel.Application.Users;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;


    /// <summary>
    /// Initializes a new UserService.
    /// </summary>
    /// <param name="userRepository">Repository used for user persistence.</param>
    public UserService(IUserRepository userRepository)
    {
        ArgumentNullException.ThrowIfNull(userRepository);

        _userRepository = userRepository;
    }


    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="user">User to create.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The created user.</returns>
    public async Task<User> CreateUserAsync(User user, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(user);

        await _userRepository.AddAsync(user, cancellationToken);

        await _userRepository.SaveChangesAsync(cancellationToken);

        return user;
    }


    /// <summary>
    /// Retrieves a user by identifier.
    /// </summary>
    /// <param name="userId">User identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The user if found; otherwise null.</returns>
    public async Task<User?> GetUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        Guard.AgainstEmptyGuid(userId, nameof(userId));

        return await _userRepository.GetByIdAsync(userId, cancellationToken);
    }


    /// <summary>
    /// Retrieves a user by email address.
    /// </summary>
    /// <param name="email">User email address.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The user if found; otherwise null.</returns>
    public async Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(email);

        return await _userRepository.GetByEmailAsync(email, cancellationToken);
    }


    /// <summary>
    /// Retrieves a user by username.
    /// </summary>
    /// <param name="username">User username.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The user if found; otherwise null.</returns>
    public async Task<User?> GetUserByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(username);

        return await _userRepository.GetByUsernameAsync(username, cancellationToken);
    }


    /// <summary>
    /// Updates an existing user.
    /// </summary>
    /// <param name="user">Updated user.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The updated user.</returns>
    public async Task<User> UpdateUserAsync(User user, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(user);

        await _userRepository.UpdateAsync(user, cancellationToken);

        await _userRepository.SaveChangesAsync(cancellationToken);

        return user;
    }


    /// <summary>
    /// Deletes a user.
    /// </summary>
    /// <param name="userId">User identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public async Task DeleteUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        Guard.AgainstEmptyGuid(userId, nameof(userId));

        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);

        if (user == null)
        {
            throw new KeyNotFoundException("User was not found.");
        }

        await _userRepository.DeleteAsync(user, cancellationToken);

        await _userRepository.SaveChangesAsync(cancellationToken);
    }
}