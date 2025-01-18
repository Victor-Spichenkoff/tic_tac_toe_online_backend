using System.Collections.Concurrent;
using System.Net.WebSockets;
using asp_rest_model.Sockets;

namespace asp_rest_model.Services;

using System.Net;
using System.Net.Sockets;
using System.Text;

// usando salas
public class SocketService
{

    //precisa pegar no middleware e passar para o handler
    public async Task HandleWebSocketAsync(WebSocket webSocket, string roomId)
    {
        // Adicionar o socket à sala correspondente
        //não existe == cria novo socket para aquela sala
        if (!RoomManager.rooms.ContainsKey(roomId))
        {
            RoomManager.CreateRoom(roomId);
        }

        RoomManager.rooms[roomId].Add(webSocket);


        var buffer = new byte[1024 * 4];
        while (webSocket.State == WebSocketState.Open)
        {
            var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            if (result.MessageType == WebSocketMessageType.Text)
            {
                var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                Console.WriteLine($"[Room: {roomId}] Received: {message}");

                // Retransmitir a mensagem para todos os WebSockets da sala
                await BroadcastMessageAsync(message, roomId);
            }
            else if (result.MessageType == WebSocketMessageType.Close)
            {
                Console.WriteLine("WebSocket closed.");
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by client",
                    CancellationToken.None);
            }
        }

        // remover aquela sala ao finalizar
        RoomManager.rooms[roomId].Remove(webSocket);

        // em caso de erro,
        if (RoomManager.rooms[roomId].Count == 0)
        {
            RoomManager.rooms.TryRemove(roomId, out _);
        }
    }


    // ligar com request de texto
    private async Task BroadcastMessageAsync(string message, string roomId)
    {
        //verificar se realmente existe
        if (RoomManager.rooms.ContainsKey(roomId))
        {
            var messageBytes = Encoding.UTF8.GetBytes(message);

            //passar para todos
            foreach (var socket in RoomManager.rooms[roomId])
            {
                if (socket.State == WebSocketState.Open)
                {
                    await socket.SendAsync(new ArraySegment<byte>(messageBytes), WebSocketMessageType.Text, true,
                        CancellationToken.None);
                }
            }
        }
    }
}


/*

public class SocketService
{
    //para usar salas
    private readonly ConcurrentDictionary<string, List<WebSocket>> _rooms = new();

    //precisa pegar no middleware e passar para o handler
    public async Task HandleWebSocketAsync(WebSocket webSocket, string roomId)
    {
        // Adicionar o socket à sala correspondente
        //não existe == cria novo socket para aquela sala
        if (!_rooms.ContainsKey(roomId))
        {
            RoomManager.CreateRoom(roomId);
            // _rooms[roomId] = new List<WebSocket>();
        }

        _rooms[roomId].Add(webSocket);


        var buffer = new byte[1024 * 4];
        while (webSocket.State == WebSocketState.Open)
        {
            var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            if (result.MessageType == WebSocketMessageType.Text)
            {
                var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                Console.WriteLine($"[Room: {roomId}] Received: {message}");

                // Retransmitir a mensagem para todos os WebSockets da sala
                await BroadcastMessageAsync(message, roomId);
            }
            else if (result.MessageType == WebSocketMessageType.Close)
            {
                Console.WriteLine("WebSocket closed.");
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by client",
                    CancellationToken.None);
            }
        }

        // remover aquela sala ao finalizar
        _rooms[roomId].Remove(webSocket);

        // em caso de erro,
        if (_rooms[roomId].Count == 0)
        {
            _rooms.TryRemove(roomId, out _);
        }
    }


    // ligar com request de texto
    private async Task BroadcastMessageAsync(string message, string roomId)
    {
        //verificar se realmente existe
        if (_rooms.ContainsKey(roomId))
        {
            var messageBytes = Encoding.UTF8.GetBytes(message);

            //passar para todos
            foreach (var socket in _rooms[roomId])
            {
                if (socket.State == WebSocketState.Open)
                {
                    await socket.SendAsync(new ArraySegment<byte>(messageBytes), WebSocketMessageType.Text, true,
                        CancellationToken.None);
                }
            }
        }
    }
}

*/