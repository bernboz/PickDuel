using PickDuel.Domain.Common;
using PickDuel.Domain.ValueObjects;
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


    public bool IsLocked { get; private set; }


    public bool IsScored { get; private set; }


    public DateTime CreatedAt { get; private set; }


    /// <summary>
    /// Creates a winner prediction pick.
    /// </summary>
    /// <param name="user">User making the prediction.</param>
    /// <param name="league">League containing the pick.</param>
    /// <param name="game">Game being predicted.</param>
    /// <param name="selectedTeam">Team selected as the winner.</param>
    /// <param name="confidenceMultiplier">Confidence value assigned to the pick.</param>
    public Pick(User user, League league, Game game, string selectedTeam, int confidenceMultiplier)
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

        IsLocked = false;
        IsScored = false;

        CreatedAt = DateTime.UtcNow;
    }


    /// <summary>
    /// Creates a pick with an exact score prediction.
    /// </summary>
    /// <param name="user">User making the prediction.</param>
    /// <param name="league">League containing the pick.</param>
    /// <param name="game">Game being predicted.</param>
    /// <param name="selectedTeam">Team selected as the winner.</param>
    /// <param name="confidenceMultiplier">Confidence value assigned to the pick.</param>
    /// <param name="scorePrediction">Predicted final score.</param>
    public Pick(User user, League league, Game game, string selectedTeam, int confidenceMultiplier, ScorePrediction scorePrediction)
        : this(user, league, game, selectedTeam, confidenceMultiplier)
    {
        ArgumentNullException.ThrowIfNull(scorePrediction);

        ScorePrediction = scorePrediction;
    }


    /// <summary>
    /// Changes the confidence multiplier for this pick.
    /// </summary>
    /// <param name="newConfidence">New confidence multiplier value.</param>
    public void ChangeConfidence(int newConfidence)
    {
        EnsurePickIsEditable();

        ValidateConfidence(newConfidence);

        ConfidenceMultiplier = newConfidence;
    }


    /// <summary>
    /// Changes the selected team for this pick.
    /// </summary>
    /// <param name="newTeam">New selected team.</param>
    public void ChangeSelection(string newTeam)
    {
        EnsurePickIsEditable();

        ValidateSelectedTeam(Game, newTeam);

        SelectedTeam = newTeam;
    }


    /// <summary>
    /// Updates the exact score prediction.
    /// </summary>
    /// <param name="prediction">Updated score prediction.</param>
    public void UpdateScorePrediction(ScorePrediction prediction)
    {
        EnsurePickIsEditable();

        ArgumentNullException.ThrowIfNull(prediction);

        ScorePrediction = prediction;
    }


    /// <summary>
    /// Locks the pick and prevents further modifications.
    /// </summary>
    public void Lock()
    {
        if (IsLocked)
        {
            throw new InvalidOperationException(
                "Pick is already locked."
            );
        }

        if (Game.HasStarted)
        {
            throw new InvalidOperationException(
                "Cannot lock pick after the game has started."
            );
        }

        IsLocked = true;
    }


    /// <summary>
    /// Marks the pick as scored after a completed game evaluation.
    /// </summary>
    public void MarkAsScored()
    {
        if (!IsLocked)
        {
            throw new InvalidOperationException(
                "Cannot score a pick that is not locked."
            );
        }

        if (IsScored)
        {
            throw new InvalidOperationException(
                "Pick has already been scored."
            );
        }

        IsScored = true;
    }


    /// <summary>
    /// Ensures the pick can still be modified.
    /// </summary>
    private void EnsurePickIsEditable()
    {
        if (IsLocked || Game.HasStarted)
        {
            throw new InvalidOperationException(
                "Pick can no longer be modified."
            );
        }
    }
    
    public void EnsureNotScored()
    {
        if (IsScored)
        {
            throw new InvalidOperationException(
                "Pick has already been scored."
            );
        }
    }
    
    /// <summary>
    /// Validates the base pick information.
    /// </summary>
    private static void ValidateBasePick(User user, League league, Game game, string selectedTeam, int confidenceMultiplier)
    {
        ArgumentNullException.ThrowIfNull(user);
        ArgumentNullException.ThrowIfNull(league);
        ArgumentNullException.ThrowIfNull(game);

        ValidateSelectedTeam(game, selectedTeam);

        ValidateConfidence(confidenceMultiplier);
    }


    /// <summary>
    /// Validates that the selected team belongs to the game.
    /// </summary>
    private static void ValidateSelectedTeam(Game game, string selectedTeam)
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


    /// <summary>
    /// Validates the confidence multiplier range.
    /// </summary>
    private static void ValidateConfidence(int confidenceMultiplier)
    {
        if (confidenceMultiplier < 1 || confidenceMultiplier > 5)
        {
            throw new ArgumentOutOfRangeException(
                nameof(confidenceMultiplier)
            );
        }
    }
}