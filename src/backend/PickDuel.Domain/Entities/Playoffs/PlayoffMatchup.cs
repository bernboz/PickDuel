using PickDuel.Domain.Common;

namespace PickDuel.Domain.Entities.Playoffs;

public class PlayoffMatchup : Entity
{
    public PlayoffRound Round { get; private set; }


    public User UserOne { get; private set; }


    public User UserTwo { get; private set; }


    public User? Winner { get; private set; }


    public bool IsCompleted { get; private set; }


    public DateTime CreatedAt { get; private set; }


    public PlayoffMatchup(
        PlayoffRound round,
        User userOne,
        User userTwo)
    {
        ArgumentNullException.ThrowIfNull(round);
        ArgumentNullException.ThrowIfNull(userOne);
        ArgumentNullException.ThrowIfNull(userTwo);

        if (userOne == userTwo)
        {
            throw new ArgumentException(
                "Users must be different."
            );
        }

        Round = round;
        UserOne = userOne;
        UserTwo = userTwo;

        CreatedAt = DateTime.UtcNow;
    }


    /// <summary>
    /// Completes the matchup with the winning user.
    /// </summary>
    public void Complete(User winner)
    {
        ArgumentNullException.ThrowIfNull(winner);

        if (IsCompleted)
        {
            throw new InvalidOperationException(
                "Matchup already completed."
            );
        }

        if (winner != UserOne &&
            winner != UserTwo)
        {
            throw new InvalidOperationException(
                "Winner must participate in matchup."
            );
        }

        Winner = winner;
        IsCompleted = true;
    }


    /// <summary>
    /// Gets the opposing user in this matchup.
    /// </summary>
    public User GetOpponent(User user)
    {
        ArgumentNullException.ThrowIfNull(user);

        if (user == UserOne)
        {
            return UserTwo;
        }

        if (user == UserTwo)
        {
            return UserOne;
        }

        throw new InvalidOperationException(
            "User is not part of this matchup."
        );
    }
}