using System.Text.Json;
using asp_rest_model.Helpers;
using asp_rest_model.Models;

namespace asp_rest_model.handlers;

public static class SocketMessageHandler
{
    /*
     * Não configura diretamente o response, apenas o retorna
     * Lida com os erros, retorna um objeto com as infos necessárias/*
     */
    public static SocketInGameResponse HandleMessage(string roomId, string message)
    {
        try
        {
            var bodyObject = FormatsHelpers.ParseReceiveFromString(message);
            if (bodyObject == null)
                throw new FormatException("Invalid message");

                if (bodyObject.isResetOperation is true)
                {
                    var restartedResponse = HandleResetMode(roomId);
                    return restartedResponse;
                }

            // resposta normal (success ou erro)
            var normalResponse = HandleNormalMode(roomId, message);
            return normalResponse;
        }
        catch (GenericApiError myError)
        {
            return new SocketInGameResponse()
            {
                isError = false,
                errorMessage = myError.Message,
            };
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex);
            Console.WriteLine("Erro inesperado no socket 🔝🔝🔝");

            return new SocketInGameResponse()
            {
                isError = false,
                errorMessage = "Unexpected error with connection!",
            };
        }
    }

    //não lida com room, apenas reseta o ingame
    private static SocketInGameResponse HandleResetMode(string roomId)
    {
        var inGameCurrentState = InGameManager.GetInGameStateById(roomId);
        var roomInfos = RoomStateManager.GetRoomStateById(roomId);

        if (inGameCurrentState == null || roomInfos == null)
            throw new GenericApiError("Room doesn't exist");

        inGameCurrentState.isFinished = false;
        inGameCurrentState.isDrawn = false;
        inGameCurrentState.state = StateManager.GiveResetState();
        inGameCurrentState.player1Wins = false;
        inGameCurrentState.player2Wins = false;

        // is par == true -> o x joga
        var isEven = GeneralHelper.IsEven(roomInfos.matchCount);

        inGameCurrentState.isPLayer1Turn = isEven;
        inGameCurrentState.isPlayer2Turn = !isEven;

        InGameManager.UpdateFullState(inGameCurrentState);
        // para testes 
        var newInGameState = InGameManager.GetInGameStateById(roomId);

        return new SocketInGameResponse()
        {
            isError = false,
            inGameState = inGameCurrentState,
        };
    }


    // Em jogo mesmo, nada de diferente
    private static SocketInGameResponse HandleNormalMode(string roomId, string message)
    {
        InGameState inGameState = InGameManager.HandleActionReceive(roomId, message);

        return new SocketInGameResponse()
        {
            isError = false,
            inGameState = inGameState
        };
    }
}