using PickDuel.Domain.Common;

namespace PickDuel.Domain.Entities;

public class User : Entity
{
    public string FirstName { get; private set; }

    public string LastName { get; private set; }

    public string Email { get; private set; }

    public string Username { get; private set; }

    public DateTime CreatedAt { get; private set; }


    public User(string firstName, string lastName, string email, string username)
    {
        ValidateRequiredField(firstName, nameof(firstName));
        ValidateRequiredField(lastName, nameof(lastName));
        ValidateRequiredField(email, nameof(email));
        ValidateRequiredField(username, nameof(username));

        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Username = username;
        CreatedAt = DateTime.UtcNow;
    }


    /// <summary>
    /// Updates the user's profile information.
    /// </summary>
    /// <param name="firstName">
    /// Updated first name.
    /// </param>
    /// <param name="lastName">
    /// Updated last name.
    /// </param>
    /// <param name="email">
    /// Updated email address.
    /// </param>
    /// <param name="username">
    /// Updated username.
    /// </param>
    public void UpdateProfile(string firstName, string lastName, string email, string username)
    {
        ValidateRequiredField(firstName, nameof(firstName));

        ValidateRequiredField(lastName, nameof(lastName));

        ValidateRequiredField(email, nameof(email));

        ValidateRequiredField(username, nameof(username));

        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Username = username;
    }


    /// <summary>
    /// Validates that a required field is not null, empty, or whitespace.
    /// </summary>
    /// <param name="value">
    /// Value to validate.
    /// </param>
    /// <param name="parameterName">
    /// Name of the parameter being validated.
    /// </param>
    private static void ValidateRequiredField(string value, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException(
                $"{parameterName} cannot be empty.",
                parameterName
            );
        }
    }
}