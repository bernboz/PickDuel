using PickDuel.Domain.ValueObjects;

namespace PickDuel.Application.Scoring;

/// <summary>
/// Calculates scoring adjustments based on how accurately
/// a user predicts the final game score.
/// </summary>
public class ScoreAccuracyRule : IPickScoringRule
{
    private readonly ScoringSettings _settings;


    /// <summary>
    /// Initializes a score accuracy scoring rule.
    /// </summary>
    public ScoreAccuracyRule(ScoringSettings settings)
    {
        if (settings is null)
        {
            throw new ArgumentNullException(nameof(settings));
        }

        _settings = settings;
    }


    /// <summary>
    /// Calculates points based on predicted score accuracy.
    /// </summary>
    public int CalculatePoints(PickEvaluationContext context)
    {
        if (context is null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        if (context.Pick.ScorePrediction is null)
        {
            return 0;
        }


        var prediction = context.Pick.ScorePrediction;
        var result = context.GameResult;


        var homeDifference = Math.Abs(
            prediction.HomeScore - result.HomeScore
        );

        var awayDifference = Math.Abs(
            prediction.AwayScore - result.AwayScore
        );


        if (homeDifference == 0 && awayDifference == 0)
        {
            return _settings.ExactScorePoints;
        }


        var worstDifference = Math.Max(
            homeDifference,
            awayDifference
        );


        if (worstDifference <= _settings.ScoreTolerance)
        {
            return _settings.ScoreAccuracyBonus;
        }


        if (worstDifference >= _settings.MaxScoreDifferencePenalty)
        {
            return _settings.ScoreAccuracyPenalty;
        }


        var penaltyRange =
            _settings.MaxScoreDifferencePenalty -
            _settings.ScoreTolerance;

        var distanceIntoPenaltyRange =
            worstDifference -
            _settings.ScoreTolerance;


        var penaltyAmount =
            (decimal)Math.Abs(_settings.ScoreAccuracyPenalty)
            *
            distanceIntoPenaltyRange
            /
            penaltyRange;


        return -(int)Math.Ceiling(penaltyAmount);
    }
}