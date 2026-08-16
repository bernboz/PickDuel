using PickDuel.Domain.Common;

namespace PickDuel.Domain.Entities;

public class Pick : Entity
{
    public User User { get; private set; }

    public League League { get; private set; }

    public Game Game { get; private set; }

    public string SelectedTeam { get; private set; }

    public DateTime CreatedAt { get; private set; }


    public Pick(
        User user,
        League league,
        Game game,
        string selectedTeam)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        if (league == null)
        {
            throw new ArgumentNullException(nameof(league));
        }

        if (game == null)
        {
            throw new ArgumentNullException(nameof(game));
        }

        if (selectedTeam != game.HomeTeam &&
            selectedTeam != game.AwayTeam)
        {
            throw new ArgumentException(
                "Selected team must be part of the game.",
                nameof(selectedTeam)
            );
        }

        User = user;
        League = league;
        Game = game;
        SelectedTeam = selectedTeam;
        CreatedAt = DateTime.UtcNow;
    }
}