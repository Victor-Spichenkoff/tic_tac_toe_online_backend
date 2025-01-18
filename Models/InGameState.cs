// ReSharper disable InconsistentNaming
namespace asp_rest_model.Models;

/*
 * * state -> cada square. 0-8
 */
public class InGameState
{
        public bool isFinished;
        public bool player1Wins;
        public bool player2Wins;
        public required SquareOptions[] state;
        public bool isPLayer1Turn;
        public bool isPlayer2Turn;
}


public class ReciveInfosObject {
        PlayerIndex playerIndex;
        private int choosePosition;
}


public enum SquareOptions
{
        Empty = 0,
        Cross = 1,
        Circle = 2,
}


public enum PlayerIndex
{
        Player1 = 1,
        Player2 = 2,
}