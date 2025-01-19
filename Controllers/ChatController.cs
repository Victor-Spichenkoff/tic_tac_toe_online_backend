using asp_rest_model.Sockets;

namespace asp_rest_model.Controllers;


using Microsoft.AspNetCore.Mvc;
using asp_rest_model.Services;

[ApiController]
[Route("[controller]")]
public class ChatController : ControllerBase
{
    private readonly SocketService _socketService;

    public ChatController(SocketService socketService)
    {
        _socketService = socketService;
    }

    [HttpGet("status")]
    public IActionResult GetStatus()
    {
        return Ok(new { status = "Socket server is running." });
    }


    [HttpGet("/room/exists/{roomId}")]
    [ProducesResponseType(typeof(bool), 200)]
    public IActionResult GetRoom(string roomId)
    {
        var exists = RoomManager.RoomExists(roomId);

        return Ok(exists);
    }
    
    
    [HttpGet("/room/isFull/{roomId}")]
    [ProducesResponseType(typeof(bool), 200)]
    public IActionResult GetRoomFullState(string roomId)
    {
        var isFull = RoomManager.RoomFull(roomId);

        return Ok(isFull);
    }
}