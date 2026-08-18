using PickDuel.Domain.Common;

namespace PickDuel.Domain.Entities;

public class User : Entity
{
    public string FirstName { get; private set; }
    
    public string LastName { get; private set; }
    
    public string Email { get; private set; }
    
    public string Username { get; private set; }
    
    public DateTime CreatedAt { get; private set; }


    public User(
        string firstName,
        string lastName,
        string email,
        string username)
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


    private static void ValidateRequiredField(
        string value,
        string parameterName)
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