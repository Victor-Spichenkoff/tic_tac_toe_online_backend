using asp_rest_model.Sockets;

namespace asp_rest_model.Helpers;

public static class GeneralHelper
{
    /*
     * Is PAR
     */
    public static bool IsEven(int num) => num % 2 == 0;


    public static async Task RemoveConnectionAfter(int ms, string roomId)
    {
        await Task.Delay(ms); //não quebra o thread principal

        var roomInfo = RoomStateManager.GetRoomStateById(roomId);
        if (roomInfo == null)
            throw new GenericApiError("Room doesn't exists");

        if (!roomInfo.isPLayer1Connected && !roomInfo.isPLayer2Connected)
        {
            RoomManager.rooms.TryRemove(roomId, out _);
            InGameManager.DeleteInGameState(roomId);
            RoomStateManager.DeleteRoom(roomId);
            Console.WriteLine("Really remove for id: " + roomId);
        }
    }
}