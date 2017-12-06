using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quaridor
{
    class Squares
    {
        public PlayerCell[] SquareMatrix;
        int Width = 9;
        int Height = 4;

        public Squares()
        {
            this.SquareMatrix = new PlayerCell[Board.BOARD_SIZE* Board.BOARD_SIZE];
            for(int i=0; i<this.SquareMatrix.Length; i++)
            {
                this.SquareMatrix[i] = new PlayerCell();
            }
        }

        public void clearBoard()
        {
            for (int i = 0; i < this.SquareMatrix.Length; i++)
            {
                this.SquareMatrix[i].color = DFSColor.White;
            }
        }

        /*
         * returns the position of the square in direction dir to square.
         * if the direction is illegal, returns -1;
        */
        public int getNeighbour(Direction dir, int square)
        {
            int res = -1;
            switch (dir)
            {
                case Direction.Down:
                    if(!(getRowFromSquare(square) >= Board.BOARD_SIZE-1))
                    {
                        res = square + Board.BOARD_SIZE;
                    }
                    break;
                case Direction.Left:
                    if(getColFromSquare(square) != 0)
                    {
                        res = square - 1;
                    }
                    break;
                case Direction.Up:
                    if(!(getRowFromSquare(square) <= 0))
                    {
                        res = square - Board.BOARD_SIZE;
                    }
                    break;
                case Direction.Right:
                    if(getColFromSquare(square) != Board.BOARD_SIZE-1)
                    {
                        res = square + 1;
                    }
                    break;
                default:
                    //TODO: through exception
                    break;
            }
            return res;
        }

        //return's the i'th index of  square
        public int getRowFromSquare(int square)
        {
            return square / Board.BOARD_SIZE;
        }

        //return's the j'th index of  square
        public int getColFromSquare(int square)
        {
            return square % Board.BOARD_SIZE;
        }

        public int getWidth()
        {
            return this.Width;
        }

        public int getHeight()
        {
            return this.Height;
        }

    }
}
