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

        public Board(int numberOFPlayers)
        {
            this.VerticalSlots = new Slots();
            this.HorizontalSlots = new Slots();
            this.squares = new Squares();
            this.Players = new Player[numberOFPlayers];
            this.Players[0] = new Player(Direction.Up);
            this.Players[1] = new Player(Direction.Down);
            if(numberOFPlayers == 4)
            {
                this.Players[2] = new Player(Direction.Right);
                this.Players[3] = new Player(Direction.Left);
            }
        }

        /*
         * input: players position and the destenation row 
         * output: true if the player has a route to it's destenation, else false
         * 
         * NOTE: this is simply a DFS implementation.
         */
        public bool isPlayerBlocked(Player p)
        {
            bool res = true;
            List<int> squaresSeen = new List<int>();

            int currentPosition = getSquareIDfromPosition(p.getRowPos(), p.getColPos());
            int nextPosition;
            PlayerCell currentSqaure = this.squares.SquareMatrix[currentPosition];
            Direction pd = p.getPlayersDirection();
            Direction movingDirection = pd;
            //this.squares.SquareMatrix[currentPosition].color = DFSColor.Grey;
            //squaresSeen.Add(currentPosition);

            while(squaresSeen.Count > 0)
            {
                if(currentSqaure.color == DFSColor.White)
                {
                    squaresSeen.Add(currentPosition);
                    currentSqaure.color = DFSColor.Grey;
                }
                //If we can move in movingDirection from currentPosition, nextPosition will get the next position index
                nextPosition = tryToMove(p, movingDirection, currentPosition);

                //if next position is illegal or the color of the next square is not white
                if(nextPosition < 0 || this.squares.SquareMatrix[nextPosition].color != DFSColor.White)
                {
                    movingDirection = getNextDirection(movingDirection);
                    //if we checked all moving directions around currentSquare
                    if(movingDirection == pd)
                    {
                        currentSqaure.color = DFSColor.Black;
                        squaresSeen.RemoveAt(squaresSeen.Count - 1);
                        currentPosition = squaresSeen.Last();
                    }
                }
                else
                {
                    currentPosition = nextPosition;
                }
                currentSqaure = this.squares.SquareMatrix[currentPosition];

                //if we got to the players destination 
                if (didPlayerGotToDestination(p,currentPosition))
                {
                    res = false;
                    break;
                }
            }

            this.squares.clearBoard();
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

        //returns the ID of a square in the square matrix
        int getSquareIDfromPosition(int row, int col)
        {
            return row * BOARD_SIZE + col;
        }

        Direction getNextDirection(Direction d)
        {
            int next = ((int)d + 1) % 4;
            Direction res = Direction.Down;
            switch(next)
            {
                case 0:
                    res = Direction.Up;
                    break;
                case 1:
                    res = Direction.Right;
                    break;
                case 2:
                    res = Direction.Down;
                    break;
                case 3:
                    res = Direction.Left;
                    break;
                default:
                    //TODO: throw expection
                    break;
            }

            return res;
        }

        int tryToMove(Player p, Direction movingDirection, int currentPosition)
        {
            int res = -1;
            switch(movingDirection)
            {
                case Direction.Up:
                    if(!this.HorizontalSlots.isOccupied(p.getRowPos(), p.getColPos()))
                    {
                        res = this.squares.getNeighbour(movingDirection, currentPosition);
                    }
                    break;
                case Direction.Right:
                    if(!this.VerticalSlots.isOccupied(p.getRowPos(), p.getColPos() + 1))
                    {
                        res = this.squares.getNeighbour(movingDirection, currentPosition);
                    }
                    break;
                case Direction.Down:
                    if(!this.HorizontalSlots.isOccupied(p.getRowPos() + 1, p.getColPos()))
                    {
                        res = this.squares.getNeighbour(movingDirection, currentPosition);
                    }
                    break;
                case Direction.Left:
                    if(!this.VerticalSlots.isOccupied(p.getRowPos(), p.getColPos()))
                    {
                        res = this.squares.getNeighbour(movingDirection, currentPosition);
                    }
                    break;
            }

            return res;
        }

        bool didPlayerGotToDestination(Player p, int currentPosition)
        {
            bool res = false;
            int rowCol = -1;
            if(p.getPlayersDirection() == Direction.Up || p.getPlayersDirection() == Direction.Down)
            {
                rowCol = this.squares.getRowFromSquare(currentPosition);
            }
            else if(p.getPlayersDirection() == Direction.Right || p.getPlayersDirection() == Direction.Left)
            {
                rowCol = this.squares.getColFromSquare(currentPosition);
            }

            if(rowCol == p.getDestination())
            {
                res = true;
            }

            return res;
        }

        //--------------------GUI----------------------------
        //TODO: change it so this representation will be kept as a class variable, and we will just update this
        //variable each move

        //prints the representation of the slots
        public void printBoard()
        {
            string HorizontalBorder = "______";
            string VerticalBorder = "|";
            string sixSpaces = "      ";
            string HorizontalBlock = "BBBBBB";
            string Block = "B";
            StringBuilder BoardRpr = new StringBuilder();

            for (int i = 0; i < BOARD_SIZE; i++)
            {
                //print upper part of the cells
                for (int j = 0; j < BOARD_SIZE; j++)
                {
                    BoardRpr.Append(markIntersection(i, j));

                    BoardRpr.Append(" ");
                    if (i == 0 || !this.HorizontalSlots.isOccupied(i, j))
                    {
                        BoardRpr.Append(HorizontalBorder);
                    }
                    else
                    {
                        BoardRpr.Append(HorizontalBlock);
                    }
                    BoardRpr.Append(" ");

                }
                BoardRpr.Append(Environment.NewLine);

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
                            BoardRpr.Append(Block);
                        }
                        else
                        {
                            BoardRpr.Append(" ");
                        }

                        if (k != 2)
                        {
                            BoardRpr.Append(VerticalBorder + sixSpaces + VerticalBorder);
                        }
                        else
                        {
                            BoardRpr.Append(VerticalBorder + HorizontalBorder + VerticalBorder);
                        }
                    }
                    BoardRpr.Append(Environment.NewLine);
                }
            }


            foreach(Player p in this.Players)
            {
                addPlayerToBoard(p, BoardRpr);
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

        public void addPlayerToBoard(Player p, StringBuilder BoardRpr)
        {
            int squareSize = this.squares.getSize();
            //since we add to each square the size of the next slot to it's right, we decrease 1 from the newLine length
            int rowSlice = squareSize * BOARD_SIZE + Environment.NewLine.Length-1;
            //each row consists of 4 slices, and we want to print the player on the third row
            int i = p.getRowPos()* rowSlice * 4 + 2 * rowSlice;
            //qw want to print the player on the fifth place in the third row of the square
            int j = p.getColPos() * squareSize + 4;

            BoardRpr[i + j] = p.getRepresentation();
        }
    }

}
