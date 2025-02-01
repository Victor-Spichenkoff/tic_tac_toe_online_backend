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

        return inGameState;
    }

    public static InGameState ChangeState(string roomId, int choosePosition, int playerIndex)
    {
        if (choosePosition < 0 || choosePosition > 8)
            throw new GenericApiError("Invalid move position");

        var inGameState = GetInGameStateById(roomId);

        inGameState.state = StateManager.ChangeOnePosition(choosePosition, playerIndex, inGameState.state);

        //todo
        // verificar se está ganhou e enviar globalmente

        return inGameState;
    }


    // ações que se refletem no front
    public static InGameState HandleActionReceive(string roomId, string receiveObjectString)
    {
        //TODo
        //não está mudando o state, nem playerXTurn
        var receiveObject = FormatsHelpers.ParseReceiveFromString(receiveObjectString);
        if (receiveObject == null)
            throw new GenericApiError("FUNCIONOU");

        ChangeState(roomId, receiveObject.choosePosition, receiveObject.playerIndex);
        // var finalInGameState = ChangeState(roomId, receiveObject.choosePosition, receiveObject.playerIndex);

        ToggleCurrentPlayerTurn(roomId);

        //TODO 
        // check de vitoria
        //

        // final mesmo
        var finalInGameState = InGameManager.GetInGameStateById(roomId);

        if (finalInGameState == null)
            throw new GenericApiError("Room does not exist");

        // WIN CHECKS
        var isDraw = StateManager.CheckForDraw(finalInGameState.state);
        var isPLayer1Victory = StateManager.CheckWinForPlayer(1, finalInGameState.state);
        var isPLayer2Victory = StateManager.CheckWinForPlayer(1, finalInGameState.state);
        // 
        if (isDraw)
        {
            finalInGameState.isDrawn = true;
            RoomStateManager.AddDrawToRoom(roomId);
        }
        else
        {
            if (isPLayer1Victory)
            {
                finalInGameState.isPLayer1Turn = true;
                RoomStateManager.AddWinToPlayer(roomId, 1);
            }

            if (isPLayer2Victory)
            {
                RoomStateManager.AddWinToPlayer(roomId, 2);
                finalInGameState.isPlayer2Turn = true;
            }
        }

        // states finais gerais
        if (isDraw || isPLayer1Victory || isPLayer2Victory)
        {
            finalInGameState.isFinished = true;
            finalInGameState.isPLayer1Turn = false;
            finalInGameState.isPlayer2Turn = false;
        }


        return finalInGameState;
    }
}