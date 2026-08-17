using PickDuel.Domain.Entities;

namespace PickDuel.Application.Scoring;

public class PickEvaluationContext
{
    public Pick Pick { get; }
    public GameResult GameResult { get; }
    public GameOdds GameOdds { get; }


    public PickEvaluationContext(Pick pick, GameResult gameResult, GameOdds gameOdds)
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

        if (gameOdds == null)
        {
            throw new ArgumentNullException(nameof(gameOdds));
        }

        Pick = pick;
        GameResult = gameResult;
        GameOdds = gameOdds;
    }
}