using PickDuel.Domain.Entities;

namespace PickDuel.Application.Scoring;

public class PickEvaluationContext
{
    public Pick Pick { get; }

    public GameResult GameResult { get; }

    public PickEvaluationContext(Pick pick, GameResult gameResult)
    {
        if (pick == null)
        {
            throw new ArgumentNullException(nameof(pick));
        }

        if (gameResult == null)
        {
            throw new ArgumentNullException(nameof(gameResult));
        }

        if (pick.Game != gameResult.Game)
        {
            throw new ArgumentException(
                "The pick and game result must reference the same game.");
        }

        Pick = pick;
        GameResult = gameResult;
    }
}