namespace PickDuel.Application.DTOs.Users;

public class CreateUserRequest
{
    public string FirstName { get; init; }

    public string LastName { get; init; }

    public string Email { get; init; }

    public string Username { get; init; }
}