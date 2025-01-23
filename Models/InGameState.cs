﻿// ReSharper disable InconsistentNaming
namespace asp_rest_model.Models;

/*
 * * state -> cada square. 0-8
 */
public class InGameState
{
        public required string roomId { get; set; }
        public bool isFinished { get; set; }
        public bool player1Wins { get; set; }
        public bool player2Wins { get; set; }
        // public required int[] state { get; set; }
        public required SquareOptions[] state;
        public bool isPLayer1Turn { get; set; }
        public bool isPlayer2Turn { get; set; }
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