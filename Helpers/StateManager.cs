using asp_rest_model.Models;

namespace asp_rest_model.Helpers;

public enum CheckWinResponse
{
    NoWinner = 0,
    WinX = 1,
    WinO = 2,
    Draw = 3
}


public static class StateManager
{
    public static SquareOptions[] GiveResetState()=>
        [
            0, 0, 0,
            0, 0, 0,
            0, 0, 0
        ];

    public static CheckWinResponse CheckForWinner()
    {
        return CheckWinResponse.NoWinner;
    }
}