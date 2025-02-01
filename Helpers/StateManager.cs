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
            1, 2, 0,
            1, 2, 0,
            0, 0, 0
        ];
    // [
    //         0, 0, 0,
    //         0, 0, 0,
    //         0, 0, 0
    //     ];

    public static CheckWinResponse CheckForWinner()
    {
        return CheckWinResponse.NoWinner;
    }
    
    
    public static int[] ChangeOnePosition (int choosePosition, int playerIndex, int[] currentState)
    {
        if(currentState[choosePosition] != 0)
            throw new GenericApiError("Position already taken");
        
        currentState[choosePosition] = playerIndex;
        
        return currentState;
    }

    public static bool CheckWinForPlayer(int playerIndex, int[] state)
    {
        // Verificar linhas
        for (int i = 0; i < 3; i++)
        {
            if (state[i * 3] == playerIndex && state[i * 3 + 1] == playerIndex && state[i * 3 + 2] == playerIndex)
            {
                return true;
            }
        }

        // Verificar colunas
        for (int i = 0; i < 3; i++)
        {
            if (state[i] == playerIndex && state[i + 3] == playerIndex && state[i + 6] == playerIndex)
            {
                return true;
            }
        }

        // Verificar diagonal principal (de)
        if (state[0] == playerIndex && state[4] == playerIndex && state[8] == playerIndex)
        {
            return true;
        }

        // Verificar diagonal secundária (ed)
        if (state[2] == playerIndex && state[4] == playerIndex && state[6] == playerIndex)
        {
            return true;
        }

        return false;
    }

    public static bool CheckForDraw(int[] state)
    {
        int filledCount = 0;

        foreach (int square in state)
            if(square != 0)
                filledCount++;
        
        if(filledCount < 9)
            return false;
        
        
        var isPlayer1Wins = CheckWinForPlayer(1, state);
        if (isPlayer1Wins)
            return false;
        
        var isPlayer2Wins = CheckWinForPlayer(2, state);
        if(isPlayer2Wins)
            return false;
        
        return true;
    }
}