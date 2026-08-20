using PickDuel.Domain.Entities;

namespace PickDuel.Application.Users;

public interface IUserService
{
    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="user">User to create.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The created user.</returns>
    Task<User> CreateUserAsync(User user, CancellationToken cancellationToken = default);


    /// <summary>
    /// Retrieves a user by identifier.
    /// </summary>
    /// <param name="userId">User identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The user if found; otherwise null.</returns>
    Task<User?> GetUserAsync(Guid userId, CancellationToken cancellationToken = default);


    /// <summary>
    /// Retrieves a user by email address.
    /// </summary>
    /// <param name="email">User email address.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The user if found; otherwise null.</returns>
    Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default);


    /// <summary>
    /// Retrieves a user by username.
    /// </summary>
    /// <param name="username">User username.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The user if found; otherwise null.</returns>
    Task<User?> GetUserByUsernameAsync(string username, CancellationToken cancellationToken = default);


    /// <summary>
    /// Updates an existing user.
    /// </summary>
    /// <param name="user">Updated user.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The updated user.</returns>
    Task<User> UpdateUserAsync(User user, CancellationToken cancellationToken = default);


    /// <summary>
    /// Deletes a user.
    /// </summary>
    /// <param name="userId">User identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task DeleteUserAsync(Guid userId, CancellationToken cancellationToken = default);
}