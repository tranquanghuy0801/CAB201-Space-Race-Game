using System.Diagnostics;
using System.Linq;

namespace Object_Classes {
    /// <summary>
    /// Models a game board for Space Race consisting of three different types of squares
    /// 
    /// Ordinary squares, Wormhole squares and Blackhole squares.
    /// 
    /// landing on a Wormhole or Blackhole square at the end of a player's move 
    /// results in the player moving to another square
    /// 
    /// </summary>
    public static class Board {
        /// <summary>
        /// Models a game board for Space Race consisting of three different types of squares
        /// 
        /// Ordinary squares, Wormhole squares and Blackhole squares.
        /// 
        /// landing on a Wormhole or Blackhole square at the end of a player's move 
        /// results in the player moving to another square
        /// 
        /// 
        /// </summary>

        public const int NUMBER_OF_SQUARES = 56;
        public const int START_SQUARE_NUMBER = 0;
        public const int FINISH_SQUARE_NUMBER = NUMBER_OF_SQUARES - 1;

        private static Square[] squares = new Square[NUMBER_OF_SQUARES];

        public static Square[] Squares {
            get {
                Debug.Assert(squares != null, "squares != null",
                   "The game board has not been instantiated");
                return squares;
            }
        }

        public static Square StartSquare {
            get {
                return squares[START_SQUARE_NUMBER];
            }
        }


        /// <summary>
        ///  Eight Wormhole squares.
        ///  
        /// Each row represents a Wormhole square number, the square to jump forward to and the amount of fuel consumed in that jump.
        /// 
        /// For example {2, 22, 10} is a Wormhole on square 2, jumping to square 22 and using 10 units of fuel
        /// 
        /// </summary>
        private static int[,] wormHoles =
        {
            {2, 22, 10},
            {3, 9, 3},
            {5, 17, 6},
            {12, 24, 6},
            {16, 47, 15},
            {29, 38, 4},
            {40, 51, 5},
            {45, 54, 4}
        };

        /// <summary>
        ///  Eight Blackhole squares.
        ///  
        /// Each row represents a Blackhole square number, the square to jump back to and the amount of fuel consumed in that jump.
        /// 
        /// For example {10, 4, 6} is a Blackhole on square 10, jumping to square 4 and using 6 units of fuel
        /// 
        /// </summary>
        private static int[,] blackHoles =
        {
            {10, 4, 6},
            {26, 8, 18},
            {30, 19, 11},
            {35,11, 24},
            {36, 34, 2},
            {49, 13, 36},
            {52, 41, 11},
            {53, 42, 11}
        };

        private static int[] blackHole_square = { 10, 26, 30, 35, 36, 49, 52, 53 }; //Square of Black Holes 
        private static int[] wormHole_square = { 2, 3, 5, 12, 16, 29, 40, 45 };  //Square of Worm Holes 
        /// <summary>
        /// Parameterless Constructor
        /// Initialises a board consisting of a mix of Ordinary Squares,
        ///     Wormhole Squares and Blackhole Squares.
        /// 
        /// Pre:  none
        /// Post: board is constructed
        /// </summary>
        public static void SetUpBoard() {

            // Create the 'start' square where all players will start.
            squares[START_SQUARE_NUMBER] = new Square("Start", START_SQUARE_NUMBER);

            for (int b = 1; b < NUMBER_OF_SQUARES - 1; b++)
            {
                if (!blackHole_square.Contains(b) || !wormHole_square.Contains(b))
                {
                    squares[b] = new Square(b.ToString(), b);

                }
            }
            //Create the BlackHoles Square
            for (int i = 0; i < blackHoles.GetLength(0); i++)
            {
                squares[blackHoles[i, 0]] = new BlackholeSquare(blackHoles[i, 0].ToString(), blackHoles[i, 0], blackHoles[i, 1], blackHoles[i, 2]);
            }
            //Create the WormHoles Square 
            for (int a = 0; a < wormHoles.GetLength(0); a++)
            {
                squares[wormHoles[a, 0]] = new WormholeSquare(wormHoles[a, 0].ToString(), wormHoles[a, 0], wormHoles[a, 1], wormHoles[a, 2]);
            }

            // Create the 'finish' square.
            squares[FINISH_SQUARE_NUMBER] = new Square("Finish", FINISH_SQUARE_NUMBER);
        } // end SetUpBoard

        /// <summary>
        /// Finds the destination square and the amount of fuel used for either a 
        /// Wormhole or Blackhole Square.
        /// 
        /// pre: squareNum is either a Wormhole or Blackhole square number
        /// post: destNum and amount are assigned correct values.
        /// </summary>
        /// <param name="holes">a 2D array representing either the Wormholes or Blackholes squares information</param>
        /// <param name="squareNum"> a square number of either a Wormhole or Blackhole square</param>
        /// <param name="destNum"> destination square's number</param>
        /// <param name="amount"> amont of fuel used to jump to the deestination square</param>
        private static void FindDestSquare(int[,] holes, int squareNum, out int destNum, out int amount) {
            destNum = 0; amount = 0;
            for(int i = 0; i < holes.GetLength(0);i++){
                if(holes[i,0]==squareNum){
                    destNum = holes[i,1];
                    amount = holes[i,2];
                }
            }
            //  CODE NEEDS TO BE ADDED HERE 

        } //end FindDestSquare

    } //end class Board
}