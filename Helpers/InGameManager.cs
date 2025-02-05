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

    // TODO -> Verificar se está persistindo as atualizações no state[]
    public static void DeleteInGameState(string roomId)
    {
        InGameManager.InGameStates.Remove(InGameManager.InGameStates.FirstOrDefault(x => x.roomId == roomId));
    }

    public static InGameState ToggleCurrentPlayerTurn(string roomId)
    {
        var inGameState = GetInGameStateById(roomId);

        if (inGameState == null)
            throw new GenericApiError("Room does not exist");

        if (inGameState.isPLayer1Turn)
        {
            inGameState.isPLayer1Turn = false;
            inGameState.isPlayer2Turn = true;
        }
        else if (inGameState.isPlayer2Turn)
        {
            inGameState.isPLayer1Turn = true;
            inGameState.isPlayer2Turn = false;
        }
        
        UpdateFullState(inGameState);

        return inGameState;
    }

    public static InGameState ChangeState(string roomId, int choosePosition, int playerIndex)
    {
        if (choosePosition < 0 || choosePosition > 8)
            throw new GenericApiError("Invalid move position");

        var inGameState = GetInGameStateById(roomId);

        inGameState.state = StateManager.ChangeOnePosition(choosePosition, playerIndex, inGameState.state);

        UpdateFullState(inGameState);

        return inGameState;
    }


    // ações que se refletem no front
    public static InGameState HandleActionReceive(string roomId, string receiveObjectString)
    {
        var receiveObject = FormatsHelpers.ParseReceiveFromString(receiveObjectString);
        if (receiveObject == null)
            throw new GenericApiError("Object is poorly formatted");

        ChangeState(roomId, receiveObject.choosePosition, receiveObject.playerIndex);
        // var finalInGameState = ChangeState(roomId, receiveObject.choosePosition, receiveObject.playerIndex);

        ToggleCurrentPlayerTurn(roomId);


        var semiFinalInGameState = InGameManager.GetInGameStateById(roomId);
        
        if (semiFinalInGameState == null)
            throw new GenericApiError("Room does not exist");

        // WIN CHECKS
        var isDraw = StateManager.CheckForDraw(semiFinalInGameState.state);
        var isPLayer1Victory = StateManager.CheckWinForPlayer(1, semiFinalInGameState.state);
        var isPLayer2Victory = StateManager.CheckWinForPlayer(2, semiFinalInGameState.state);
        // 
        if (isDraw)
        {
            semiFinalInGameState.isDrawn = true;
            RoomStateManager.AddDrawToRoom(roomId);
        }
        else
        {
            if (isPLayer1Victory)
            {
                semiFinalInGameState.isPLayer1Turn = true;
                RoomStateManager.AddWinToPlayer(roomId, 1);
            }

            if (isPLayer2Victory)
            {
                RoomStateManager.AddWinToPlayer(roomId, 2);
                semiFinalInGameState.isPlayer2Turn = true;
            }
        }

        // states finais gerais
        if (isDraw || isPLayer1Victory || isPLayer2Victory)
        {
            semiFinalInGameState.isFinished = true;
            semiFinalInGameState.isPLayer1Turn = false;
            semiFinalInGameState.isPlayer2Turn = false;
        }
        
        UpdateFullState(semiFinalInGameState);
        
        var finalInGameState = InGameManager.GetInGameStateById(roomId);
        return semiFinalInGameState;
    }


    public static void UpdateFullState(InGameState newInGameState)
    {
        var id = newInGameState.roomId;
        InGameStates.RemoveAll(x => x.roomId == id);

        InGameStates.Add(newInGameState);
    }
}