using PickDuel.Domain.Common;

namespace PickDuel.Domain.Entities;

public class Game : Entity
{
    public string HomeTeam { get; private set; }
    public string AwayTeam { get; private set; }
    public DateTime StartTime { get; private set; }
    public DateTime EndTime { get; private set; }
    
    public Game(
        string homeTeam,
        string awayTeam,
        DateTime startTime,
        DateTime endTime)
    {
        if (string.IsNullOrWhiteSpace(homeTeam))
        {
            throw new ArgumentException(
                "Home team cannot be empty.",
                nameof(homeTeam)
            );
        }

        if (string.IsNullOrWhiteSpace(awayTeam))
        {
            throw new ArgumentException(
                "Away team cannot be empty.",
                nameof(awayTeam)
            );
        }

        HomeTeam = homeTeam;
        AwayTeam = awayTeam;
        StartTime = startTime;
        EndTime = endTime;
    }
}