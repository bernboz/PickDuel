using PickDuel.Domain.Common;

namespace PickDuel.Domain.Entities;

public class LeagueStanding : Entity
{
    public User User { get; private set; }

    public League League { get; private set; }

    public int TotalPoints { get; private set; }


    public LeagueStanding(
        User user,
        League league)
    {
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        if (league is null)
        {
            throw new ArgumentNullException(nameof(league));
        }


        User = user;
        League = league;
        TotalPoints = 0;
    }


    public void AddPoints(int points)
    {
        TotalPoints += points;
    }
}