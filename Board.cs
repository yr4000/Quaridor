using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quaridor
{

    class Board
    {
        public static int BOARD_SIZE = 9;
        Slots VerticalSlots;
        Slots HorizontalSlots;
        Squares squares;
        Player[] Players;   //TODO: complete the initialization of this.

        public Board()
        {
            this.VerticalSlots = new Slots();
            this.HorizontalSlots = new Slots();
            this.squares = new Squares();
        }

        /*
         * input: players position and the destenation row 
         * output: true if the player has a route to it's destenation, else false
         * 
         * NOTE: this is simply a DFS implementation.
         */
        public bool isPlayerBlocked(Player p)
        {
            bool res = false;
            List<int> squaresSeen = new List<int>();

            int currentPosition

            return res;
        }

        /*
         * Checks if intersection between two slots is blocked.
         * NOTE: row and column both should be different from 0. The horizental slot of an intercection the one
         * to it's right, and the vertical slot is the one down to it.
        */ 
        public bool isIntersectionBlocked(int row, int col)
        {
            bool res = false;
            if(this.HorizontalSlots.isOccupied(row,col-1) && this.HorizontalSlots.isOccupied(row,col))
            {
                if(this.HorizontalSlots.isOneWall(row,col-1,row,col))
                {
                    res = true;
                }
            }
            else if(this.VerticalSlots.isOccupied(row-1,col) && this.VerticalSlots.isOccupied(row,col))
            {
                if(this.VerticalSlots.isOneWall(row-1,col,row,col))
                {
                    res = true;
                }
            }
            return res;
        }

        public bool placeHWall(int row, int col)
        {
            if (row == 0 || col==0 || isIntersectionBlocked(row,col) ||
                !this.HorizontalSlots.placeWall(row, col, row, col - 1))
            {
                return false;
            }

            return true;
        }

        public bool placeVWall(int row, int col)
        {
            if (col==0 || row==0 || isIntersectionBlocked(row, col)
                || !this.VerticalSlots.placeWall(row, col, row - 1, col))
            {
                return false;
            }

            return true;
        }

        //return's the i'th index of  square
        int getRowFromSquare(int square)
        {
            return square / BOARD_SIZE;
        }

        //return's the j'th index of  square
        int getColFromSquare(int square)
        {
            return square % BOARD_SIZE;
        }

        //returns the ID of a square in the square matrix
        int getSquareIDfromPosition(int row, int col)
        {
            return row * BOARD_SIZE + col;
        }

        //--------------------GUI----------------------------

        //prints the representation of the slots
        public void printBoard()
        {
            string HorizontalBorder = "______";
            string VerticalBorder = "|";
            string sixSpaces = "      ";
            string HorizontalBlock = "BBBBBB";
            string Block = "B";
            string BoardRpr = "";

            for (int i = 0; i < BOARD_SIZE; i++)
            {
                //print upper part of the cells
                for (int j = 0; j < BOARD_SIZE; j++)
                {
                    BoardRpr += markIntersection(i, j);

                    BoardRpr += " ";
                    if (i == 0 || !this.HorizontalSlots.isOccupied(i, j))
                    {
                        BoardRpr += HorizontalBorder;
                    }
                    else
                    {
                        BoardRpr += HorizontalBlock;
                    }
                    BoardRpr += " ";

                }
                BoardRpr += Environment.NewLine;

                //print the rest of the cells
                for (int k = 0; k < 3; k++)
                {
                    for (int j = 0; j < BOARD_SIZE; j++)
                    {
                        if (j == 0)
                        {
                            //do nothing
                        }
                        else if (this.VerticalSlots.isOccupied(i, j))
                        {
                            BoardRpr += "B";
                        }
                        else
                        {
                            BoardRpr += " ";
                        }

                        if (k != 2)
                        {
                            BoardRpr = BoardRpr + VerticalBorder + sixSpaces + VerticalBorder;
                        }
                        else
                        {
                            BoardRpr = BoardRpr + VerticalBorder + HorizontalBorder + VerticalBorder;
                        }
                    }
                    BoardRpr += Environment.NewLine;
                }
            }
            Console.Write(BoardRpr);
        }

        public string markIntersection(int row, int col)
        {
            string res = "";
            if (col != 0)
            {
                if (row != 0 && isIntersectionBlocked(row, col))
                {
                    res = "B";
                }
                else
                {
                    res = " ";
                }
            }

            return res;
        }
    }

}
