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
        Player[] Players;
        StringBuilder[][] BoardRpr;     //holds a representation of the board

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
            this.BoardRpr = initializeBoardRpr();
        }

        public Player getPlayer(int playersIndex)
        {
            return this.Players[playersIndex];
        }

        public int getNumberOfPlayers()
        {
            return this.Players.Length;
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
                nextPosition = tryToMove(p, movingDirection);

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
            bool res = true;
            if (row == 0 || col==0 || isIntersectionBlocked(row,col) ||
                !this.HorizontalSlots.placeWall(row, col, row, col - 1))
            {
                return false;
            }
            /*
            foreach(Player p in this.Players)
            {
                if(isPlayerBlocked(p))
                {
                    res = false;
                    if(!removeHWall(row, col))
                    {
                        //TODO: throw exception
                    }
                    break;
                }
            }
            */
            if(res)
            {
                paintHWall(row, col);
            }

            return res;
        }

        public bool removeHWall(int row, int col)
        {
            if(row == 0 || col == 0 || !this.HorizontalSlots.removeWall(row, col, row, col - 1))
            {
                return false;
            }

            return true;
        }

        public bool placeVWall(int row, int col)
        {
            bool res = true;
            if (col==0 || row==0 || isIntersectionBlocked(row, col)
                || !this.VerticalSlots.placeWall(row, col, row - 1, col))
            {
                return false;
            }

            /*
            foreach (Player p in this.Players)
            {
                if (isPlayerBlocked(p))
                {
                    res = false;
                    if(!removeVWall(row, col))
                    {
                        //TODO: throw an exception
                    }
                    break;
                }
            }
            */

            if(res)
            {
                paintVWall(row, col);
            }

            return res;
        }

        public bool removeVWall(int row, int col)
        {
            if (row == 0 || col == 0 || !this.VerticalSlots.removeWall(row, col, row - 1, col))
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

        public int tryToMove(Player p, Direction movingDirection)
        {
            int res = -1;
            int currentPosition = getSquareIDfromPosition(p.getRowPos(), p.getColPos());
            switch (movingDirection)
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
        static string HorizontalBorder = "______";
        static string VerticalBorder = "|";
        static string sixSpaces = "      ";
        static char Block = 'B';
        static char Space = ' ';

        StringBuilder[][] initializeBoardRpr()
        {
            StringBuilder[][] res = new StringBuilder[Board.BOARD_SIZE][];
            for(int i=0; i<Board.BOARD_SIZE; i++)
            {
                res[i] = new StringBuilder[this.squares.getHeight()];
                
                for(int j=0; j<this.squares.getHeight(); j++)
                {
                    res[i][j] = new StringBuilder();
                    //print upper part of the cells
                    for (int m = 0; j==0 && m < BOARD_SIZE; m++)
                    {
                        if (m > 0)
                        {
                            res[i][j].Append(Space);
                        }

                        res[i][j].Append(Space);
                        res[i][j].Append(HorizontalBorder);
                        res[i][j].Append(Space);

                    }

                    //print the rest of the cells
                    for (int m = 0; j > 0 && m < BOARD_SIZE; m++)
                    {
                        if (m > 0)
                        {
                            res[i][j].Append(Space);
                        }

                        if (j != this.squares.getHeight() - 1)
                        {
                            res[i][j].Append(VerticalBorder + sixSpaces + VerticalBorder);
                        }
                        else
                        {
                            res[i][j].Append(VerticalBorder + HorizontalBorder + VerticalBorder);
                        }
                    }
                    res[i][j].Append(Environment.NewLine);
                }
            }

            foreach (Player p in this.Players)
            {
                paintPlayerToBoard(p, res);
            }

            return res;
        }

        public void printBoard()
        {
            foreach(StringBuilder[] row in this.BoardRpr)
            {
                foreach(StringBuilder slice in row)
                {
                    Console.Write(slice);
                }
            }
        }

        //TODO: get rid of this?
        void markIntersection(int row, int col)
        {
            if (col != 0)
            {
                if (row != 0 && isIntersectionBlocked(row, col))
                {
                    this.BoardRpr[row][0][getSquareRprIndex(col)-1] = Block;
                }
                else
                {
                    this.BoardRpr[row][0][getSquareRprIndex(col)-1] = Space;
                }
            }
        }

        void paintPlayerToBoard(Player p, StringBuilder[][] BoardRpr)
        {

            BoardRpr[p.getRowPos()][this.squares.getHeight()/2]
                [getSquareRprIndex(p.getColPos()) + this.squares.getWidth()/2] = p.getRepresentation();
        }

        void paintHWall(int row, int col, bool doErase = false)
        {
            char c = Block;
            if (doErase)
                c = Space;
            for(int i=1; i<this.squares.getWidth()-2; i++)
            {
                this.BoardRpr[row][0][getSquareRprIndex(col) + i] = c;
                this.BoardRpr[row][0][getSquareRprIndex(col-1) + i] = c;
            }
            markIntersection(row, col);
        }

        void paintVWall(int row, int col, bool doErase = false)
        {
            char c = Block;
            if (doErase)
                c = Space;
            for (int i=1; i<this.squares.getHeight(); i++)
            {
                this.BoardRpr[row][i][getSquareRprIndex(col)-1] = c;
                this.BoardRpr[row - 1][i][getSquareRprIndex(col)-1] = c;
            }

            markIntersection(row, col);
        }


        //TODO: temporary public
        void paintSquare(int row, int col, DFSColor color)
        {
            char c = ' ';
            if(color == DFSColor.Grey)
            {
                c = '\\';
            }
            else if(color == DFSColor.Black)
            {
                c = 'X';
            }
            for(int i=1; i<this.squares.getHeight(); i++)
            {
                for(int j=1; j<this.squares.getWidth()-2; j++)
                {
                    this.BoardRpr[row][i][getSquareRprIndex(col) + j] = c;
                }
            }
        }

        int getSquareRprIndex(int col)
        {
            return col * this.squares.getWidth();
        }

    }

}
