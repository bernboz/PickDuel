using PickDuel.Application.DTOs.Users;
using PickDuel.Domain.Entities;

namespace PickDuel.Application.Mappers.Interfaces;

public interface IUserMapper
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
    UserDto ToDto(User user);


    /// <summary>
    /// Converts a create user request to a user entity.
    /// </summary>
    /// <param name="request">
    /// Create user request.
    /// </param>
    /// <returns>
    /// User entity.
    /// </returns>
    User ToEntity(CreateUserRequest request);


    /// <summary>
    /// Updates an existing user entity using an update request.
    /// </summary>
    /// <param name="user">
    /// User entity to update.
    /// </param>
    /// <param name="request">
    /// Updated user information.
    /// </param>
    void UpdateEntity(
        User user,
        UpdateUserRequest request);
}