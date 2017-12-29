using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quaridor
{
    enum Direction {Up, Right, Down, Left}

    class Wall
    {
        public static int wallCount = 0;
        int id;

        public Wall()
        {
            this.id = wallCount;
            wallCount++;
        }

        public int getId()
        {
            return this.id;
        }
    }

    class Player
    {
        int RowPos;
        int ColPos;
        int wallsAmount;
        char rpr;
        Direction playersDirection;

        public Player(Direction dir, int wallNumber)
        {
            this.playersDirection = dir;
            initPlayer(wallNumber);
        }

        public void initPlayer(int wallNumber)
        {
            this.wallsAmount = wallNumber;
            switch (this.playersDirection)
            {
                case Direction.Down:
                    this.RowPos = 0;
                    this.ColPos = Board.BOARD_SIZE / 2;
                    this.rpr = 'B';
                    break;
                case Direction.Up:
                    this.RowPos = Board.BOARD_SIZE - 1;
                    this.ColPos = Board.BOARD_SIZE / 2;
                    this.rpr = 'A';
                    break;
                case Direction.Right:
                    this.RowPos = Board.BOARD_SIZE / 2;
                    this.ColPos = 0;
                    this.rpr = 'C';
                    break;
                case Direction.Left:
                    this.RowPos = Board.BOARD_SIZE / 2;
                    this.ColPos = Board.BOARD_SIZE - 1;
                    this.rpr = 'D';
                    break;
                default:
                    //TODO: throw an error
                    break;
            }
        }

        public int getDestination()
        {
            int res = -1;
            Direction pd = this.playersDirection;
            if(pd == Direction.Up || pd == Direction.Left)
            {
                res = 0;
            }
            else if(pd == Direction.Down || pd == Direction.Right)
            {
                res = Board.BOARD_SIZE - 1;
            }

            return res;
        }

        /*
         * Moves the player to one of four direction.
         * NOTE: this function is not responsible to check if the move is legal.
        */
        public bool move(Direction direction)
        {
            bool res = true;
            switch(direction)
            {
                case Direction.Up:
                    this.RowPos--;
                    break;
                case Direction.Right:
                    this.ColPos++;
                    break;
                case Direction.Down:
                    this.RowPos++;
                    break;
                case Direction.Left:
                    this.ColPos--;
                    break;
                default:
                    Console.WriteLine("This move is illegal!");     //TODO: change to something more informative
                    res = false;
                    break;
            }

            return res;
        }

        public void decreaseWalls()
        {
            this.wallsAmount--;
        }

        public int getRowPos()
        {
            return this.RowPos;
        }

        public int getColPos()
        {
            return this.ColPos;
        }

        //returns the ID of a square in the square matrix
        public int getSquare()
        {
            return this.RowPos * Board.BOARD_SIZE + this.ColPos;
        }

        public Direction getPlayersDirection()
        {
            return this.playersDirection;
        }

        public char getRepresentation()
        {
            return this.rpr;
        }

        public int getWallsAmount()
        {
            return this.wallsAmount;
        }

    }
}
