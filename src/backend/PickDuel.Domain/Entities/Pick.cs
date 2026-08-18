using PickDuel.Domain.Common;
using PickDuel.Domain.ValueObjects;

namespace PickDuel.Domain.Entities;

public class Pick : Entity
{
    public User User { get; private set; }

    public League League { get; private set; }

    public Game Game { get; private set; }

    public string SelectedTeam { get; private set; }

    public int ConfidenceMultiplier { get; private set; }

    public ScorePrediction? ScorePrediction { get; private set; }

    public DateTime CreatedAt { get; private set; }


    /// <summary>
    /// Creates a pick for the specified team and confidence multiplier.
    /// </summary>
    /// <param name="user">User making the prediction.</param>
    /// <param name="league">League where the prediction exists.</param>
    /// <param name="game">Game being predicted.</param>
    /// <param name="selectedTeam">Team selected by the user.</param>
    /// <param name="confidenceMultiplier">Confidence value from 1-5.</param>
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


    /// <summary>
    /// Changes the confidence multiplier for this pick.
    /// </summary>
    /// <param name="newConfidence">New confidence value from 1-5.</param>
    public void ChangeConfidence(int newConfidence)
    {
        EnsurePickIsEditable();

        ValidateConfidence(newConfidence);

        ConfidenceMultiplier = newConfidence;
    }


    /// <summary>
    /// Changes the selected team for this pick.
    /// </summary>
    /// <param name="newTeam">New team selected by the user.</param>
    public void ChangeSelection(string newTeam)
    {
        EnsurePickIsEditable();

        ValidateSelectedTeam(Game, newTeam);

        SelectedTeam = newTeam;
    }


    /// <summary>
    /// Updates the exact score prediction for this pick.
    /// </summary>
    /// <param name="prediction">Predicted score for both teams.</param>
    public void UpdateScorePrediction(ScorePrediction prediction)
    {
        EnsurePickIsEditable();

        if (prediction == null)
        {
            throw new ArgumentNullException(nameof(prediction));
        }

        ScorePrediction = prediction;
    }


    /// <summary>
    /// Ensures the pick can still be modified before game start.
    /// </summary>
    private void EnsurePickIsEditable()
    {
        if (Game.HasStarted)
        {
            throw new InvalidOperationException(
                "Pick can no longer be modified because the game has started."
            );
        }
    }


    /// <summary>
    /// Validates that the selected team is part of the game.
    /// </summary>
    /// <param name="game">Game being predicted.</param>
    /// <param name="selectedTeam">Team selected by the user.</param>
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


    /// <summary>
    /// Validates that confidence is within the allowed range.
    /// </summary>
    /// <param name="confidenceMultiplier">Confidence value to validate.</param>
    private static void ValidateConfidence(int confidenceMultiplier)
    {
        if (confidenceMultiplier < 1 || confidenceMultiplier > 5)
        {
            throw new ArgumentOutOfRangeException(nameof(confidenceMultiplier));
        }
    }
}