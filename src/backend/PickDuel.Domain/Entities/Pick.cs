using PickDuel.Domain.Common;
using PickDuel.Domain.ValueObjects;
using PickDuel.Domain.Entities.Predictions;

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
    /// Creates a winner prediction pick.
    /// </summary>
    public Pick(
        User user,
        League league,
        Game game,
        string selectedTeam,
        int confidenceMultiplier)
    {
        ValidateBasePick(
            user,
            league,
            game,
            selectedTeam,
            confidenceMultiplier
        );

        User = user;
        League = league;
        Game = game;
        SelectedTeam = selectedTeam;
        ConfidenceMultiplier = confidenceMultiplier;
        CreatedAt = DateTime.UtcNow;
    }


    /// <summary>
    /// Creates a pick with an exact score prediction.
    /// </summary>
    public Pick(
        User user,
        League league,
        Game game,
        string selectedTeam,
        int confidenceMultiplier,
        ScorePrediction scorePrediction)
        : this(
            user,
            league,
            game,
            selectedTeam,
            confidenceMultiplier)
    {
        if (scorePrediction == null)
        {
            throw new ArgumentNullException(nameof(scorePrediction));
        }

        ScorePrediction = scorePrediction;
    }


    /// <summary>
    /// Changes the confidence multiplier for this pick.
    /// </summary>
    public void ChangeConfidence(int newConfidence)
    {
        EnsurePickIsEditable();

        ValidateConfidence(newConfidence);

        ConfidenceMultiplier = newConfidence;
    }


    /// <summary>
    /// Changes the selected team for this pick.
    /// </summary>
    public void ChangeSelection(string newTeam)
    {
        EnsurePickIsEditable();

        ValidateSelectedTeam(Game, newTeam);

        SelectedTeam = newTeam;
    }


    /// <summary>
    /// Updates the exact score prediction.
    /// </summary>
    public void UpdateScorePrediction(ScorePrediction prediction)
    {
        EnsurePickIsEditable();

        if (prediction == null)
        {
            throw new ArgumentNullException(nameof(prediction));
        }

        ScorePrediction = prediction;
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


    private static void ValidateBasePick(
        User user,
        League league,
        Game game,
        string selectedTeam,
        int confidenceMultiplier)
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
    }


    private static void ValidateSelectedTeam(
        Game game,
        string selectedTeam)
    {
        if (string.IsNullOrWhiteSpace(selectedTeam) ||
            (selectedTeam != game.HomeTeam &&
             selectedTeam != game.AwayTeam))
        {
            throw new ArgumentException(
                "Selected team must be part of the game.",
                nameof(selectedTeam)
            );
        }
    }


    private static void ValidateConfidence(
        int confidenceMultiplier)
    {
        if (confidenceMultiplier < 1 ||
            confidenceMultiplier > 5)
        {
            throw new ArgumentOutOfRangeException(
                nameof(confidenceMultiplier)
            );
        }
    }
}