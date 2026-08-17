using PickDuel.Domain.Common;

namespace PickDuel.Domain.Entities;

public class Pick : Entity
{
    public User User { get; private set; }

    public League League { get; private set; }

    public Game Game { get; private set; }

    public string SelectedTeam { get; private set; }

    public int ConfidenceMultiplier { get; private set; }

    public DateTime CreatedAt { get; private set; }


    public Pick(User user, League league, Game game, string selectedTeam, int confidenceMultiplier)
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

        ValidateSelectedTeam(game, selectedTeam);
        ValidateConfidence(confidenceMultiplier);

        User = user;
        League = league;
        Game = game;
        SelectedTeam = selectedTeam;
        ConfidenceMultiplier = confidenceMultiplier;
        CreatedAt = DateTime.UtcNow;
    }


    public void ChangeConfidence(int newConfidence)
    {
        EnsurePickIsEditable();

        ValidateConfidence(newConfidence);

        ConfidenceMultiplier = newConfidence;
    }


    public void ChangeSelection(string newTeam)
    {
        EnsurePickIsEditable();

        ValidateSelectedTeam(Game, newTeam);

        SelectedTeam = newTeam;
    }


    private void EnsurePickIsEditable()
    {
        if (Game.HasStarted)
        {
            throw new InvalidOperationException(
                "Pick can no longer be modified because the game has started."
            );
        }
    }


    private static void ValidateSelectedTeam(Game game, string selectedTeam)
    {
        if (selectedTeam != game.HomeTeam && selectedTeam != game.AwayTeam)
        {
            throw new ArgumentException(
                "Selected team must be part of the game.",
                nameof(selectedTeam)
            );
        }
    }


    private static void ValidateConfidence(int confidenceMultiplier)
    {
        if (confidenceMultiplier < 1 || confidenceMultiplier > 5)
        {
            throw new ArgumentOutOfRangeException(nameof(confidenceMultiplier));
        }
    }
}