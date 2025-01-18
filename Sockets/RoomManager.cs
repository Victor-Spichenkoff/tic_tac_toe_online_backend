using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace asp_rest_model.Sockets;

public class RoomManager
{
    public static readonly ConcurrentDictionary<string, List<WebSocket>> rooms = new();

    public static void CreateRoom(string roomId)
    {
        rooms[roomId] = new List<WebSocket>();
    }

    public static bool RoomExists(string roomId)
    {
        return rooms.ContainsKey(roomId);
    }
    
    public static bool RoomFull(string roomId)
    {
        return rooms[roomId].Count > 2;
    }
}