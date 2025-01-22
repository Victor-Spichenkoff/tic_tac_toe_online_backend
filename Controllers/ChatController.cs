using asp_rest_model.Helpers;
using asp_rest_model.Models;
using asp_rest_model.Sockets;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using StateManager = asp_rest_model.Helpers.StateManager;

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

    [HttpPost("/create/{roomId}")]
    [ProducesResponseType(typeof(RoomCreationResponse), 200)]
    [ProducesResponseType(typeof(string), 400)]
    public IActionResult CreateRoom(string roomId)
    {
        // valid
        if(RoomManager.RoomExists(roomId))
            throw new GenericApiError("Room already exists, Connect to it!");
        
        if(string.IsNullOrWhiteSpace(roomId) || roomId == "0")
            throw new GenericApiError("Invalid room ID");
        
        if (RoomManager.RoomFull(roomId))
            throw new GenericApiError("Room is full");
        
        // creations
        RoomManager.CreateRoom(roomId);

        var roomStateInfos = RoomStateManager.AddNewRoom(roomId);
        
        var inGameState = InGameManager.GiveInitialInGameState();
        
        RoomCreationResponse response =  new ()
        {
            inGameState = inGameState, 
            roomState = roomStateInfos
        };
        
        if(!response.roomState.roomId.Equals(roomId))
            throw new GenericApiError("Different room IDs");
        
        return Ok(response);
    }
}