using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quaridor
{
    class Squares
    {
        PlayerCell[] SquareMatrix;

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

        public int getNeighbour(Direction dir, int square)
        {
            //TODO: complete
        }

    }
}
