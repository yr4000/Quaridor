using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quaridor
{
    class Slots
    {
        WallCell[,] SlotsMatrix;

        /*
         * The Walls slots includes also the the edges of the board, which in practice can't hold walls.
        */
        public Slots()
        {
            SlotsMatrix = new WallCell[Board.BOARD_SIZE, Board.BOARD_SIZE];
            for(int i=0; i < Board.BOARD_SIZE; i++)
            {
                for(int j=0; j < Board.BOARD_SIZE; j++)
                {
                    SlotsMatrix[i,j] = new WallCell();
                }
            }
        }

        /*
         * Places a wall if the slots are not taken or not on the edges.
         */
        public bool placeWall(int RowPos1, int ColPos1, int RowPos2, int ColPos2)
        {
            bool res = true;
            //Player tries to place a wall on the edge - illegal!
            if (isIndexOutOfBoudaries(RowPos1, ColPos1) || isIndexOutOfBoudaries(RowPos2, ColPos2))
            {
                res = false;
            }
            //Player tries to place a wall on an occupied slot - illegal!
            else if (this.SlotsMatrix[RowPos1, ColPos1].Occupied || this.SlotsMatrix[RowPos2, ColPos2].Occupied)
            {
                res = false;
            }
            //everything is ok
            else
            {
                Wall wall = new Wall();
                this.SlotsMatrix[RowPos1, ColPos1].Occupied = true;
                this.SlotsMatrix[RowPos1, ColPos1].wall = wall;
                this.SlotsMatrix[RowPos2, ColPos2].Occupied = true;
                this.SlotsMatrix[RowPos2, ColPos2].wall = wall;
            }

            return res;
        }

        public bool removeWall(int RowPos1, int ColPos1, int RowPos2, int ColPos2)
        {
            bool res = true;
            //Player tries to place a wall on the edge - illegal!
            if (isIndexOutOfBoudaries(RowPos1, ColPos1) || isIndexOutOfBoudaries(RowPos2, ColPos2))
            {
                res = false;
            }
            //you can't remove half a wall
            else if (this.SlotsMatrix[RowPos1, ColPos1].wall.getId() != this.SlotsMatrix[RowPos2, ColPos2].wall.getId())
            {
                res = false;
            }
            //everything is ok
            else
            {
                this.SlotsMatrix[RowPos1, ColPos1].Occupied = false;
                this.SlotsMatrix[RowPos1, ColPos1].wall = null;
                this.SlotsMatrix[RowPos2, ColPos2].Occupied = false;
                this.SlotsMatrix[RowPos2, ColPos2].wall = null;
            }

            return res;
        }

        public bool isOccupied(int row, int col)
        {
            if(isIndexOutOfBoudaries(row, col))
            {
                return false;
            }
            return this.SlotsMatrix[row, col].Occupied;
        }

        /*
         * Returns true if two slots are occupied by the same wall. 
         */
        public bool isOneWall(int RowPos1, int ColPos1, int RowPos2, int ColPos2)
        {
            //indexes are not checked here since if somehow the bouderies are crossed, i rather get an exception
            return this.SlotsMatrix[RowPos1, ColPos1].wall.getId() == this.SlotsMatrix[RowPos2, ColPos2].wall.getId();
        }

        //Returns a copy of the wall in the [row,col] cell.
        public WallCell getWallFromCell(int row, int col)
        {
            WallCell res = null;
            if(!isIndexOutOfBoudaries(row, col))
            {
                res = this.SlotsMatrix[row, col].copy();
            }
            return res;
        }

        public void clearSlots()
        {
            for(int i = 0; i<Board.BOARD_SIZE; i++)
            {
                for(int j = 0; j<Board.BOARD_SIZE; j++)
                {
                    this.SlotsMatrix[i, j].Occupied = false;
                    this.SlotsMatrix[i, j].wall = null;
                }
            }
        }

        bool isIndexOutOfBoudaries(int row, int col)
        {
            return (row < 0 || row > Board.BOARD_SIZE - 1 || col < 0 || col > Board.BOARD_SIZE - 1);
        }

    }
}
