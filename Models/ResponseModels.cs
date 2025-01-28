namespace asp_rest_model.Models;

public class AllStatesResponse
{
    public required InGameState inGameState { get; set; }
    public RoomState roomState { get; set; }
}


public class AllStateResponseWithPLayerIndex : AllStatesResponse
{
    public required int playerIndex { get; set; } 
}

public class SocketInGameResponse
{
    public required bool isError { get; set; }
    public InGameState? inGameState { get; set; }
    public string? errorMessage { get; set; }
}