using System;
using System.ComponentModel;
//DO NOT DELETE the two following using statements *********************************
using Game_Logic_Class;
using Object_Classes;


namespace Space_Race
{
    class Console_Class
    {
        /// <summary>
        /// Algorithm below currently plays only one game
        /// 
        /// when have this working correctly, add the abilty for the user to 
        /// play more than 1 game if they choose to do so.
        /// </summary>
        /// <param name="args"></param>
        private static bool end_game = false;
        private static int round = 0;
        static void Main(string[] args)
        {      
            DisplayIntroductionMessage();
            /*                    
             Set up the board in Board class (Board.SetUpBoard)
             Determine number of players - initally play with 2 for testing purposes 
             Create the required players in Game Logic class
              and initialize players for start of a game             
             loop  until game is finished           
                call PlayGame in Game Logic class to play one round
                Output each player's details at end of round
             end loop
             Determine if anyone has won
             Output each player's details at end of the game
           */
            while(!end_game){
                Console.Write("\nPress Enter to play a round...\n");
                PressEnter();
            }

        }//end Main

   
        /// <summary>
        /// Display a welcome message to the console
        /// Pre:    none.
        /// Post:   A welcome message is displayed to the console.
        /// </summary>
        static void DisplayIntroductionMessage()
        {
            Console.Clear();
            Console.WriteLine("\tWelcome to Space Race.\n");
            Console.WriteLine("\tThis game is for 2 to 6 players.");
            Console.Write("\tHow many players (2-6): ");
            SpaceRaceGame.NumberOfPlayers = InputCheck();
            Board.SetUpBoard();
            SpaceRaceGame.SetUpPlayers();
        } //end DisplayIntroductionMessage

        //Check the number of players press by the user 
        private static int InputCheck(){
            string num_player;
            bool choice;
            int option;
            do{
                num_player = Console.ReadLine();
                choice = int.TryParse(num_player,out option);
                if (!choice || option < SpaceRaceGame.MIN_PLAYERS || option > SpaceRaceGame.MAX_PLAYERS){
                    choice = false;
                    Console.Write("Error: Invalid number of players entered.\n");
                    Console.WriteLine("\tThis game is for 2 to 6 players.");
                    Console.Write("\tHow many players (2-6): ");
                }
            }while(!choice);
            return option;
        }
        /// <summary>
        /// Displays a prompt and waits for a keypress.
        /// Pre:  none
        /// Post: a key has been pressed.
        /// </summary>
        private static BindingList<Player> out_players = new BindingList<Player>(); //Store the array of players out of the game
        private static BindingList<Player> win_players = new BindingList<Player>(); //Store winning players 
        private static void PressEnter(){
            string key = Console.ReadKey().Key.ToString();
            if(key != "Enter"){
                Console.WriteLine("\nInvalid input.\n");
            }
            if(key == "Enter"){
                 SpaceRaceGame.PlayOneRound();
                 if(round==0){
                        Console.WriteLine("\tFirst Round\n");
                 }
                 else{
                    Console.WriteLine("\tNext Round\n");
                 }
                 round++;
                //Print to the console the information of each player at each round 
                 for(int i = 0; i < SpaceRaceGame.NumberOfPlayers; i++){
                    if(SpaceRaceGame.Players[i].Position >= 55 && SpaceRaceGame.Players[i].RocketFuel >= 0){
                        SpaceRaceGame.Players[i].Position = 55;
                        if (!win_players.Contains(SpaceRaceGame.Players[i]))
                        {
                            win_players.Add(SpaceRaceGame.Players[i]);
                        }
                    }
                    Console.WriteLine("\t{0} on square {1} with {2} yottawatt of power remanining\n",SpaceRaceGame.Players[i].Name,
                        SpaceRaceGame.Players[i].Position,SpaceRaceGame.Players[i].RocketFuel);

                 }
                 for(int i = 0; i < SpaceRaceGame.NumberOfPlayers; i++){
                       //Stop the game when any players reach the destination square
                       if(SpaceRaceGame.Players[i].Position == 55){
                            Console.Write("\tThe following player(s) finished the game\n\n");
                            EndGame();
                            Console.Write("\n\n\tPress Enter key to continue...\n\n\n");
                            PlayAgain();

                       }
                       //Remove the player whose fuel is 0 and position is less than 55
                       else if(SpaceRaceGame.Players[i].RocketFuel == 0 && SpaceRaceGame.Players[i].Position < 55){
                          out_players.Add(SpaceRaceGame.Players[i]);
                          SpaceRaceGame.Players.RemoveAt(i);
                          SpaceRaceGame.NumberOfPlayers--; 

                       }

                 }
                 //No players win 
                if (SpaceRaceGame.NumberOfPlayers == 0 || SpaceRaceGame.Players[0].RocketFuel == 0)
                {
                    Console.Write("\n\tNo One Wins");
                    end_game = true;
                    EndGame();
                    Console.Write("\n\n\tPress Enter key to continue...\n\n\n");
                    PlayAgain();

                }


            }
        } // end PressAny

        //Display the position of players when the game ended 
        private static void EndGame(){
            for(int b = 0; b < win_players.Count; b++)
            {
                Console.WriteLine("\t\t{0}", win_players[b].Name);
            }
            for(int a = 0; a < out_players.Count; a++){
                SpaceRaceGame.Players.Add(out_players[a]);
            }
            SpaceRaceGame.NumberOfPlayers+=out_players.Count;
            Console.Write("\n\t\tIndividual players finished with the at the locations specified");
            for(int i = 0; i < SpaceRaceGame.NumberOfPlayers;i++){
                Console.Write("\n\t\t{0} with {1} yattowatt of power at square {2}",SpaceRaceGame.Players[i].Name,
                    SpaceRaceGame.Players[i].RocketFuel,SpaceRaceGame.Players[i].Position);
            }
        }

        //Play again appear when a game ended
        private static void PlayAgain(){
            Console.Write("\n\tPlay Again? (Y or N): ");
            string reply = Console.ReadLine();
            if(reply == "Y" || reply == "y"){
                 round = 0;
                 win_players.Clear();
                 out_players.Clear();
                 SpaceRaceGame.Players.Clear();
                 DisplayIntroductionMessage();
                 end_game = false;
            }
            else
            {
                end_game = true;
                Console.Write("\n\tThank for playing Space Race.\n");
                Console.Write("\n\tPress Enter to terminate the program...");
                string key = Console.ReadKey().Key.ToString();
                if (key == "Enter")
                {
                    Environment.Exit(-1);
                }
            }

        }//end PlayAgain




    }//end Console class
}
