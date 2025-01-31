using System.Text.Json;
using asp_rest_model.Models;
using asp_rest_model.Services;

namespace asp_rest_model.MIddlewares;

public class SocketMiddleware(RequestDelegate next, SocketService socketService)
{
    private readonly RequestDelegate _next = next;
    private readonly SocketService _socketService = socketService;

    public async Task InvokeAsync(HttpContext context)
    {
        //receber quando tiver /ws ou for especificamente conexão socket
        if (context.Request.Path == "/ws" && context.WebSockets.IsWebSocketRequest)
        {
            var webSocket = await context.WebSockets.AcceptWebSocketAsync();
            Console.WriteLine("WebSocket connection established");
            
            //pegar o room id
            var roomId = context.Request.Query["roomId"]; // Capturar o roomId na query string
            if (string.IsNullOrEmpty(roomId))
            {
                context.Response.StatusCode = 400; // Responder com erro se roomId não for enviado
                await context.Response.WriteAsync("Missing roomId");
                return;
            }

            try
            {
                await _socketService.HandleWebSocketAsync(webSocket, roomId);

            }
            catch
            {
                context.Response.StatusCode = 400;
                var res = new SocketInGameResponse()
                {
                    errorMessage = "Room with problems, please create a new one",
                    isError = true
                };
                await context.Response.WriteAsync(JsonSerializer.Serialize(res));
                    await _next(context);
                // await context.Response.WriteAsync("Room with problems, please create a new one");
            }

            // Delegar para o serviço de sockets
        }
        else
        {
            await _next(context);
        }
    }
}