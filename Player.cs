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
        char rpr;
        Direction playersDirection;

        public Player(Direction dir)
        {
            this.playersDirection = dir;
            switch(dir)
            {
                case Direction.Down:
                    this.RowPos = 0;
                    this.ColPos = Board.BOARD_SIZE/2;
                    this.rpr = 'B';
                    break;
                case Direction.Up:
                    this.RowPos = Board.BOARD_SIZE-1;
                    this.ColPos = Board.BOARD_SIZE / 2;
                    this.rpr = 'A';
                    break;
                case Direction.Right:
                    this.RowPos = Board.BOARD_SIZE / 2;
                    this.ColPos = 0;
                    this.rpr = 'C';
                    break;
                case Direction.Left:
                    this.RowPos = Board.BOARD_SIZE/2;
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
            if(pd == Direction.Down || pd == Direction.Left)
            {
                res = 0;
            }
            else if(pd == Direction.Up || pd == Direction.Right)
            {
                res = Board.BOARD_SIZE - 1;
            }

            return res;
        }

        /*
         * Moves the player to one of four direction.
         * NOTE: this function is not responsible to check if the move is legal.
        */
        public bool move(string direction)
        {
            direction = direction.ToLower();
            bool res = true;
            switch(direction)
            {
                case "up":
                    this.RowPos++;
                    break;
                case "right":
                    this.ColPos++;
                    break;
                case "down":
                    this.RowPos--;
                    break;
                case "left":
                    this.ColPos--;
                    break;
                default:
                    Console.WriteLine("This move is illegal!");
                    res = false;
                    break;
            }

            return res;
        }

        public int getRowPos()
        {
            return this.RowPos;
        }

        public int getColPos()
        {
            return this.ColPos;
        }

        public Direction getPlayersDirection()
        {
            return this.playersDirection;
        }

        public char getRepresentation()
        {
            return this.rpr;
        }

    }
}
