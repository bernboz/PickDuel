using PickDuel.Domain.Common;

namespace PickDuel.Domain.Entities;

public class GameOdds : Entity
{
    public Game Game { get; private set; }

    public decimal HomeWinProbability { get; private set; }

    public decimal AwayWinProbability { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime? LockedAt { get; private set; }

    public bool IsLocked => LockedAt.HasValue;


    public GameOdds(Game game, decimal homeWinProbability, decimal awayWinProbability)
    {
        if (game == null)
        {
            throw new ArgumentNullException(nameof(game));
        }

        if (homeWinProbability <= 0 || homeWinProbability >= 1)
        {
            throw new ArgumentOutOfRangeException(nameof(homeWinProbability));
        }

        if (awayWinProbability <= 0 || awayWinProbability >= 1)
        {
            throw new ArgumentOutOfRangeException(nameof(awayWinProbability));
        }

        if (homeWinProbability + awayWinProbability != 1)
        {
            throw new ArgumentException(
                "Home and away probabilities must equal 1."
            );
        }

        Game = game;
        HomeWinProbability = homeWinProbability;
        AwayWinProbability = awayWinProbability;
        CreatedAt = DateTime.UtcNow;
    }


    public void Lock()
    {
        if (IsLocked)
        {
            throw new InvalidOperationException(
                "Game odds are already locked."
            );
        }

        LockedAt = DateTime.UtcNow;
    }
}