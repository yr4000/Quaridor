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
            bool doPrint = false;

            Board b = new Board(numberOfPlayers);
            Tests.testPlaceWall(b, 0, 0, "H", doPrint);
            Tests.testPlaceWall(b, 0, 1, "H", doPrint);
            Tests.testPlaceWall(b, 1, 0, "H", doPrint);
            Tests.testPlaceWall(b, 1, 1, "H", doPrint);
            Tests.testPlaceWall(b, 4, 4, "H", doPrint);

            Tests.testPlaceWall(b, 0, 0, "V", doPrint);
            Tests.testPlaceWall(b, 1, 0, "V", doPrint);
            Tests.testPlaceWall(b, 0, 1, "V", doPrint);
            Tests.testPlaceWall(b, 1, 2, "V", doPrint);
            Tests.testPlaceWall(b, 4, 3, "V", doPrint);

            Tests.testPlaceWall(b, 4, 3, "H", doPrint);
            Tests.testPlaceWall(b, 1, 3, "V", doPrint);
            Tests.testPlaceWall(b, 1, 3, "H", doPrint);

            b.printBoard();
            Console.WriteLine("Press any key to continue....");
            Console.ReadKey();

            //present game rules
            //Console.ReadKey();
            //Console.Clear();
            //while(true)
            //choose a player to play it's turn
            //recieve an input from the player which is the move he does (moving or placing wall)
            //if the move is legal - modify the board, reprint it and change to the next player, else print en error message and wait for another command
            //if one of the players won - print a message, wait for him to press a key (new game?) and break out of the loop
            //if at any time the player types "quit", we break out of the loop and the game ends.
        }
    }
}
