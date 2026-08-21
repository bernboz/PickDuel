using PickDuel.Domain.Common;

namespace PickDuel.Domain.Entities;

public class Game : Entity
{
    public string HomeTeam { get; private set; }

    public string AwayTeam { get; private set; }

    public DateTime StartTime { get; private set; }

    public DateTime EndTime { get; private set; }

    public int? HomeScore { get; private set; }

    public int? AwayScore { get; private set; }

    public string? WinningTeam { get; private set; }

    public bool HasStarted => DateTime.UtcNow >= StartTime;

    public bool IsCompleted => HomeScore.HasValue && AwayScore.HasValue;


    public Game(string homeTeam, string awayTeam, DateTime startTime, DateTime endTime)
    {
        if (string.IsNullOrWhiteSpace(homeTeam))
        {
            throw new ArgumentException("Home team cannot be empty.", nameof(homeTeam));
        }

        if (string.IsNullOrWhiteSpace(awayTeam))
        {
            throw new ArgumentException("Away team cannot be empty.", nameof(awayTeam));
        }

        if (startTime >= endTime)
        {
            throw new ArgumentException("Game start time must be before end time.");
        }

        if (homeTeam == awayTeam)
        {
            throw new ArgumentException("Home and away teams cannot be the same.");
        }

        HomeTeam = homeTeam;
        AwayTeam = awayTeam;
        StartTime = startTime;
        EndTime = endTime;
    }
    
    /// <summary>
    /// Completes the game by recording the final score and determining the winner.
    /// </summary>
    /// <param name="homeScore">
    /// Final score for the home team.
    /// </param>
    /// <param name="awayScore">
    /// Final score for the away team.
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the game has already been completed.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when a score is negative.
    /// </exception>
    public void CompleteGame(int homeScore, int awayScore)
    {
        if (IsCompleted)
        {
            throw new InvalidOperationException(
                "Game has already been completed."
            );
        }

        if (homeScore < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(homeScore),
                "Home score cannot be negative."
            );
        }

        if (awayScore < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(awayScore),
                "Away score cannot be negative."
            );
        }

        HomeScore = homeScore;
        AwayScore = awayScore;

        WinningTeam =
            homeScore > awayScore
                ? HomeTeam
                : awayScore > homeScore
                    ? AwayTeam
                    : null;
    }
}