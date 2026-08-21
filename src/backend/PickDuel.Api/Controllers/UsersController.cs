using Microsoft.AspNetCore.Mvc;
using PickDuel.Application.DTOs.Users;
using PickDuel.Application.Mappers;
using PickDuel.Application.Users;
using PickDuel.Application.Mappers.Interfaces;

namespace PickDuel.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    private readonly IUserMapper _userMapper;

    public UsersController(
        IUserService userService,
        IUserMapper userMapper)
    {
        ArgumentNullException.ThrowIfNull(userService);
        ArgumentNullException.ThrowIfNull(userMapper);

        _userService = userService;
        _userMapper = userMapper;
    }


    /// <summary>
    /// Retrieves a user by identifier.
    /// </summary>
    /// <param name="id">
    /// User identifier.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation token.
    /// </param>
    /// <returns>
    /// User information if found.
    /// </returns>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserDto>> GetUser(
        Guid id,
        CancellationToken cancellationToken)
    {
        var user = await _userService.GetUserAsync(
            id,
            cancellationToken
        );

        if (user is null)
        {
            return NotFound();
        }

        return Ok(
            _userMapper.ToDto(user)
        );
    }


    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="request">
    /// User creation request.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation token.
    /// </param>
    /// <returns>
    /// Created user.
    /// </returns>
    [HttpPost]
    public async Task<ActionResult<UserDto>> CreateUser(
        CreateUserRequest request,
        CancellationToken cancellationToken)
    {
        var user = _userMapper.ToEntity(request);

        var createdUser = await _userService.CreateUserAsync(
            user,
            cancellationToken
        );

        return CreatedAtAction(
            nameof(GetUser),
            new
            {
                id = createdUser.Id
            },
            _userMapper.ToDto(createdUser)
        );
    }


    /// <summary>
    /// Updates an existing user.
    /// </summary>
    /// <param name="id">
    /// User identifier.
    /// </param>
    /// <param name="request">
    /// User update request.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation token.
    /// </param>
    /// <returns>
    /// Updated user.
    /// </returns>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<UserDto>> UpdateUser(
        Guid id,
        UpdateUserRequest request,
        CancellationToken cancellationToken)
    {
        var user = await _userService.GetUserAsync(
            id,
            cancellationToken
        );

        if (user is null)
        {
            return NotFound();
        }

        _userMapper.UpdateEntity(
            user,
            request
        );

        var updatedUser = await _userService.UpdateUserAsync(
            user,
            cancellationToken
        );

        return Ok(
            _userMapper.ToDto(updatedUser)
        );
    }


    /// <summary>
    /// Deletes an existing user.
    /// </summary>
    /// <param name="id">
    /// User identifier.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation token.
    /// </param>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteUser(
        Guid id,
        CancellationToken cancellationToken)
    {
        var user = await _userService.GetUserAsync(
            id,
            cancellationToken
        );

        if (user is null)
        {
            return NotFound();
        }

        await _userService.DeleteUserAsync(
            id,
            cancellationToken
        );

        return NoContent();
    }
}