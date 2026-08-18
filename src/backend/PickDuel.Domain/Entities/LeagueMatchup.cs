using PickDuel.Domain.Common;
using PickDuel.Domain.Entities.History;
using PickDuel.Domain.Enums;

namespace PickDuel.Domain.Entities.Matchups;

public class LeagueMatchup : Entity
{
    public League League { get; private set; }

    public User UserOne { get; private set; }

    public User UserTwo { get; private set; }


    public DateTime StartDate { get; private set; }

    public DateTime EndDate { get; private set; }


    public MatchupStatus Status { get; private set; }

    public MatchupResult Result { get; private set; }


    public int UserOnePoints { get; private set; }

    public int UserTwoPoints { get; private set; }


    private readonly List<PickHistory> _pickHistories = new();

    public IReadOnlyCollection<PickHistory> PickHistories =>
        _pickHistories.AsReadOnly();


    public DateTime CreatedAt { get; private set; }

    public DateTime? CompletedAt { get; private set; }


    public LeagueMatchup(
        League league,
        User userOne,
        User userTwo,
        DateTime startDate,
        DateTime endDate)
    {
        ArgumentNullException.ThrowIfNull(league);
        ArgumentNullException.ThrowIfNull(userOne);
        ArgumentNullException.ThrowIfNull(userTwo);


        if (userOne == userTwo)
        {
            throw new ArgumentException(
                "A matchup requires two different users."
            );
        }


        if (startDate >= endDate)
        {
            throw new ArgumentException(
                "Matchup start date must be before end date."
            );
        }


        League = league;

        UserOne = userOne;
        UserTwo = userTwo;

        StartDate = startDate;
        EndDate = endDate;


        Status = MatchupStatus.Scheduled;
        Result = MatchupResult.Pending;


        UserOnePoints = 0;
        UserTwoPoints = 0;


        CreatedAt = DateTime.UtcNow;
    }


    public void Lock()
    {
        EnsureStatus(MatchupStatus.Scheduled);

        if (DateTime.UtcNow < StartDate)
        {
            throw new InvalidOperationException(
                "Matchup cannot be locked before the start date."
            );
        }

        Status = MatchupStatus.Locked;
    }


    public void AddPickHistory(
        PickHistory pickHistory)
    {
        ArgumentNullException.ThrowIfNull(pickHistory);

        EnsurePickSubmissionAllowed();


        if (pickHistory.League != League)
        {
            throw new InvalidOperationException(
                "Pick history must belong to this league."
            );
        }


        if (pickHistory.User != UserOne &&
            pickHistory.User != UserTwo)
        {
            throw new InvalidOperationException(
                "Pick history user must participate in this matchup."
            );
        }


        _pickHistories.Add(pickHistory);

        RecalculatePoints();
    }


    public void Complete()
    {
        EnsureStatus(MatchupStatus.Locked);

        RecalculatePoints();

        DetermineResult();

        Status = MatchupStatus.Completed;

        CompletedAt = DateTime.UtcNow;
    }


    private void RecalculatePoints()
    {
        UserOnePoints = _pickHistories
            .Where(x => x.User == UserOne)
            .Sum(x => x.PointsEarned);


        UserTwoPoints = _pickHistories
            .Where(x => x.User == UserTwo)
            .Sum(x => x.PointsEarned);
    }


    private void DetermineResult()
    {
        if (UserOnePoints > UserTwoPoints)
        {
            Result = MatchupResult.UserOneWin;
            return;
        }


        if (UserTwoPoints > UserOnePoints)
        {
            Result = MatchupResult.UserTwoWin;
            return;
        }


        Result = MatchupResult.Tie;
    }


    private void EnsurePickSubmissionAllowed()
    {
        if (Status != MatchupStatus.Scheduled)
        {
            throw new InvalidOperationException(
                "Picks cannot be added after the matchup is locked."
            );
        }
    }


    private void EnsureStatus(
        MatchupStatus expected)
    {
        if (Status != expected)
        {
            throw new InvalidOperationException(
                $"Matchup must be {expected}."
            );
        }
    }
}