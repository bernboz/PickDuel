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


    public PlayoffMatchup(PlayoffRound round, User userOne, User userTwo)
    {
        ArgumentNullException.ThrowIfNull(round);
        ArgumentNullException.ThrowIfNull(userOne);
        ArgumentNullException.ThrowIfNull(userTwo);

        if (userOne == userTwo)
        {
            throw new ArgumentException(
                "Users in a playoff matchup must be different."
            );
        }

        Round = round;
        UserOne = userOne;
        UserTwo = userTwo;

        IsCompleted = false;

        CreatedAt = DateTime.UtcNow;
    }


    /// <summary>
    /// Completes the matchup and advances the winner.
    /// </summary>
    public void Complete(User winner)
    {
        ArgumentNullException.ThrowIfNull(winner);

        if (winner != UserOne && winner != UserTwo)
        {
            throw new InvalidOperationException(
                "Winner must participate in the matchup."
            );
        }

        Winner = winner;

        IsCompleted = true;
    }
}