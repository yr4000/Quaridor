using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quaridor
{
    class Tests
    {
        public static void testPlaceWall(Board b, int row, int col, string type, bool doPrint = true)
        {
            bool res = false;
            switch(type)
            {
                case "H":
                    res = b.placeHWall(row, col);
                    break;
                case "V":
                    res = b.placeVWall(row, col);
                    break;
                default:
                    Console.WriteLine("ERROR: illegal wall placement in Tests.testPlaceWall");
                    break;
            }

            Console.WriteLine("Attempt to place an " + type + " wall in (" + row + "," + col + ") resulted in: " + res);
        }
    }
}
