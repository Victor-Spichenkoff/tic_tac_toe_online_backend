using asp_rest_model.Models;
using asp_rest_model.Sockets;

namespace asp_rest_model.Helpers;

public class RoomStateManager
{
    //ERRO AQUI, retorna um básico, só o id está certo, tá desligando sem precisar
    public static List<RoomState> roomStates = new List<RoomState>();

    public static RoomState AddNewRoom(string roomId)
    {
        var initialState = new RoomState
        {
            roomId = roomId,
            drawsCount = 0,
            player1Points = 0,
            player2Points = 0,
            isPLayer1Connected = true,
            isPLayer2Connected = false
        };
        
        RoomStateManager.roomStates.Add(initialState);
        
        return initialState;
    }


    public static RoomState? GetRoomStateById(string roomId)
    {
 
        var result = roomStates.FirstOrDefault(x => x.roomId == roomId);
        return result;
    }

    public static bool UpdateRoom(RoomState newRoomState)
    {
        if(!RoomManager.RoomExists(newRoomState.roomId))
            return false;
        
        var index = roomStates.FindIndex(x => x.roomId == newRoomState.roomId);
        
        roomStates[index] = newRoomState;


        return true;
    }

    public static void AddDrawToRoom(string roomId)
    {
        var room = roomStates.FirstOrDefault(x => x.roomId == roomId);
        if(room == null)
            throw new GenericApiError("Room doesn't exist");
        
        room.drawsCount++;
    }
    
    public static void AddWinToPlayer(string roomId, int playerIndex)
    {
        var room = roomStates.FirstOrDefault(x => x.roomId == roomId);
        
        if(room == null)
            throw new GenericApiError("Room doesn't exist");

        if (playerIndex == 1)
            room.player1Points++;
        else if (playerIndex == 2)
            room.player2Points++;
    }

    public static void DeleteRoom(string roomId)
    {
        var room = roomStates.FirstOrDefault(x => x.roomId == roomId);
        roomStates.Remove(room);
    }

    public static bool RoomWithIdExists(string roomId)
    {
        return roomStates.Any(x => x.roomId == roomId);
    }
}