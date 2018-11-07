using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Game_Logic_Class;
using Object_Classes;

namespace GUI_Class
{
    public partial class SpaceRaceForm : Form
    {
        // The numbers of rows and columns on the screen.
        const int NUM_OF_ROWS = 7;
        const int NUM_OF_COLUMNS = 8;

        // When we update what's on the screen, we show the movement of a player 
        // by removing them from their old square and adding them to their new square.
        // This enum makes it clear that we need to do both.
        enum TypeOfGuiUpdate { AddPlayer, RemovePlayer };


        public SpaceRaceForm()
        {
            InitializeComponent();
            Board.SetUpBoard();
            ResizeGUIGameBoard();
            SetUpGUIGameBoard();
            SetupPlayersDataGridView();
            DetermineNumberOfPlayers();
            SpaceRaceGame.SetUpPlayers();
            PrepareToPlay();
        }


        /// <summary>
        /// Handle the Exit button being clicked.
        /// Pre:  the Exit button is clicked.
        /// Post: the game is terminated immediately
        /// </summary>
        private void exitButton_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }


        /// <summary>
        /// Resizes the entire form, so that the individual squares have their correct size, 
        /// as specified by SquareControl.SQUARE_SIZE.  
        /// This method allows us to set the entire form's size to approximately correct value 
        /// when using Visual Studio's Designer, rather than having to get its size correct to the last pixel.
        /// Pre:  none.
        /// Post: the board has the correct size.
        /// </summary>
        private void ResizeGUIGameBoard()
        {
            const int SQUARE_SIZE = SquareControl.SQUARE_SIZE;
            int currentHeight = tableLayoutPanel.Size.Height;
            int currentWidth = tableLayoutPanel.Size.Width;
            int desiredHeight = SQUARE_SIZE * NUM_OF_ROWS;
            int desiredWidth = SQUARE_SIZE * NUM_OF_COLUMNS;
            int increaseInHeight = desiredHeight - currentHeight;
            int increaseInWidth = desiredWidth - currentWidth;
            this.Size += new Size(increaseInWidth, increaseInHeight);
            tableLayoutPanel.Size = new Size(desiredWidth, desiredHeight);

        }// ResizeGUIGameBoard


        /// <summary>
        /// Creates a SquareControl for each square and adds it to the appropriate square of the tableLayoutPanel.
        /// Pre:  none.
        /// Post: the tableLayoutPanel contains all the SquareControl objects for displaying the board.
        /// </summary>
        private void SetUpGUIGameBoard()
        {
            for (int squareNum = Board.START_SQUARE_NUMBER; squareNum <= Board.FINISH_SQUARE_NUMBER; squareNum++)
            {
                Square square = Board.Squares[squareNum];
                SquareControl squareControl = new SquareControl(square, SpaceRaceGame.Players);
                AddControlToTableLayoutPanel(squareControl, squareNum);
            }//endfor

        }// end SetupGameBoard

        private void AddControlToTableLayoutPanel(Control control, int squareNum)
        {
            int screenRow = 0;
            int screenCol = 0;
            MapSquareNumToScreenRowAndColumn(squareNum, out screenRow, out screenCol);
            tableLayoutPanel.Controls.Add(control, screenCol, screenRow);
        }// end Add Control


        /// <summary>
        /// For a given square number, tells you the corresponding row and column number
        /// on the TableLayoutPanel.
        /// Pre:  none.
        /// Post: returns the row and column numbers, via "out" parameters.
        /// </summary>
        /// <param name="squareNumber">The input square number.</param>
        /// <param name="rowNumber">The output row number.</param>
        /// <param name="columnNumber">The output column number.</param>
        private static void MapSquareNumToScreenRowAndColumn(int squareNum, out int screenRow, out int screenCol)
        {
            // Code needs to be added here to do the mapping

            // Makes the compiler happy - these two lines below need to deleted 
            //    once mapping code is written above
            screenRow = 6 - (int)squareNum / 8;
            if(screenRow%2 == 0)
            {
                screenCol = squareNum % 8;
            }
            else {
                screenCol = 7 - squareNum % 8;
            }
        }//end MapSquareNumToScreenRowAndColumn


        private void SetupPlayersDataGridView()
        {
            // Stop the playersDataGridView from using all Player columns.
            playersDataGridView.AutoGenerateColumns = false;
            // Tell the playersDataGridView what its real source of data is.
            playersDataGridView.DataSource = SpaceRaceGame.Players;

        }// end SetUpPlayersDataGridView


        /// <summary>
        /// Obtains the current "selected item" from the ComboBox
        ///  and
        ///  sets the NumberOfPlayers in the SpaceRaceGame class.
        ///  Pre: none
        ///  Post: NumberOfPlayers in SpaceRaceGame class has been updated
        /// </summary>
        private void DetermineNumberOfPlayers()
        {
            // Store the SelectedItem property of the ComboBox in a string
            string num_choice = numplayerBox.SelectedItem.ToString();
            // Parse string to a number
            int num = Convert.ToInt32(num_choice);
            // Set the NumberOfPlayers in the SpaceRaceGame class to that number
            SpaceRaceGame.NumberOfPlayers = num;
        }//end DetermineNumberOfPlayers

        /// <summary>
        /// The players' tokens are placed on the Start square
        /// </summary>
        /// 
        private static int count = 0; //Store the turn of player in single step mode
        private static int round_play = 0; //Store the round play of players in single step mode 
        private void PrepareToPlay()
        {
            // More code will be needed here to deal with restarting 
            // a game after the Reset button has been clicked. 
            //
            // Leave this method with the single statement 
            // until you can play a game through to the finish square
            // and you want to implement the Reset button event handler.
            //
            yesButton.Click += new EventHandler(YesNoButton_Click);
            noButton.Click += new EventHandler(YesNoButton_Click);
            rollButton.Click += new EventHandler(clickRollButton_Click);
            resetButton.Click += new EventHandler(resetButton_Click);
            UpdatePlayersGuiLocations(TypeOfGuiUpdate.AddPlayer);

        }//end PrepareToPlay()


        /// <summary>
        /// Tells you which SquareControl object is associated with a given square number.
        /// Pre:  a valid squareNumber is specified; and
        ///       the tableLayoutPanel is properly constructed.
        /// Post: the SquareControl object associated with the square number is returned.
        /// </summary>
        /// <param name="squareNumber">The square number.</param>
        /// <returns>Returns the SquareControl object associated with the square number.</returns>
        private SquareControl SquareControlAt(int squareNum)
        {
            int screenRow;
            int screenCol;
            
            MapSquareNumToScreenRowAndColumn(squareNum, out screenRow, out screenCol);
            return (SquareControl)tableLayoutPanel.GetControlFromPosition(screenCol, screenRow);
        }


        /// <summary>
        /// Tells you the current square number of a given player.
        /// Pre:  a valid playerNumber is specified.
        /// Post: the square number of the player is returned.
        /// </summary>
        /// <param name="playerNumber">The player number.</param>
        /// <returns>Returns the square number of the player.</returns>
        private int GetSquareNumberOfPlayer(int playerNumber)
        {
            // Code needs to be added here.

            //     delete the "return -1;" once body of method has been written 
           return SpaceRaceGame.Players[playerNumber].Position;
        }//end GetSquareNumberOfPlayer


        /// <summary>
        /// When the SquareControl objects are updated (when players move to a new square),
        /// the board's TableLayoutPanel is not updated immediately.  
        /// Each time that players move, this method must be called so that the board's TableLayoutPanel 
        /// is told to refresh what it is displaying.
        /// Pre:  none.
        /// Post: the board's TableLayoutPanel shows the latest information 
        ///       from the collection of SquareControl objects in the TableLayoutPanel.
        /// </summary>
        private void RefreshBoardTablePanelLayout()
        {
            tableLayoutPanel.Invalidate(true);
        }

        /// <summary>
        /// When the Player objects are updated (location, etc),
        /// the players DataGridView is not updated immediately.  
        /// Each time that those player objects are updated, this method must be called 
        /// so that the players DataGridView is told to refresh what it is displaying.
        /// Pre:  none.
        /// Post: the players DataGridView shows the latest information 
        ///       from the collection of Player objects in the SpaceRaceGame.
        /// </summary>
        private void UpdatesPlayersDataGridView()
        {
            SpaceRaceGame.Players.ResetBindings();
        }

        /// <summary>
        /// At several places in the program's code, it is necessary to update the GUI board,
        /// so that player's tokens are removed from their old squares
        /// or added to their new squares. E.g. at the end of a round of play or 
        /// when the Reset button has been clicked.
        /// 
        /// Moving all players from their old to their new squares requires this method to be called twice: 
        /// once with the parameter typeOfGuiUpdate set to RemovePlayer, and once with it set to AddPlayer.
        /// In between those two calls, the players locations must be changed. 
        /// Otherwise, you won't see any change on the screen.
        /// 
        /// Pre:  the Players objects in the SpaceRaceGame have each players' current locations
        /// Post: the GUI board is updated to match 
        /// </summary>
        private void UpdatePlayersGuiLocations(TypeOfGuiUpdate typeOfGuiUpdate)
        {
            // Code needs to be added here which does the following:
            //
            //   for each player
            //       determine the square number of the player
            //       retrieve the SquareControl object with that square number
            //       using the typeOfGuiUpdate, update the appropriate element of  
            //          the ContainsPlayers array of the SquareControl object.
            //          
            for (int i = 0; i < SpaceRaceGame.NumberOfPlayers; i++)
            {
                int num_square = GetSquareNumberOfPlayer(i);
                SquareControl square = SquareControlAt(num_square);
                if (typeOfGuiUpdate == 0)
                {
                    square.ContainsPlayers[i] = true;
                }
                else
                {
                    square.ContainsPlayers[i] = false;
                }

            }
            RefreshBoardTablePanelLayout();//must be the last line in this method. Do not put inside above loop.
        } //end UpdatePlayersGuiLocations
        private static int num_death_player = 0;
        private static BindingList<Player> players_win = new BindingList<Player>(); //Store winning players 

        //Display the message box when the game ends 
        private void message_end_game()
        {
            string a = "";
            for(int i = 0; i < players_win.Count; i++)
            {
                a += (players_win[i].Name + "\n");
            }
            MessageBox.Show("The following player(s) finished the game\n " + a);
            rollButton.Enabled = false;
            numplayerBox.Enabled = false;
            resetButton.Enabled = true;
            stepGroupBox.Enabled = false;
        }

        //Click the Roll Button 
        private void clickRollButton_Click(Object sender, EventArgs e)
        {
            numplayerBox.Enabled = false;
            //Select Single Step Mode 
            if (noButton.Checked == true)
            {
                stepGroupBox.Enabled = false;
                UpdatePlayersGuiLocations(TypeOfGuiUpdate.RemovePlayer);
                SpaceRaceGame.PlayOneRound();
                round_play++;
                if (round_play > 0)
                {
                    //Exit and Reset Buttons are enabled at the end of any round
                    exitButton.Enabled = true;
                    resetButton.Enabled = true;
                }
                //Update the position and fuel on the DataGridView 
                for (int i = 0; i < SpaceRaceGame.NumberOfPlayers; i++)
                {
                    playersDataGridView.Rows[i].Cells[2].Value = SpaceRaceGame.Players[i].Position;
                    playersDataGridView.Rows[i].Cells[3].Value = SpaceRaceGame.Players[i].RocketFuel;
                    if (SpaceRaceGame.Players[i].Position >= 55 && SpaceRaceGame.Players[i].RocketFuel > 0)
                    {
                        if (!players_win.Contains(SpaceRaceGame.Players[i]))
                        {
                            players_win.Add(SpaceRaceGame.Players[i]);
                        }

                    }
                    else if (SpaceRaceGame.Players[i].Position < 55 && SpaceRaceGame.Players[i].RocketFuel == 0) 
                    {
                        SpaceRaceGame.Players.RemoveAt(i); 
                        SpaceRaceGame.NumberOfPlayers--;
                        num_death_player++;
                    }
                }
                //Display the message box for the game winner(s)
                if (players_win.Count >= 1)
                {
                    message_end_game();
                }

                //All players run out of fuel before reaching the final square - Square 
                if(num_death_player == SpaceRaceGame.NumberOfPlayers)
                {
                    MessageBox.Show("No One Wins \n All Players Run Out Of Fuels");
                    rollButton.Enabled = false;
                    numplayerBox.Enabled = false;
                    resetButton.Enabled = true;
                    stepGroupBox.Enabled = false;
                }

                //Update DataGridView 
                UpdatesPlayersDataGridView();
                UpdatePlayersGuiLocations(TypeOfGuiUpdate.AddPlayer);
            }
            //Select Not Single Step Mode
            else if (yesButton.Checked == true)
            {
                resetButton.Enabled = false;
                stepGroupBox.Enabled = false;
                exitButton.Enabled = false;
                UpdatePlayersGuiLocations(TypeOfGuiUpdate.RemovePlayer);
                //Play Single Step For Each Person
                SpaceRaceGame.PlaySingle(SpaceRaceGame.Players[count].Name); 
                //Update the position and fuel on the DataGridView 
                playersDataGridView.Rows[count].Cells[2].Value = SpaceRaceGame.Players[count].Position; 
                playersDataGridView.Rows[count].Cells[3].Value = SpaceRaceGame.Players[count].RocketFuel;
                //Add game winners into the list players_win 
                if (SpaceRaceGame.Players[count].Position >= 55 && SpaceRaceGame.Players[count].RocketFuel > 0)
                {
                    if (!players_win.Contains(SpaceRaceGame.Players[count]))
                    {
                        players_win.Add(SpaceRaceGame.Players[count]);
                    }
                }
                //Increment the number of death players and reset the count if any players run out of fuels 
                //before reaching the final square 
                else if (SpaceRaceGame.Players[count].Position < 55 && SpaceRaceGame.Players[count].RocketFuel == 0)
                {
                        SpaceRaceGame.NumberOfPlayers--;
                        num_death_player++;
                        if(count == SpaceRaceGame.NumberOfPlayers)
                        {
                            count = 0;
                        }
                }
                if(players_win.Count >= 1)
                {
                    message_end_game();
                }
                //Increment the turn in each single step 
                count++; 
                //If the count of clicking the roll button is equal to the number of players, reset
                //and increment the number of rounds
                if (count == SpaceRaceGame.NumberOfPlayers)
                {
                    round_play++;
                    if (round_play > 0)
                    {
                        //Exit and Reset Buttons are enabled at the end of any round
                        exitButton.Enabled = true;
                        resetButton.Enabled = true;
                    }
                    count = 0;
                }
                //If All Players Run Out Of Fuels - End Game 
                if (SpaceRaceGame.NumberOfPlayers == 0)
                {
                    MessageBox.Show("No One Wins \n All Players Run Out Of Fuels");
                    rollButton.Enabled = false;
                    numplayerBox.Enabled = false;
                    resetButton.Enabled = true;
                    stepGroupBox.Enabled = false;
                }
                //Update DataGridView 
                UpdatesPlayersDataGridView();
                UpdatePlayersGuiLocations(TypeOfGuiUpdate.AddPlayer);

                
            }
            
        }
        //Enable Roll Button When either Radio Button is clicked 
        private void YesNoButton_Click(object sender, EventArgs e)
        {
            if(yesButton.Checked || noButton.Checked)
            {
                rollButton.Enabled = true;
            }
        }

        //Select the number of player in the combo box
        private void numplayerBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdatePlayersGuiLocations(TypeOfGuiUpdate.RemovePlayer);
            SpaceRaceGame.Players.Clear();
            DetermineNumberOfPlayers();
            SpaceRaceGame.SetUpPlayers();
            UpdatePlayersGuiLocations(TypeOfGuiUpdate.AddPlayer);
            UpdatesPlayersDataGridView();
        }

        //Click the reset Button 
        private void resetButton_Click(Object sender, EventArgs e)
        {
            numplayerBox.SelectedIndex = 4;
            UpdatePlayersGuiLocations(TypeOfGuiUpdate.RemovePlayer);
            SpaceRaceGame.Players.Clear();
            players_win.Clear();
            SpaceRaceGame.NumberOfPlayers = 6;
            SpaceRaceGame.SetUpPlayers();
            UpdatePlayersGuiLocations(TypeOfGuiUpdate.AddPlayer);
            UpdatesPlayersDataGridView();
            rollButton.Enabled = true;
            stepGroupBox.Enabled = true;
            numplayerBox.Enabled = true;
            yesButton.Checked = false;
            noButton.Checked = false;
            num_death_player = 0;
            count = 0;
            round_play = 0;
        }
    }// end class
}
