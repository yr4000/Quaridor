namespace Quaridor
{
    class Slots
    {
        private readonly Wall[,] _slotsMatrix;

        /*
         * The Walls slots includes also the the edges of the board, which in practice can't hold walls.
        */
        public Slots()
        {
            _slotsMatrix = new Wall[Board.BoardSize, Board.BoardSize];
            for(int i=0; i < Board.BoardSize; i++)
            {
                for(int j=0; j < Board.BoardSize; j++)
                {
                    _slotsMatrix[i,j] = null;
                }
            }
        }

        /*
         * Places a wall if the slots are not taken or not on the edges.
         */
        public bool PlaceWall(int rowPos1, int colPos1, int rowPos2, int colPos2)
        {
            bool res = true;
            //Player tries to place a wall on the edge - illegal!
            if (IsIndexOutOfBoundaries(rowPos1, colPos1) || IsIndexOutOfBoundaries(rowPos2, colPos2))
            {
                res = false;
            }
            //Player tries to place a wall on an occupied slot - illegal!
            else if (_slotsMatrix[rowPos1, colPos1] != null || _slotsMatrix[rowPos2, colPos2] != null)
            {
                res = false;
            }
            //everything is ok
            else
            {
                Wall wall = new Wall();
                _slotsMatrix[rowPos1, colPos1] = wall;
                _slotsMatrix[rowPos2, colPos2] = wall;
            }

            return res;
        }

        public bool RemoveWall(int rowPos1, int colPos1, int rowPos2, int colPos2)
        {
            bool res = true;
            //Player tries to place a wall on the edge - illegal!
            if (IsIndexOutOfBoundaries(rowPos1, colPos1) || IsIndexOutOfBoundaries(rowPos2, colPos2))
            {
                res = false;
            }
            //you can't remove half a wall
            else if (_slotsMatrix[rowPos1, colPos1].getId() != _slotsMatrix[rowPos2, colPos2].getId())
            {
                res = false;
            }
            //everything is ok
            else
            {
                _slotsMatrix[rowPos1, colPos1] = null;
                _slotsMatrix[rowPos2, colPos2] = null;
            }

            return res;
        }

        public bool IsOccupied(int row, int col)
        {
            if(IsIndexOutOfBoundaries(row, col))
            {
                return false;
            }
            return _slotsMatrix[row, col] != null;
        }

        /*
         * Returns true if two slots are occupied by the same wall. 
         */
        public bool IsOneWall(int rowPos1, int colPos1, int rowPos2, int colPos2)
        {
            //indexes are not checked here since if somehow the bouderies are crossed, i rather get an exception
            return _slotsMatrix[rowPos1, colPos1].getId() == _slotsMatrix[rowPos2, colPos2].getId();
        }

        public void ClearSlots()
        {
            for(int i = 0; i<Board.BoardSize; i++)
            {
                for(int j = 0; j<Board.BoardSize; j++)
                {
                    _slotsMatrix[i, j] = null;
                }
            }
        }

        private bool IsIndexOutOfBoundaries(int row, int col)
        {
            return (row < 0 || row > Board.BoardSize - 1 || col < 0 || col > Board.BoardSize - 1);
        }

    }
}
