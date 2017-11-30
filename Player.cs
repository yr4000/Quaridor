using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quaridor
{
    enum PlayersType {North, South, East, West}
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
        PlayersType type;

        Player(PlayersType type)
        {
            this.type = type;
            switch(type)
            {
                case PlayersType.North:
                    this.RowPos = 0;
                    this.ColPos = Board.BOARD_SIZE/2;
                    break;
                case PlayersType.South:
                    this.RowPos = Board.BOARD_SIZE-1;
                    this.ColPos = Board.BOARD_SIZE / 2;
                    break;
                case PlayersType.West:
                    this.RowPos = Board.BOARD_SIZE / 2;
                    this.ColPos = 0;
                    break;
                case PlayersType.East:
                    this.RowPos = Board.BOARD_SIZE/2;
                    this.ColPos = Board.BOARD_SIZE - 1;
                    break;
                default:
                    //TODO: throw an error
                    break;
            }
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

    }
}
