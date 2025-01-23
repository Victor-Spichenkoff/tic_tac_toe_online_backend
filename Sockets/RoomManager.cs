using System.Collections.Concurrent;
using System.Net.WebSockets;
using asp_rest_model.Helpers;
using asp_rest_model.Models;

namespace asp_rest_model.Sockets;

// vai ter as Conexões
public class RoomManager
{
    //TODO
    // TODO - Testar a criação de 2 salas + controller de isFull
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
        if (!rooms.ContainsKey(roomId))
            return false;

        return rooms[roomId].Count >= 2;
    }

    
}