using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quaridor
{
    public class Board
    {
        #region Members

        public static int BoardSize = 9;
        private const int WallAmount = 20;
        private readonly Slots _verticalSlots;
        private readonly Slots _horizontalSlots;
        private readonly Squares _squares;
        private readonly Player[] _players;
        private StringBuilder[][] _boardRpr;

        #endregion Members

        #region Constructor

        public Board(int numberOfPlayers)
        {
            _verticalSlots = new Slots();
            _horizontalSlots = new Slots();
            _squares = new Squares();
            _players = new Player[numberOfPlayers];
            _players[0] = new Player(Direction.Up, WallAmount / numberOfPlayers);
            _players[1] = new Player(Direction.Down, WallAmount / numberOfPlayers);
            if (numberOfPlayers == 4)
            {
                _players[2] = new Player(Direction.Right, WallAmount / numberOfPlayers);
                _players[3] = new Player(Direction.Left, WallAmount / numberOfPlayers);
            }

            _boardRpr = InitializeBoardRpr();
            PaintPlayersToBoard();
        }

        #endregion Constructor

        public Player GetPlayer(int playersIndex)
        {
            return _players[playersIndex];
        }

        public int GetNumberOfPlayers()
        {
            return _players.Length;
        }

        //return represantation of the board as string, for debugging.
        public string GetBoardRprAsString()
        {
            var res = new StringBuilder();
            foreach (var row in _boardRpr)
            foreach (var slice in row)
                res.Append(slice);

            return res.ToString();
        }

        /*
         * input: players position and the destenation row 
         * output: true if the player has a route to it's destenation, else false
         * 
         * NOTE: this is simply a DFS implementation.
         */
        public bool IsPlayerBlocked(Player p)
        {
            var res = true;
            var squaresSeen = new List<int>();

            var currentPosition = p.getSquare();
            int nextPosition;
            var currentSqaure = _squares[currentPosition];
            var pd = p.getPlayersDirection();
            var movingDirection = pd;
            //this.squares.SquareMatrix[currentPosition].color = DFSColor.Grey;
            //squaresSeen.Add(currentPosition);

            do
            {
                if (currentSqaure.color == DFSColor.White)
                {
                    squaresSeen.Add(currentPosition);
                    currentSqaure.color = DFSColor.Grey;
                    PaintSquare(currentPosition, DFSColor.Grey);
                }

                //If we can move in movingDirection from currentPosition, nextPosition will get the next position index
                nextPosition = TryToMove(currentPosition, movingDirection);

                //if next position is illegal or the color of the next square is not white
                if (nextPosition < 0 || _squares[nextPosition].color != DFSColor.White)
                {
                    movingDirection = getNextDirection(movingDirection);
                    //if we checked all moving directions around currentSquare
                    if (movingDirection == pd)
                    {
                        currentSqaure.color = DFSColor.Black;
                        PaintSquare(currentPosition, DFSColor.Black);
                        squaresSeen.RemoveAt(squaresSeen.Count - 1);
                        if (!(squaresSeen.Count == 0))
                            currentPosition = squaresSeen.Last();
                    }
                }
                else
                {
                    currentPosition = nextPosition;
                    movingDirection = pd;
                }

                currentSqaure = _squares[currentPosition];

                //if we got to the players destination 
                if (DidPlayerGotToDestination(p, currentPosition))
                {
                    res = false;
                    break;
                }
            } while (squaresSeen.Count > 0);

            _squares.clearBoard();
            PaintSquaresByTheirColor();
            PaintPlayersToBoard();
            return res;
        }

        /*
         * Checks if intersection between two slots is blocked.
         * NOTE: row and column both should be different from 0. The horizental slot of an intercection the one
         * to it's right, and the vertical slot is the one down to it.
        */
        public bool IsIntersectionBlocked(int row, int col)
        {
            var res = false;
            if (_horizontalSlots.IsOccupied(row, col - 1) && _horizontalSlots.IsOccupied(row, col))
            {
                if (_horizontalSlots.IsOneWall(row, col - 1, row, col)) res = true;
            }
            else if (_verticalSlots.IsOccupied(row - 1, col) && _verticalSlots.IsOccupied(row, col))
            {
                if (_verticalSlots.IsOneWall(row - 1, col, row, col)) res = true;
            }

            return res;
        }

        public bool PlaceHWall(int row, int col)
        {
            var res = true;
            if (row == 0 || col == 0 || IsIntersectionBlocked(row, col) ||
                !_horizontalSlots.PlaceWall(row, col, row, col - 1))
                return false;


            foreach (var p in _players)
                if (IsPlayerBlocked(p))
                {
                    res = false;
                    if (!RemoveHWall(row, col))
                    {
                        //TODO: throw exception
                    }

                    break;
                }

            if (res) PaintHWall(row, col);

            return res;
        }

        public bool RemoveHWall(int row, int col)
        {
            if (row == 0 || col == 0 || !_horizontalSlots.RemoveWall(row, col, row, col - 1)) return false;

            return true;
        }

        public bool PlaceVWall(int row, int col)
        {
            var res = true;
            if (col == 0 || row == 0 || IsIntersectionBlocked(row, col)
                || !_verticalSlots.PlaceWall(row, col, row - 1, col))
                return false;


            foreach (var p in _players)
                if (IsPlayerBlocked(p))
                {
                    res = false;
                    if (!RemoveVWall(row, col))
                    {
                        //TODO: throw an exception
                    }

                    break;
                }


            if (res) PaintVWall(row, col);

            return res;
        }

        public bool RemoveVWall(int row, int col)
        {
            if (row == 0 || col == 0 || !_verticalSlots.RemoveWall(row, col, row - 1, col)) return false;

            return true;
        }

        private Direction getNextDirection(Direction d)
        {
            var next = ((int) d + 1) % 4;
            Direction res;
            switch (next)
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
                    throw new Exception("Unsupported direction.");
            }

            return res;
        }

        public bool MovePlayer(Player p, Direction movingDirection)
        {
            var res = InnerMove(p, movingDirection);
            foreach (var po in _players)
                if (p.getRowPos() == po.getRowPos() &&
                    p.getColPos() == po.getColPos() &&
                    p.getRepresentation() != po.getRepresentation())
                    InnerMove(p, movingDirection);
            PaintPlayersToBoard();
            return res;
        }

        private bool InnerMove(Player p, Direction movingDirection)
        {
            var res = false;
            var playersPosition = p.getSquare();
            PaintSquare(playersPosition, DFSColor.White);
            if (TryToMove(playersPosition, movingDirection) > -1)
            {
                p.move(movingDirection);
                res = true;
            }

            return res;
        }

        public int TryToMove(int currentPosition, Direction movingDirection)
        {
            var res = -1;
            switch (movingDirection)
            {
                case Direction.Up:
                    if (!_horizontalSlots.IsOccupied(_squares.getRowFromPosition(currentPosition),
                        _squares.getColFromPosition(currentPosition)))
                        res = _squares.getNeighbour(movingDirection, currentPosition);
                    break;
                case Direction.Right:
                    if (!_verticalSlots.IsOccupied(_squares.getRowFromPosition(currentPosition),
                        _squares.getColFromPosition(currentPosition) + 1))
                        res = _squares.getNeighbour(movingDirection, currentPosition);
                    break;
                case Direction.Down:
                    if (!_horizontalSlots.IsOccupied(_squares.getRowFromPosition(currentPosition) + 1,
                        _squares.getColFromPosition(currentPosition)))
                        res = _squares.getNeighbour(movingDirection, currentPosition);
                    break;
                case Direction.Left:
                    if (!_verticalSlots.IsOccupied(_squares.getRowFromPosition(currentPosition),
                        _squares.getColFromPosition(currentPosition)))
                        res = _squares.getNeighbour(movingDirection, currentPosition);
                    break;
            }

            return res;
        }

        public bool PlayerGotToDestination(Player p)
        {
            return DidPlayerGotToDestination(p, p.getSquare());
        }

        private bool DidPlayerGotToDestination(Player p, int currentPosition)
        {
            var res = false;
            var rowCol = -1;
            if (p.getPlayersDirection() == Direction.Up || p.getPlayersDirection() == Direction.Down)
                rowCol = _squares.getRowFromPosition(currentPosition);
            else if (p.getPlayersDirection() == Direction.Right || p.getPlayersDirection() == Direction.Left)
                rowCol = _squares.getColFromPosition(currentPosition);

            if (rowCol == p.getDestination()) res = true;

            return res;
        }

        /*
         * Using Dijkstra like algorithm we can find the shortest path from the player to it's destination.
         * We assume that the player is not blocked, and that each edge between two squeres wights 1
         */
        public int FindShortestPath(Player p)
        {
            //initialize neccessary arguments 
            var shortestPathLen = int.MaxValue;
            var sqauresQ = new List<int>();
            for (var i = 0; i < BoardSize * BoardSize; i++) sqauresQ.Add(i);
            var currentSquare = p.getSquare();
            var allLonger = false;
            var d = Direction.Up;
            _squares[currentSquare].distFromSource = 0;

            while (sqauresQ.Count() != 0 && !allLonger)
            {
                //checking eack possible direction
                for (var i = 0; i < 4; i++)
                {
                    var neigh = TryToMove(currentSquare, d);
                    if (neigh >= 0 &&_squares[neigh].distFromSource > _squares[currentSquare].distFromSource + 1)
                        _squares[neigh].distFromSource = _squares[currentSquare].distFromSource + 1;

                    d = getNextDirection(d);
                }

                //pop current square and find the next square to search from
                sqauresQ.Remove(currentSquare);
                currentSquare = sqauresQ[0];
                foreach (var square in sqauresQ)
                    if (_squares[currentSquare].distFromSource > _squares[square].distFromSource)
                        currentSquare = square;

                //if the sqaure we found is further away from the shortest path we found there is no point to continue the search.
                if (_squares[currentSquare].distFromSource >= shortestPathLen)
                    allLonger = true;
                //if we got to desteneation, update result
                else if (DidPlayerGotToDestination(p, currentSquare) &&
                         _squares[currentSquare].distFromSource < shortestPathLen)
                    shortestPathLen = _squares[currentSquare].distFromSource;
            }

            _squares.clearBoard();
            return shortestPathLen;
        }

        public void Restart(int numberOfPlayers)
        {
            _squares.clearBoard();
            PaintSquaresByTheirColor();
            _horizontalSlots.ClearSlots();
            _verticalSlots.ClearSlots();
            foreach (var p in _players) p.initPlayer(WallAmount / numberOfPlayers);
            _boardRpr = InitializeBoardRpr();
            PaintPlayersToBoard();
        }

        #region GUI

        //--------------------GUI----------------------------
        //TODO: change it so this representation will be kept as a class variable, and we will just update this
        //variable each move
        private static readonly string HorizontalBorder = "______";
        private static readonly string VerticalBorder = "|";
        private static readonly string sixSpaces = "      ";
        private static readonly char Block = 'B';
        private static readonly char Space = ' ';

        private StringBuilder[][] InitializeBoardRpr()
        {
            var res = new StringBuilder[BoardSize][];
            for (var i = 0; i < BoardSize; i++)
            {
                res[i] = new StringBuilder[_squares.getHeight()];

                for (var j = 0; j < _squares.getHeight(); j++)
                {
                    res[i][j] = new StringBuilder();
                    //print upper part of the cells
                    for (var m = 0; j == 0 && m < BoardSize; m++)
                    {
                        if (m > 0) res[i][j].Append(Space);

                        res[i][j].Append(Space);
                        res[i][j].Append(HorizontalBorder);
                        res[i][j].Append(Space);
                    }

                    //print the rest of the cells
                    for (var m = 0; j > 0 && m < BoardSize; m++)
                    {
                        if (m > 0) res[i][j].Append(Space);

                        if (j != _squares.getHeight() - 1)
                            res[i][j].Append(VerticalBorder + sixSpaces + VerticalBorder);
                        else
                            res[i][j].Append(VerticalBorder + HorizontalBorder + VerticalBorder);
                    }

                    res[i][j].Append(Environment.NewLine);
                }
            }

            return res;
        }

        public void PrintBoard()
        {
            Console.Write("    ");
            for (var i = 1; i < _boardRpr.Length; i++) Console.Write("        {0}", i);
            Console.Write(Environment.NewLine);
            for (var i = 0; i < _boardRpr.Length; i++)
            for (var j = 0; j < _boardRpr[i].Length; j++)
            {
                if (i == 0 || j > 0)
                    Console.Write("    ");
                else
                    Console.Write("{0} - ", i);
                Console.Write(_boardRpr[i][j]);
            }
        }

        //TODO: get rid of this?
        private void MarkIntersection(int row, int col)
        {
            if (col != 0)
            {
                if (row != 0 && IsIntersectionBlocked(row, col))
                    _boardRpr[row][0][GetSquareRprIndex(col) - 1] = Block;
                else
                    _boardRpr[row][0][GetSquareRprIndex(col) - 1] = Space;
            }
        }

        private void PaintPlayersToBoard()
        {
            foreach (var p in _players)
                _boardRpr[p.getRowPos()][_squares.getHeight() / 2]
                    [GetSquareRprIndex(p.getColPos()) + _squares.getWidth() / 2] = p.getRepresentation();
        }

        private void PaintHWall(int row, int col, bool doErase = false)
        {
            if (!(row > BoardSize - 1 || row < 0 || col > BoardSize - 1 || col < 0))
            {
                var c = Block;
                if (doErase)
                    c = Space;
                for (var i = 1; i < _squares.getWidth() - 2; i++)
                {
                    _boardRpr[row][0][GetSquareRprIndex(col) + i] = c;
                    _boardRpr[row][0][GetSquareRprIndex(col - 1) + i] = c;
                }

                MarkIntersection(row, col);
            }
        }

        private void PaintVWall(int row, int col, bool doErase = false)
        {
            if (!(row > BoardSize - 1 || row < 0 || col > BoardSize - 1 || col < 0))
            {
                var c = Block;
                if (doErase)
                    c = Space;
                for (var i = 1; i < _squares.getHeight(); i++)
                {
                    _boardRpr[row][i][GetSquareRprIndex(col) - 1] = c;
                    _boardRpr[row - 1][i][GetSquareRprIndex(col) - 1] = c;
                }

                MarkIntersection(row, col);
            }
        }


        private void PaintSquare(int row, int col, DFSColor color)
        {
            var c = ' ';
            if (color == DFSColor.Grey)
                c = '\\';
            else if (color == DFSColor.Black) c = 'X';
            for (var i = 1; i < _squares.getHeight(); i++)
            for (var j = 1; j < _squares.getWidth() - 2; j++)
            {
                _boardRpr[row][i][GetSquareRprIndex(col) + j] = c;
                if (i == _squares.getHeight() - 1 && color == DFSColor.White)
                    _boardRpr[row][i][GetSquareRprIndex(col) + j] = '_';
            }
        }

        private void PaintSquare(int squarePosition, DFSColor color)
        {
            var row = _squares.getRowFromPosition(squarePosition);
            var col = _squares.getColFromPosition(squarePosition);
            PaintSquare(row, col, color);
        }

        private int GetSquareRprIndex(int col)
        {
            return col * _squares.getWidth();
        }

        private void PaintSquaresByTheirColor()
        {
            for (var i = 0; i < _squares.size(); i++)
                PaintSquare(_squares.getRowFromPosition(i), _squares.getColFromPosition(i), _squares.GetSquareColor(i));
        }
    }
}

#endregion GUI