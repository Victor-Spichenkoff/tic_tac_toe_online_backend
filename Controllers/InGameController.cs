using asp_rest_model.Helpers;
using asp_rest_model.Models;
using asp_rest_model.Sockets;
using Microsoft.AspNetCore.Mvc;

namespace asp_rest_model.Controllers;

[ApiController]
[Route("inGame")]
public class InGameController: ControllerBase
{
    [HttpGet("/infos/{roomId}")]
    [ProducesResponseType(typeof(AllStatesResponse), 200)]
    [ProducesResponseType(typeof(string), 404)]
    public ActionResult GetGameInfos(string roomId)
    {
        if(!RoomManager.RoomExists(roomId))
            throw new GenericApiError("Room doesn't exist", 404);

        var inGameState = InGameManager.GetInGameStateById(roomId);
        var roomState = RoomStateManager.GetRoomStateById(roomId);
        
        AllStatesResponse response = new ()
        {
            inGameState = inGameState,
            roomState = roomState
        };

        return Ok(response);
    }


    [HttpPost("/disconnect/{playerIndex}/{roomId}")]
    public IActionResult DeleteOnDesconection(int playerIndex, string roomId)
    {
        var roomInfo = RoomStateManager.GetRoomStateById(roomId);
        var inGameInfo = InGameManager.GetInGameStateById(roomId);
        
        if(inGameInfo == null || roomInfo == null)
            throw new GenericApiError("Room doesn't exists");
        
        if(playerIndex == 1)
            roomInfo.isPLayer1Connected = false;
        else if(playerIndex == 2)
            roomInfo.isPLayer2Connected = false;
        
        RoomStateManager.UpdateRoom(roomInfo);

        Console.WriteLine($"Desligando: {playerIndex} em ID - {roomId}");
        
        return Ok(true);
    }
}