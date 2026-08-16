using System.ComponentModel.DataAnnotations;

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
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new ArgumentException(
                "Username cannot be empty.",
                nameof(username)
            );
        }
        this.FirstName = firstName;
        this.LastName = lastName;
        this.Email = email;
        this.Username = username;
        this.CreatedAt = DateTime.UtcNow;
    }
}