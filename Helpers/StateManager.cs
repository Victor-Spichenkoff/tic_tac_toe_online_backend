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
    public static int[] GiveResetState()=>
    // public static SquareOptions[] GiveResetState()=>
        [
            0, 0, 0,
            0, 0, 0,
            0, 0, 0
        ];

    public static CheckWinResponse CheckForWinner()
    {
        return CheckWinResponse.NoWinner;
    }
    
    
    public static int[] ChangeOnePosition (int choosePosition, int playerIndex, int[] currentState)
    {
        if(currentState[choosePosition] != 0)
            throw new GenericApiError("Position already taken");
        
        // todo
        // verificar vitória aqui, criar nova função
        
        
        currentState[choosePosition] = playerIndex;
        
        Console.WriteLine("NEw state: ", currentState.ToString());
        
        return currentState;
    }
}