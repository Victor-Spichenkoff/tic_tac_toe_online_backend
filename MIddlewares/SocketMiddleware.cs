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

            // Delegar para o serviço de sockets
            await _socketService.HandleWebSocketAsync(webSocket, roomId);
        }
        else
        {
            await _next(context);
        }
    }
}