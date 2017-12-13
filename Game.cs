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
            /*
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

            Tests.testPlaceWall(board, 8, 4, "V", doPrint);
            Tests.testPlaceWall(board, 8, 5, "V", doPrint);
            Tests.testPlaceWall(board, 6, 4, "H", doPrint);
            board.movePlayer(board.getPlayer(0), Direction.Up);
            board.movePlayer(board.getPlayer(0), Direction.Up);
            board.movePlayer(board.getPlayer(0), Direction.Up);
            board.printBoard();
            Console.WriteLine("Press any key to continue....");
            Console.ReadKey();
            */
            //present game rules
            //Console.ReadKey();
            //Console.Clear();

            Console.WriteLine("Welcome to quoridor! are you 2 or 4 players?");
            int numberOfPlayers;
            int.TryParse(Console.ReadLine(), out numberOfPlayers);
            Board board = new Board(numberOfPlayers);
            int row, col, currentPlayerIndex = 0;
            string[] move;
            bool isLegalMove;
            bool doQuit = false;

            Console.Clear();
            while(!doQuit)
            {
                isLegalMove = false;
                Player currentPlayer = board.getPlayer(currentPlayerIndex);
                board.printBoard();
                while(!isLegalMove)
                {
                    Console.WriteLine("Player " + currentPlayer.getRepresentation() + ", please write your move: ");
                    move = Console.ReadLine().Split(' ');
                    move[0] = move[0].ToLower();
                    if (move[0] == "up")
                    {
                        isLegalMove = board.movePlayer(currentPlayer, Direction.Up);
                    }
                    else if (move[0] == "down")
                    {
                        isLegalMove = board.movePlayer(currentPlayer, Direction.Down);
                    }
                    else if (move[0] == "left")
                    {
                        isLegalMove = board.movePlayer(currentPlayer, Direction.Left);
                    }
                    else if (move[0] == "right")
                    {
                        isLegalMove = board.movePlayer(currentPlayer, Direction.Right);
                    }
                    else if (move[0] == "place")
                    {
                        if (move.Length < 4 || !int.TryParse(move[2], out row) || !int.TryParse(move[3], out col) ||
                            row < 0 || row >Board.BOARD_SIZE || col < 0 || col > Board.BOARD_SIZE)
                        {
                            Console.WriteLine("Illegal arguments for place wall move");
                        }
                        else if (currentPlayer.getWallsAmount() == 0)
                        {
                            Console.WriteLine("No walls left!");
                        }
                        else
                        {
                            move[1] = move[1].ToLower();
                            if (move[1] == "hwall")
                            {
                                isLegalMove = board.placeHWall(row, col);
                                
                            }
                            else if (move[1] == "vwall")
                            {
                                isLegalMove = board.placeVWall(row, col);
                            }
                            //decrease wall if the move was legal
                            if(isLegalMove)
                            {
                                currentPlayer.decreaseWalls();
                            }
                        }
                    }
                    else if (move[0] == "restart")
                    {
                        board.restart(numberOfPlayers);
                        currentPlayerIndex = board.getNumberOfPlayers()-1;
                        break;
                    }
                    else if (move[0] == "quit")
                    {
                        doQuit = true;
                        break;
                    }

                    if (!isLegalMove)
                    {
                        Console.WriteLine("Illegal move: no such move.");
                    }

                    if(board.playerGotToDestination(currentPlayer))
                    {
                        Console.WriteLine("Congratulations! player " + currentPlayer.getRepresentation() + " WON!!!" + Environment.NewLine +
                            "Would you like to play again? [Y\\n]");
                        if(Console.ReadLine() != "Y")
                        {
                            doQuit = true;
                            break;
                        }
                        else
                        {
                            board.restart(numberOfPlayers);
                            currentPlayerIndex = board.getNumberOfPlayers() - 1;
                        }
                    }
                }
                currentPlayerIndex = (currentPlayerIndex + 1) % board.getNumberOfPlayers();
                Console.Clear();
            }

            Console.Clear();
            Console.WriteLine("Thank you for playing, goodbye!");
            Console.ReadKey();
        }
    }
}
