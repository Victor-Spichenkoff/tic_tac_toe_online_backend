using asp_rest_model.Models;

namespace asp_rest_model.Helpers;

public class RoomStateManager
{
    public static List<RoomState> roomStates = new List<RoomState>();

    public static RoomState AddNewRoom(string roomId)
    {
        // TODO
        // não retorna o correto, só um objeto vazio
        return new RoomState
        {
            roomId = roomId,
            drawsCount = 0,
            player1Points = 0,
            player2Points = 0,
            isPLayer1Connected = true,
            isPLayer2Connected = false
        };
    }


    public static List<RoomState> GetRoomStates() => roomStates;
    
    
}