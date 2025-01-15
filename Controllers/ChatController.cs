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
}