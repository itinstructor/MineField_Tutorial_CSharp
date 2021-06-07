using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MineField
{
    public partial class frmMineField : Form
    {
        Label lblLabel;             // Create a global Label variable to track which label we are working with or have clicked
        int intMine;                // Where is the mine?
        int intClickCounter = 0;    // Track how many times we have clicked a square
        int intWIN = 10;            // How many squares we have to click to win
        int intWins = 0;            // How many wins do we have
        int intGames = 0;           // How many games have we played
        const int MIN = 1;          // Lower bounds of random number, is included
        const int MAX = 21;         // Upper bounds of random number, is excluded
        Random randomMine = new Random();   // Create a new random object

        public frmMineField()
        {
            InitializeComponent();
        }

        private void MineLabel_Click(object sender, EventArgs e)
        {
            intClickCounter += 1;       // Increase the click counter by 1 for each mine we click to track when we win
            lblLabel = (Label)sender;   // Use the label variable to hold the label that was just clicked

            if (intClickCounter == intWIN) // We clicked the WIN number of times, time to end this round
            {
                // We clicked the mine and lost
                if (lblLabel.TabIndex == intMine)
                {
                    // Blink the mine on and off
                    BlinkMine();
                    intGames += 1;
                    lblWins.Text = ("Wins: " + intWins + " out of " + intGames);
                    lblHitRate.Text = HitRateCalculation.HitRate(intWins, intGames);

                    // Ask the player if they want to play again
                    DialogResult ResponseDialogResult = MessageBox.Show("You lost. Play again?", "Play again?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (ResponseDialogResult == DialogResult.Yes)
                    {
                        lblLabel.Text = "";
                        ResetGame();
                    }
                    else
                    {
                        this.Close();   // Exit the game
                    }
                }
                // We win
                else
                {
                    lblLabel.BackColor = Color.White;   // Indicate the label has been clicked
                    intGames += 1;  // Increment the number of rounds played
                    intWins += 1;   // Increment the number of games won
                    lblWins.Text = ("Wins: " + intWins + " out of " + intGames);        // Display game stats
                    lblHitRate.Text = HitRateCalculation.HitRate(intWins, intGames);    // Display percentage of wins

                    // Ask the player if they want to play again
                    DialogResult ResponseDialogResult = MessageBox.Show("You won! Play again?", "Play again?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (ResponseDialogResult == DialogResult.Yes)
                    {
                        ResetGame();    // Reset the round
                    }
                    else
                    {
                        this.Close();   // Exit the game
                    }
                }
            }
            // Go here until we reach intWIN
            else
            {
                // User clicked the mine and lost
                if (lblLabel.TabIndex == intMine)
                {
                    BlinkMine();    // Blink the mine on and off
                    intGames += 1;  // Increment the number of rounds played
                    lblWins.Text = ("Wins: " + intWins + " out of " + intGames);        // Display game stats
                    lblHitRate.Text = HitRateCalculation.HitRate(intWins, intGames);    // Display percentage of wins

                    // Ask the player if they want to play again
                    DialogResult ResponseDialogResult = MessageBox.Show("You lost. Play again?", "Play again?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (ResponseDialogResult == DialogResult.Yes)
                    {
                        lblLabel.Text = ""; // Clear the label
                        ResetGame();        // Reset this round
                    }
                    else
                    {
                        this.Close();   // End the game
                    }

                }
                // User missed the mine and continues trying
                else
                {
                    lblLabel.BackColor = Color.White;   // Change the color of the mine to white to indicate that it has been clicked
                    lblLabel.Enabled = false;           // Disable the control, it can't be clicked again
                }
            }
        }

        // Reset everything for a new game
        private void NewGame()
        {
            // Reset all counters and labels
            intClickCounter = 0;
            intWins = 0;
            intGames = 0;
            lblWins.Text = "Wins: 0 out of 0";
            lblHitRate.Text = "0%";
            BlinkSquares();         // Blink the square to different colors to indicate that the game is being reset

            // Generate a random integer between 1 & 20 and assign it to the intMine variable
            // Basically, create a new random mine
            intMine = randomMine.Next(MIN, MAX);
            label21.Text = intMine.ToString();  // Display the mine number for program testing, comment out when done
        }

        // Reset the mine location and the game pieces
        private void ResetGame()
        {
            intClickCounter = 0;    // Reset the click counter to 0
            BlinkSquares();         // Blink the square to different colors to indicate that the game is being reset

            // Generate a random integer between 1 & 20 and assign it to the intMine variable
            // Basically, create a new random mine
            intMine = randomMine.Next(MIN, MAX);
            label21.Text = intMine.ToString();  // Display the mine number for program testing, comment out when done
        }

        // Blink the selected mine white and red to indicate losing
        private void BlinkMine()
        {
            int intSleep = 75;  // Variable for how many milliseconds to pause the program to allow the form to redraw
            // Change color of mine back and forth 6 times
            for (int i = 1; i < 6; i++)
            {
                lblLabel.BackColor = Color.White;           // Change mine to White
                this.Refresh();                             // Redraw the form to display change in mine color
                System.Threading.Thread.Sleep(intSleep);    // Pause program for indicated milliseconds to display new label color
                lblLabel.BackColor = Color.Red;             // Change mine to Red
                this.Refresh();                             // Redraw the form to display change in mine color
                System.Threading.Thread.Sleep(intSleep);    // Pause the program for indicated milliseconds to display new color
            }
            lblLabel.Text = "*Mine*";                       // Change text of mine to indicate hit
        }

        // Blink all the mines and reset color to White
        private void BlinkSquares()
        {
            int intSleep = 75;  // Variable for how many milliseconds to pause the program to allow the form to redraw

            // Go through all the controls/labels in the boardTableLayoutPanel
            foreach (Label lbl in boardTableLayoutPanel.Controls.OfType<Label>())
            {
                lbl.Enabled = true;                         // Enable labels
                lbl.BackColor = Color.White;                // Change label color
                this.Refresh();                             // Redraw the form to display change in mine color
                System.Threading.Thread.Sleep(intSleep);    // Pause program for indicated milliseconds to display new label color
                lbl.BackColor = Color.Blue;                 // Change label color back to original color
                this.Refresh();                             // Redraw the form to display change in mine color
                System.Threading.Thread.Sleep(intSleep);    // Pause program for indicated milliseconds to display new label color
            }
        }

        private void frmMineField_Load(object sender, EventArgs e)
        {
            // Generate a random integer betwen 1 & 20, assign it to the intMine variable when the form loads
            intMine = randomMine.Next(MIN, MAX);
            label21.Text = intMine.ToString();  // Display the mine number for program testing, comment out when done
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();   // Exit the program
        }

        private void tsmiNewGame_Click(object sender, EventArgs e)
        {
            NewGame();  // Start a new game
        }

        // About box describing the program
        private void AboutMineFieldToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Mine Field 1.0" + Environment.NewLine + "Written: 12/07/2010" + Environment.NewLine + Environment.NewLine + "Click 10 safe squares and miss the mine to win." + Environment.NewLine + "Use the Options menu to change game difficulty.", "Mine Field 1.0", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Set the difficulty of the game
        private void tsmiEasy5SquaresToWin_Click(object sender, EventArgs e)
        {
            intWIN = 5;     // Set the win variable to 5 mines
            // Set the appropriate check for the menu to indicate which is selected
            tsmiEasy5SquaresToWin.Checked = true;
            tsmiMedium10SquaresToWin.Checked = false;
            tsmiDifficult15SquaresToWin.Checked = false;
            NewGame();  // Start a new game
        }

        // Set the difficulty of the game
        private void tsmiMedium10SquaresToWin_Click(object sender, EventArgs e)
        {
            intWIN = 10;     // Set the win variable to 10 mines
            // Set the appropriate check for the menu to indicate which is selected
            tsmiEasy5SquaresToWin.Checked = false;
            tsmiMedium10SquaresToWin.Checked = true;
            tsmiDifficult15SquaresToWin.Checked = false;
            NewGame();  // Start a new game
        }

        // Set the difficulty of the game
        private void tsmiDifficult15SquaresToWin_Click(object sender, EventArgs e)
        {
            intWIN = 15;     // Set the win variable to 15 mines
            // Set the appropriate check for the menu to indicate which is selected
            tsmiEasy5SquaresToWin.Checked = false;
            tsmiMedium10SquaresToWin.Checked = false;
            tsmiDifficult15SquaresToWin.Checked = true;
            NewGame();  // Start a new game
        }
    }
}
