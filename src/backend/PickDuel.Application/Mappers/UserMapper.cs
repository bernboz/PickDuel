using PickDuel.Application.DTOs.Users;
using PickDuel.Application.Mappers.Interfaces;
using PickDuel.Domain.Entities;

namespace PickDuel.Application.Mappers;

public class UserMapper : IUserMapper
{
    /// <summary>
    /// Converts a user entity to a user DTO.
    /// </summary>
    /// <param name="user">
    /// User entity.
    /// </param>
    /// <returns>
    /// User DTO.
    /// </returns>
    public UserDto ToDto(User user)
    {
        ArgumentNullException.ThrowIfNull(user);

        return new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Username = user.Username,
            CreatedAt = user.CreatedAt
        };
    }


    /// <summary>
    /// Converts a create user request to a user entity.
    /// </summary>
    /// <param name="request">
    /// Create user request.
    /// </param>
    /// <returns>
    /// User entity.
    /// </returns>
    public User ToEntity(CreateUserRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        return new User(
            request.FirstName,
            request.LastName,
            request.Email,
            request.Username
        );
    }


    /// <summary>
    /// Applies an update request to an existing user entity.
    /// </summary>
    /// <param name="user">
    /// User entity to update.
    /// </param>
    /// <param name="request">
    /// Updated user information.
    /// </param>
    public void UpdateEntity(User user, UpdateUserRequest request)
    {
        ArgumentNullException.ThrowIfNull(user);
        ArgumentNullException.ThrowIfNull(request);

        user.UpdateProfile(
            request.FirstName,
            request.LastName,
            request.Email,
            request.Username
        );
    }
}