using PickDuel.Domain.Entities;

namespace PickDuel.Application.Repositories.Interfaces;

public interface IUserRepository
{
    /// <summary>
    /// Adds a new user to the data store.
    /// </summary>
    /// <param name="user">User entity to add.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task AddAsync(User user, CancellationToken cancellationToken = default);


    /// <summary>
    /// Retrieves a user by identifier.
    /// </summary>
    /// <param name="userId">User identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The user if found; otherwise null.</returns>
    Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default);


    /// <summary>
    /// Retrieves a user by email address.
    /// </summary>
    /// <param name="email">Email address.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The matching user if found; otherwise null.</returns>
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);


    /// <summary>
    /// Retrieves a user by username.
    /// </summary>
    /// <param name="username">Username.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The matching user if found; otherwise null.</returns>
    Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);


    /// <summary>
    /// Updates an existing user.
    /// </summary>
    /// <param name="user">Updated user entity.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task UpdateAsync(User user, CancellationToken cancellationToken = default);


    /// <summary>
    /// Removes a user.
    /// </summary>
    /// <param name="user">User entity to remove.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task DeleteAsync(User user, CancellationToken cancellationToken = default);


    /// <summary>
    /// Saves all pending repository changes.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}