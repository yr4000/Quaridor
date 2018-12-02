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

            Console.WriteLine("Welcome to quoridor! are you 2 or 4 players?");
            int numberOfPlayers;
            int.TryParse(Console.ReadLine(), out numberOfPlayers);
            if(!(numberOfPlayers==2 || numberOfPlayers==4))
            {
                Console.WriteLine("Ilegal number of players - creating a board for 2 players.");
                numberOfPlayers = 2;
            }
            Game.printRules();
            Board board = new Board(numberOfPlayers);
            int row, col, currentPlayerIndex = 0;
            string[] move;
            bool isLegalMove;
            bool doQuit = false;

            Console.Clear();
            while(!doQuit)
            {
                isLegalMove = false;
                Player currentPlayer = board.GetPlayer(currentPlayerIndex);
                board.PrintBoard();
                while(!isLegalMove)
                {
                    Console.WriteLine("Player " + currentPlayer.getRepresentation() + ", please write your move: ");
                    Console.WriteLine("You have " + currentPlayer.getWallsAmount() + " walls left.");
                    move = Console.ReadLine().Split(' ');
                    move[0] = move[0].ToLower();
                    if (move[0] == "up")
                    {
                        isLegalMove = board.MovePlayer(currentPlayer, Direction.Up);
                    }
                    else if (move[0] == "down")
                    {
                        isLegalMove = board.MovePlayer(currentPlayer, Direction.Down);
                    }
                    else if (move[0] == "left")
                    {
                        isLegalMove = board.MovePlayer(currentPlayer, Direction.Left);
                    }
                    else if (move[0] == "right")
                    {
                        isLegalMove = board.MovePlayer(currentPlayer, Direction.Right);
                    }
                    else if (move[0] == "place")
                    {
                        if (move.Length < 4 || !int.TryParse(move[2], out row) || !int.TryParse(move[3], out col) ||
                            row < 0 || row >Board.BoardSize || col < 0 || col > Board.BoardSize)
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
                                isLegalMove = board.PlaceHWall(row, col);
                                
                            }
                            else if (move[1] == "vwall")
                            {
                                isLegalMove = board.PlaceVWall(row, col);
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
                        board.Restart(numberOfPlayers);
                        currentPlayerIndex = board.GetNumberOfPlayers()-1;
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

                    if(board.PlayerGotToDestination(currentPlayer))
                    {
                        Console.Clear();
                        board.PrintBoard();
                        Console.WriteLine("Congratulations! player " + currentPlayer.getRepresentation() + " WON!!!" + Environment.NewLine +
                            "Would you like to play again? [Y\\n]");
                        if(Console.ReadLine() != "Y")
                        {
                            doQuit = true;
                            break;
                        }
                        else
                        {
                            board.Restart(numberOfPlayers);
                            currentPlayerIndex = board.GetNumberOfPlayers() - 1;
                        }
                    }
                }
                currentPlayerIndex = (currentPlayerIndex + 1) % board.GetNumberOfPlayers();
                Console.Clear();
            }

            Console.Clear();
            Console.WriteLine("Thank you for playing, goodbye!");
            Console.ReadKey();
        }

        static public void printRules()
        {
            Console.Write(
          "     How to play:\n" +
          "     The goal of each player is to reach to the other side of the board.\n" +
          "     Each turn you can choose to move or to place a wall. Each player has 5 or 10 walls\n" +
          "     (depends on the amount of players)\n" +

          "     You can use the following commands:\n" +
          "     - up, down, right, left: will move your piece up, down, right or left\n" +

          "     - place hwall / vwall i j: allows you to place a horizontal or a vertical wall on the[i][j] slot\n" +
          "       Examples:\n" +
          "       place hwall 2 2\n" +
          "             1        2        3\n" +
          "      ______   ______   ______ \n" +
          "     |      | |      | |      |\n" +
          "     |      | |      | |      |\n" +
          "     |______| |______| |______|\n" +
          "  1 - ______   ______   ______ \n" +
          "     |      | |      | |      |\n" +
          "     |      | |   A  | |      |\n" +
          "     |______| |______| |______|\n" +
          "  2 - ______   BBBBBB B BBBBBB\n" +
          "     |      | |      | |      |\n" +
          "     |      | |      | |      |\n" +
          "     |______| |______| |______|\n" +
          "  3 - \n" +

          "      place vwall 1 1\n" +
          "             1        2        3\n" +
          "      ______   ______   ______  \n" +
          "     |      |B|      | |      | \n" +
          "     |      |B|      | |      | \n" +
          "     |______|B|______| |______| \n" +
          "  1 - ______ B ______   ______  \n" +
          "     |      |B|      | |      | \n" +
          "     |      |B|   A  | |      | \n" +
          "     |______|B|______| |______| \n" +
          "  2 - ______   ______   ______  \n" +
          "     |      | |      | |      | \n" +
          "     |      | |      | |      | \n" +
          "     |______| |______| |______| \n" +
          "  3 -\n" +

          "     - restart: will start a new game\n" +
          "     - quit: will quit the game.\n" +

          "  Have fun!\n" +
          " (press on any key to continue)\n");

            Console.ReadKey();
        }
    }
}
