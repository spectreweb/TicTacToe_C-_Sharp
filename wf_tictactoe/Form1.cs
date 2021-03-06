﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/*Created by Akash Khosla 
 * 
 *  Used tutorials from Chris Merritt on Youtube.
 *  Includes AI and easy to use GUI!
 *  
 * 
 * 
 */

namespace wf_tictactoe
{
    public partial class Form1 : Form
    {
        bool turn = true; //true = X's turn; false = Y's turn;
        bool computerActive = false;
        int turn_count = 0;
        static String player1, player2; 

        public Form1()
        {
            InitializeComponent();
        }

        public static void setPlayerNames(String name1, String name2)
        {
            player1 = name1;
            player2 = name2;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            f2.ShowDialog();
            label1.Text = player1;
            label3.Text = player2; 


        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("By Akash Khosla, if you want to play against a computer, type CPU inside of player 2's name.", "Tic Tac Toe About");
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button_click(object sender, EventArgs e)
        {
            if ((player2 == "CPU"))
            {
                computerActive = true; 
            }
            Button b = (Button)sender;
            if (turn)
                b.Text = "X";
            else
                b.Text = "O";

            turn = !turn;
            b.Enabled = false;
            turn_count++;
            checkForWinner();

            if ((!turn) && (computerActive))
            {
                computer_make_move();
            }
        }

        private void checkForWinner()
        {
            bool winnerExists = false;
           
            //horizontal checks
                if ((A1.Text == A2.Text) && (A2.Text == A3.Text) && (!A1.Enabled))
                    winnerExists = true;
                else if ((B1.Text == B2.Text) && (B2.Text == B3.Text) && (!B1.Enabled))
                    winnerExists = true;
                else if ((C1.Text == C2.Text) && (C2.Text == C3.Text) && (!C1.Enabled))
                    winnerExists = true;
            //Verticle checks
                else if ((A1.Text == B1.Text) && (B1.Text == C1.Text) && (!A1.Enabled))
                    winnerExists = true;
                else if ((A2.Text == B2.Text) && (B2.Text == C2.Text) && (!A2.Enabled))
                    winnerExists = true;
                else if ((A3.Text == B3.Text) && (B3.Text == C3.Text) && (!A3.Enabled))
                    winnerExists = true;

            //Diagonal checks
                else if ((A1.Text == B2.Text) && (B2.Text == C3.Text) && (!A1.Enabled))
                    winnerExists = true;
                else if ((A3.Text == B2.Text) && (B2.Text == C1.Text) && (!C1.Enabled))
                    winnerExists = true;
             

                if (winnerExists)
                {
                    disableButtons();
                    String winner = "";
                    if (turn)
                    {
                        winner = player2;
                        oWins.Text = (Int32.Parse(oWins.Text) + 1).ToString(); //parses as int, then increments by one and converts back to String.
                    }
                    else
                    {
                        winner = player1;
                        xWins.Text = (Int32.Parse(xWins.Text) + 1).ToString();
                    }
                    MessageBox.Show(winner + " Wins!", "Congrats!");
       
                } //endif
                else
                {
                    if (turn_count == 9)
                    {
                        xoDraws.Text = (Int32.Parse(xoDraws.Text) + 1).ToString();
                        MessageBox.Show("Draw!", "Too bad!");
                    }
                }
          
        }//endcheck

        private void disableButtons()
        {
            try
            {
                foreach (Control c in Controls)
                {
                    Button b = (Button)c;
                    b.Enabled = false;
                }//end enhanced for loop
            }//end try 
            catch{ }
        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            turn = true;
            turn_count = 0;
            
                foreach (Control c in Controls)
                {
                    try //converting label to a button will come up with failure, this will get us past
                    {
                        Button b = (Button)c;
                        b.Enabled = true; //enable our buttons
                        b.Text = ""; //text is blank
                    }//end try
                    catch { }
                }//end enhanced for loop
           
        }

        private void button_enter(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            if (b.Enabled)
            {
                if (turn)
                {
                    b.Text = "X";
                }
                else
                {
                    b.Text = "O";
                }
            }//end b.enabled check
            
        }

        private void button_leave(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            if (b.Enabled)
            {
                
               b.Text = "";
               //REVERT
            }//end b.enabled check
        }

        private void resetCountsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            oWins.Text = "0";
            xWins.Text = "0";
            xoDraws.Text = "0";
        }


        /*AI Parts of it are from me, most of it is from Chris Merritt

         * 
         * 
         * 
         * 
         */

        private void computer_make_move()
        {
            //priority 1:  get tick tac toe
            //priority 2:  block x tic tac toe
            //priority 3:  go for corner space
            //priority 4:  pick open space

            Button move = null;

            //look for tic tac toe opportunities
            move = look_for_win_or_block("O"); //look for win
            if (move == null)
            {
                move = look_for_win_or_block("X"); //look for block
                if (move == null)
                {
                    move = look_for_corner();
                    if (move == null)
                    {
                        move = look_for_open_space();
                    }//end if
                }//end if
            }//end if

            move.PerformClick();
        }

        private Button look_for_open_space()
        {
            Console.WriteLine("Looking for open space");
            Button b = null;
            foreach (Control c in Controls)
            {
                b = c as Button;
                if (b != null)
                {
                    if (b.Text == "")
                        return b;
                }//end if
            }//end if

            return null;
        }

        private Button look_for_corner()
        {
            Console.WriteLine("Looking for corner");
            if (A1.Text == "O")
            {
                if (A3.Text == "")
                    return A3;
                if (C3.Text == "")
                    return C3;
                if (C1.Text == "")
                    return C1;
            }

            if (A3.Text == "O")
            {
                if (A1.Text == "")
                    return A1;
                if (C3.Text == "")
                    return C3;
                if (C1.Text == "")
                    return C1;
            }

            if (C3.Text == "O")
            {
                if (A1.Text == "")
                    return A3;
                if (A3.Text == "")
                    return A3;
                if (C1.Text == "")
                    return C1;
            }

            if (C1.Text == "O")
            {
                if (A1.Text == "")
                    return A3;
                if (A3.Text == "")
                    return A3;
                if (C3.Text == "")
                    return C3;
            }

            if (A1.Text == "")
                return A1;
            if (A3.Text == "")
                return A3;
            if (C1.Text == "")
                return C1;
            if (C3.Text == "")
                return C3;

            return null;
        }

        private Button look_for_win_or_block(string mark)
        {
            Console.WriteLine("Looking for win or block:  " + mark);
            //HORIZONTAL TESTS
            if ((A1.Text == mark) && (A2.Text == mark) && (A3.Text == ""))
                return A3;
            if ((A2.Text == mark) && (A3.Text == mark) && (A1.Text == ""))
                return A1;
            if ((A1.Text == mark) && (A3.Text == mark) && (A2.Text == ""))
                return A2;

            if ((B1.Text == mark) && (B2.Text == mark) && (B3.Text == ""))
                return B3;
            if ((B2.Text == mark) && (B3.Text == mark) && (B1.Text == ""))
                return B1;
            if ((B1.Text == mark) && (B3.Text == mark) && (B2.Text == ""))
                return B2;

            if ((C1.Text == mark) && (C2.Text == mark) && (C3.Text == ""))
                return C3;
            if ((C2.Text == mark) && (C3.Text == mark) && (C1.Text == ""))
                return C1;
            if ((C1.Text == mark) && (C3.Text == mark) && (C2.Text == ""))
                return C2;

            //VERTICAL TESTS
            if ((A1.Text == mark) && (B1.Text == mark) && (C1.Text == ""))
                return C1;
            if ((B1.Text == mark) && (C1.Text == mark) && (A1.Text == ""))
                return A1;
            if ((A1.Text == mark) && (C1.Text == mark) && (B1.Text == ""))
                return B1;

            if ((A2.Text == mark) && (B2.Text == mark) && (C2.Text == ""))
                return C2;
            if ((B2.Text == mark) && (C2.Text == mark) && (A2.Text == ""))
                return A2;
            if ((A2.Text == mark) && (C2.Text == mark) && (B2.Text == ""))
                return B2;

            if ((A3.Text == mark) && (B3.Text == mark) && (C3.Text == ""))
                return C3;
            if ((B3.Text == mark) && (C3.Text == mark) && (A3.Text == ""))
                return A3;
            if ((A3.Text == mark) && (C3.Text == mark) && (B3.Text == ""))
                return B3;

            //DIAGONAL TESTS
            if ((A1.Text == mark) && (B2.Text == mark) && (C3.Text == ""))
                return C3;
            if ((B2.Text == mark) && (C3.Text == mark) && (A1.Text == ""))
                return A1;
            if ((A1.Text == mark) && (C3.Text == mark) && (B2.Text == ""))
                return B2;

            if ((A3.Text == mark) && (B2.Text == mark) && (C1.Text == ""))
                return C1;
            if ((B2.Text == mark) && (C1.Text == mark) && (A3.Text == ""))
                return A3;
            if ((A3.Text == mark) && (C1.Text == mark) && (B2.Text == ""))
                return B2;

            return null;
        }




    }
}
