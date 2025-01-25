using System.Text.Json;
using System.Text.Json.Nodes;
using asp_rest_model.Models;

namespace asp_rest_model.Helpers;

public class InGameManager
{
    public static List<InGameState> InGameStates = [];


    public static InGameState? GetInGameStateById(string roomId)
    {
        var test = InGameManager.InGameStates;
        return InGameManager.InGameStates.FirstOrDefault(x => x.roomId == roomId);
    }
    
    public static bool AddNewInGameState(InGameState inGameState)
    {
        InGameManager.InGameStates.Add(inGameState);

        return true;
    }
    
    
    public static InGameState GiveInitialInGameState(string roomId)
    {
        var state = new InGameState()
        {
            roomId = roomId,
            state = StateManager.GiveResetState(),
            isFinished = false,
            player1Wins = false,
            player2Wins = false,
            isPLayer1Turn = true,
            isPlayer2Turn = false,
        };

        return state;
    }
    
    
    // ações que se refletem no front
    public static InGameState HandleActionReceive(string roomId, string inGameAsString)
    {
        var inGameObject = FormatsHelpers.ParseInGameFromString(inGameAsString);
        if (inGameObject == null)
            throw new GenericApiError("FUNCIONOU");

        return inGameObject;
    }
}