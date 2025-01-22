using asp_rest_model.Models;

namespace asp_rest_model.Helpers;

public class InGameManager
{
    public static InGameState GiveInitialInGameState() => new InGameState()
    {
        state = StateManager.GiveResetState(),
        isFinished = false,
        player1Wins = false,
        player2Wins = false,
        isPLayer1Turn = true,
        isPlayer2Turn = false,
    };
}