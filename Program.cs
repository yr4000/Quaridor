using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quaridor
{
    class Program
    {
        static void Main(string[] args)
        {
            Board b = new Board();
            Tests.testPlaceWall(b, 0, 0, "H");
            Tests.testPlaceWall(b, 0, 1, "H");
            Tests.testPlaceWall(b, 1, 0, "H");
            Tests.testPlaceWall(b, 1, 1, "H");
            Tests.testPlaceWall(b, 4, 4, "H");

            Tests.testPlaceWall(b, 0, 0, "V");
            Tests.testPlaceWall(b, 1, 0, "V");
            Tests.testPlaceWall(b, 0, 1, "V");
            Tests.testPlaceWall(b, 1, 2, "V");
            Tests.testPlaceWall(b, 4, 3, "V");

            Tests.testPlaceWall(b, 4, 3, "H");
            Tests.testPlaceWall(b, 1, 3, "V");
            Tests.testPlaceWall(b, 1, 3, "H");
            b.printBoard();
            Console.WriteLine("Press any key to continue....");
            Console.ReadKey();
        }
    }
}
