using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quaridor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quaridor.Tests
{
    [TestClass()]
    public class BoardTests
    {
        [TestMethod()]
        public void findShortestPathTest()
        {
            Board b = new Board(2);
            void isCorrectShortestPath(int playerIndex, int length, string msg)
            {
                if (b.FindShortestPath(b.GetPlayer(playerIndex)) != length)
                {
                    Console.WriteLine(msg);
                    Assert.Fail();
                }
            }
            isCorrectShortestPath(1, 8, "Failed findShortestPathTest: no walls test");
            b.PlaceHWall(1, 4);
            isCorrectShortestPath(1, 9, "Failed findShortestPathTest: one wall test");
            b.RemoveHWall(1, 4);

            for(int i=0; i<3; i++)
            {
                b.GetPlayer(1).move(Direction.Down);
                b.GetPlayer(1).move(Direction.Left);
                b.PlaceHWall(4, 1 + 2 * i);
                b.PlaceHWall(5, 2 + 2 * i);
            }
            b.PlaceHWall(6, 1);
            b.PlaceHWall(6, 4);
            b.PlaceVWall(3, 5);
            b.PlaceVWall(4, 7);
            isCorrectShortestPath(1, 15, $"Failed findShortestPathTest: player{1} board state 0");
            b.PlaceHWall(4, 8);
            isCorrectShortestPath(1, 22, $"Failed findShortestPathTest: player{1} complex state 1");
            isCorrectShortestPath(0, 18, $"Failed findShortestPathTest: player{0} complex state 1");
            
        }
    }
}