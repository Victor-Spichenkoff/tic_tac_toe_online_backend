namespace asp_rest_model.Models;

public class AllStatesResponse
{
    public required InGameState inGameState { get; set; }
    public RoomState roomState { get; set; }
}