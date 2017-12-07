using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quaridor
{
    class Game
    {
        static void Main(string[] args)
        {
            int numberOfPlayers = 4;
            Board board = new Board(numberOfPlayers);
            bool doPrint = false;

            //this is for tests
            Tests.testPlaceWall(board, 0, 0, "H", doPrint);
            Tests.testPlaceWall(board, 0, 1, "H", doPrint);
            Tests.testPlaceWall(board, 1, 0, "H", doPrint);
            Tests.testPlaceWall(board, 1, 1, "H", doPrint);
            Tests.testPlaceWall(board, 4, 4, "H", doPrint);

            Tests.testPlaceWall(board, 0, 0, "V", doPrint);
            Tests.testPlaceWall(board, 1, 0, "V", doPrint);
            Tests.testPlaceWall(board, 0, 1, "V", doPrint);
            Tests.testPlaceWall(board, 1, 2, "V", doPrint);
            Tests.testPlaceWall(board, 4, 3, "V", doPrint);

            Tests.testPlaceWall(board, 4, 3, "H", doPrint);
            Tests.testPlaceWall(board, 1, 3, "V", doPrint);
            Tests.testPlaceWall(board, 1, 3, "H", doPrint);
            /*
            board.paintSquare(0, 0, DFSColor.Black);
            board.paintSquare(8, 8, DFSColor.Black);
            board.paintSquare(6, 7, DFSColor.Grey);
            board.paintSquare(7, 5, DFSColor.Grey);
            */
            board.printBoard();
            Console.WriteLine("Press any key to continue....");
            Console.ReadKey();

            //present game rules
            //Console.ReadKey();
            //Console.Clear();
            int row, col, currentPlayerIndex = 0;
            string[] move;
            bool isLegalMove;

            /*
            while(true)
            {
                isLegalMove = false;
                Player currentPlayer = board.getPlayer(currentPlayerIndex);
                board.printBoard();
                while(!isLegalMove)
                {
                    Console.WriteLine("Please enter your move: ");
                    move = Console.ReadLine().Split(' ');
                    move[0] = move[0].ToLower();
                    if (move[0] == "up")
                    {
                        if (board.tryToMove(currentPlayer, Direction.Up) > 0)
                        {
                            currentPlayer.move(Direction.Up);
                            isLegalMove = true;
                        }
                    }
                    else if (move[0] == "down")
                    {
                        if (board.tryToMove(currentPlayer, Direction.Down) > 0)
                        {
                            currentPlayer.move(Direction.Down);
                            isLegalMove = true;
                        }
                    }
                    else if (move[0] == "left")
                    {
                        if (board.tryToMove(currentPlayer, Direction.Left) > 0)
                        {
                            currentPlayer.move(Direction.Left);
                            isLegalMove = true;
                        }
                    }
                    else if (move[0] == "right")
                    {
                        if (board.tryToMove(currentPlayer, Direction.Right) > 0)
                        {
                            currentPlayer.move(Direction.Right);
                            isLegalMove = true;
                        }
                    }
                    else if (move[0] == "place")
                    {
                        if (move.Length < 4 || !int.TryParse(move[2], out row) || int.TryParse(move[3], out col))
                        {
                            Console.WriteLine("Illegal arguments for place wall move");
                        }
                        else
                        {
                            move[1] = move[1].ToLower();
                            if (move[1] == "hwall")
                            {
                                if (board.placeHWall(row, col))
                                    isLegalMove = true;
                            }
                            else if (move[1] == "vwall")
                            {
                                if (board.placeVWall(row, col))
                                    isLegalMove = true;
                            }
                        }
                    }
                    else if (move[0] == "restart")
                    {
                        //TODO: complete
                    }
                    else if (move[0] == "quit")
                    {
                        //lshfleshldkfh
                    }

                    if (!isLegalMove)
                    {
                        Console.WriteLine("Illegal move: no such move.");
                    }
                }
                currentPlayerIndex = (currentPlayerIndex + 1) % board.getNumberOfPlayers();
            }
            //recieve an input from the player which is the move he does (moving or placing wall)
            //if the move is legal - modify the board, reprint it and change to the next player, else print en error message and wait for another command
            //if one of the players won - print a message, wait for him to press a key (new game?) and break out of the loop
            //if at any time the player types "quit", we break out of the loop and the game ends.
            */
        }
    }
}
