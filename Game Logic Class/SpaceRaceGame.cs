using System;
using System.Drawing;
using System.ComponentModel;
using Object_Classes;


namespace Game_Logic_Class
{
    public static class SpaceRaceGame
    {
        // Minimum and maximum number of players.
        public const int MIN_PLAYERS = 2;
        public const int MAX_PLAYERS = 6;
   
        private static int numberOfPlayers;  //default value for test purposes only 
        public static int NumberOfPlayers
        {
            get
            {
                return numberOfPlayers;
            }
            set
            {
                numberOfPlayers = value;
            }
        }

        public static string[] names = { "One", "Two", "Three", "Four", "Five", "Six" };  // default values
        
        // Only used in Part B - GUI Implementation, the colours of each player's token
        private static Brush[] playerTokenColours = new Brush[MAX_PLAYERS] { Brushes.Yellow, Brushes.Red,
                                                                       Brushes.Orange, Brushes.White,
                                                                      Brushes.Green, Brushes.DarkViolet};
        /// <summary>
        /// A BindingList is like an array which grows as elements are added to it.
        /// </summary>
        private static BindingList<Player> players = new BindingList<Player>();
        public static BindingList<Player> Players
        {
            get
            {
                return players;
            }
        }

        // The pair of die
        private static Die die1 = new Die(), die2 = new Die();
       

        /// <summary>
        /// Set up the conditions for this game as well as
        ///   creating the required number of players, adding each player 
        ///   to the Binding List and initialize the player's instance variables
        ///   except for playerTokenColour and playerTokenImage in Console implementation.
        ///   
        ///     
        /// Pre:  none
        /// Post:  required number of players have been initialsed for start of a game.
        /// </summary>
        public static void SetUpPlayers() 
        {
            // for number of players
            //      create a new player object
            //      initialize player's instance variables for start of a game
            //      add player to the binding list
            for(int i = 0; i < NumberOfPlayers; i++){
                Player player = new Player(names[i]);
                player.AtFinish = false;
                player.HasPower = true;
                player.Position = 0;
                player.RocketFuel = Player.INITIAL_FUEL_AMOUNT;
                player.PlayerTokenColour = playerTokenColours[i];
                players.Add(player);
            }
        }

        /// <summary>
        ///  Plays one round of a game
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
        private static int[] wormHole_square = { 2, 3, 5, 12, 16, 29, 40, 45 }; //Square of Worm Holes 
 
        //Find the index of in either blackHoles or wormHoles to access the destination and the amount of fuel to use
        private static int Index_Square(int square, int[] holes)
        {
            for (int i = 0; i < holes.Length; i++)
            {
                if (holes[i] == square)
                {
                    return i;
                }
            }
            return -1;
        }

        //Play One Round For Al Selected Players
        public static void PlayOneRound()
        {

            for (int i = 0; i < NumberOfPlayers; i++)
            {

                players[i].Play(die1, die2);
                if (Check_Collision_Holes(players[i].Position, wormHoles)) //Step in WormHoles
                {
                    int a = Index_Square(players[i].Position, wormHole_square);
                    players[i].Position = wormHoles[a, 1];
                    players[i].ConsumeFuel(wormHoles[a, 2]);

                }
                else if (Check_Collision_Holes(players[i].Position, blackHoles)) //Step in BlackHoles 
                {
                    int b = Index_Square(players[i].Position, blackHole_square);
                    players[i].Position = blackHoles[b, 1];
                    players[i].ConsumeFuel(blackHoles[b, 2]);
                }
                else
                {
                    players[i].ConsumeFuel(2);
                }
            }
        }

        //Checking if the player steps in either WormHoles Square or BlackHoles Square 
        private static bool Check_Collision_Holes(int position, int[,] holes)
        {
            for (int i = 0; i < holes.GetLength(0); i++)
            {
                if (holes[i, 0] == position)
                {
                    return true;
                }
            }
            return false;
        }

        //Play Single Step in GUI Mode For Each Player if the name is satisfied
        public static void PlaySingle(string name)
        {
            for (int i = 0; i < NumberOfPlayers; i++)
            {
                if (players[i].Name == name)
                {
                    players[i].Play(die1, die2);
                    if (Check_Collision_Holes(players[i].Position, wormHoles))
                    {
                        int a = Index_Square(players[i].Position, wormHole_square); //Step in WormHoles
                        players[i].Position = wormHoles[a, 1];
                        players[i].ConsumeFuel(wormHoles[a, 2]);

                    }
                    else if (Check_Collision_Holes(players[i].Position, blackHoles)) //Step in BlackHoles 
                    {
                        int b = Index_Square(players[i].Position, blackHole_square);
                        players[i].Position = blackHoles[b, 1];
                        players[i].ConsumeFuel(blackHoles[b, 2]);
                    }
                    else
                    {
                        players[i].ConsumeFuel(2);
                    }

                }
                else
                {

                }
            }
        }
    }//end SnakesAndLadders
}