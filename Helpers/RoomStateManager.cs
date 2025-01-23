using asp_rest_model.Models;
using asp_rest_model.Sockets;

namespace asp_rest_model.Helpers;

public class RoomStateManager
{
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
        // TODO
        // não retorna o correto, só um objeto vazio
        return initialState;
    }


    public static RoomState? GetRoomStateById(string roomId)
    {
        return roomStates.FirstOrDefault(x => x.roomId == roomId);
    }

    public static bool UpdateRoom(RoomState newRoomState)
    {
        if(!RoomManager.RoomExists(newRoomState.roomId))
            return false;
        
        var index = roomStates.FindIndex(x => x.roomId == newRoomState.roomId);
        
        roomStates[index] = newRoomState;


        return true;
    }
}