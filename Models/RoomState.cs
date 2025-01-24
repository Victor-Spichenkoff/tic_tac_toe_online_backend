namespace asp_rest_model.Models;

public class RoomState()
{
    public string roomId { get; set; }
    public bool isPLayer1Connected { get; set; } = false;
    public bool isPLayer2Connected { get; set; } = false;
    public int player1Points { get; set; } = 0;
    public int player2Points { get; set; } = 0;
    public int drawsCount { get; set; } = 0;
}