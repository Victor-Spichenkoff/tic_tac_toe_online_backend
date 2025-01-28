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
        InGameState? inGameState;

        try
        {
            inGameState = InGameManager.HandleActionReceive(roomId, message);
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


        return new SocketInGameResponse()
        {
            isError = false,
            inGameState = inGameState
        };
    }
}