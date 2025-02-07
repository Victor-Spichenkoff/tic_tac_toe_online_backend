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
    [ProducesResponseType(typeof(AllStatesResponse), 200)]
    [ProducesResponseType(typeof(string), 400)]
    public IActionResult CreateRoom(string roomId)
    {
        // valid
        if (roomId.Contains(' '))
            throw new GenericApiError("Spaces are not allowed!");

        if (RoomManager.RoomExists(roomId) || RoomStateManager.RoomWithIdExists(roomId))
            throw new GenericApiError("Room already exists, Connect to it!");

        if (string.IsNullOrWhiteSpace(roomId) || roomId == "0")
            throw new GenericApiError("Invalid room ID");

        if (RoomManager.RoomFull(roomId))
            throw new GenericApiError("Room is full");

        // creations
        RoomManager.CreateRoom(roomId);

        var roomStateInfos = new RoomState() { roomId = roomId }; //RoomStateManager.AddNewRoom(roomId);

        var inGameStateInfos = InGameManager.GiveInitialInGameState(roomId);

        //cadastrando
        InGameManager.AddNewInGameState(inGameStateInfos);
        RoomStateManager.AddNewRoom(roomId); // cria com o default state


        AllStatesResponse response = new()
        {
            inGameState = inGameStateInfos,
            roomState = roomStateInfos
        };

        return Ok(response);
    }


    [HttpPost("/join/{roomId}")]
    [ProducesResponseType(typeof(AllStatesResponse), 200)]
    [ProducesResponseType(typeof(string), 400)]
    public IActionResult JoinRoom(string roomId)
    {
        // já conectado mesmo; Talvez precise de uma verificação para InGame e RoomState
        if (!RoomManager.RoomExists(roomId))
            throw new GenericApiError("Room doesn't exists");

        // decomentar em caso de erro aqui
        // if (RoomManager.RoomFull(roomId))
        //     throw new GenericApiError("Room Full");


        var newRoomInfos = RoomStateManager.GetRoomStateById(roomId);

        if (newRoomInfos == null)
            throw new GenericApiError("Room doesn't exists");


        var playerIndex = 2;

        if (newRoomInfos.isPLayer1Connected == false)
        {
            playerIndex = 1;
            newRoomInfos.isPLayer1Connected = true;
        }
        else if (newRoomInfos.isPLayer2Connected == false)
            newRoomInfos.isPLayer2Connected = true;
        else
            throw new GenericApiError("Both players are connected!");

        var success = RoomStateManager.UpdateRoom(newRoomInfos);

        if (!success)
            throw new GenericApiError("Failed to update room");

        RoomStateManager.UpdateRoom(newRoomInfos);
        
        var inGameStateInfos = InGameManager.GetInGameStateById(roomId);

        var response = new AllStateResponseWithPLayerIndex()
        {
            inGameState = inGameStateInfos,
            roomState = newRoomInfos,
            playerIndex = playerIndex
        };

        return Ok(response);
    }
}