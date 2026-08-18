using PickDuel.Domain.Common;

namespace PickDuel.Domain.Entities;

public class Game : Entity
{
    public string HomeTeam { get; private set; }

    public string AwayTeam { get; private set; }

    public DateTime StartTime { get; private set; }

    public DateTime EndTime { get; private set; }

    public bool HasStarted => DateTime.UtcNow >= StartTime;


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
            throw new ArgumentException(
                "Home and away teams cannot be the same."
            );
        }

        HomeTeam = homeTeam;
        AwayTeam = awayTeam;
        StartTime = startTime;
        EndTime = endTime;
    }
}