using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using System.Drawing;

namespace CatanApp
{
    [TestClass]
    public class BoardTest 
    {
        /***********************************************************************/
        // Testing: RandomizeBoard
        /***********************************************************************/
        // Tests include: testing that the board dictionary covers the correct 
        // number of chits and resources, checks that the desert has chit 7 and
        // that the robber begins on the desert tile
        /***********************************************************************/
        [TestMethod]
        public void TestAllResourcesCovered()
        {
            Board testBoard = new Board(0);
            //testBoard.RandomizeBoard(); 
            Dictionary<int, Tile> tiles = testBoard.Tiles; 
            int forest_count = 0;
            int hills_count = 0;
            int fields_count = 0;
            int pasture_count = 0;
            int mountain_count = 0;
            int desert_count = 0; 

            for (int i = 0; i < tiles.Count; i++)
            {  
                string temp_res = tiles[i].Resource;
                if (temp_res == "wood")
                {
                    forest_count++;
                }
                else if (temp_res == "wheat")
                {
                    fields_count++;
                }
                else if (temp_res == "brick")
                {
                    hills_count++;
                }
                else if (temp_res == "sheep")
                {
                    pasture_count++;
                }
                else if (temp_res == "ore")
                {
                    mountain_count++;
                }
                else if (temp_res == "desert")
                {
                    desert_count++;
                }
            }

            Assert.AreEqual(19, tiles.Count, "expected 19 tiles in the dictionary");
            Assert.AreEqual(4, forest_count, "expected 4 wood tiles on the board");
            Assert.AreEqual(4, pasture_count, "expected 4 sheep tiles on the board");
            Assert.AreEqual(4, fields_count, "expected 4 wheat tiles on the board");
            Assert.AreEqual(3, hills_count, "expected 3 brick tiles on the board");
            Assert.AreEqual(3, mountain_count, "expected 4 ore tiles on the board");
            Assert.AreEqual(1, desert_count, "expected 1 desert tile on the board");
        }
        [TestMethod]
        public void TestAllChitsCovered()
        {
            Board testBoard = new Board(0);
            //testBoard.RandomizeBoard(); 
            Dictionary<int, Tile> tiles = testBoard.Tiles; 
            int two = 0;
            int three = 0;
            int four = 0;
            int five = 0;
            int six = 0;
            int eight = 0;
            int nine = 0;
            int ten = 0;
            int eleven = 0;
            int twelve = 0;

            for (int i = 0; i < tiles.Count; i++)
            {
                int chit_temp = tiles[i].Chit; 
                if (chit_temp == 2)
                {
                    two++;
                }
                else if (chit_temp == 3)
                {
                    three++;
                }
                else if (chit_temp == 4)
                {
                    four++;
                }
                else if (chit_temp == 5)
                {
                    five++;
                }
                else if (chit_temp == 6)
                {
                    six++;
                }
                else if (chit_temp == 8)
                {
                    eight++;
                }
                else if (chit_temp == 9)
                {
                    nine++;
                }
                else if (chit_temp == 10)
                {
                    ten++;
                }
                else if (chit_temp == 11)
                {
                    eleven++;
                }
                else if (chit_temp == 12)
                {
                    twelve++;
                }
            }

            Assert.AreEqual(19, tiles.Count, "expected 19 tiles in the dictionary");
            Assert.AreEqual(1, two, "expected one tiles with chit value 2 in dictionary");
            Assert.AreEqual(2, three, "expected two tiles with chit value 3 in dictionary");
            Assert.AreEqual(2, four, "expected two tiles with chit value 4 in dictionary");
            Assert.AreEqual(2, five, "expected two tiles with chit value 5 in dictionary");
            Assert.AreEqual(2, six, "expected two tiles with chit value 6 in dictionary");
            Assert.AreEqual(2, eight, "expected two tiles with chit value 8 in dictionary");
            Assert.AreEqual(2, nine, "expected two tiles with chit value 9 in dictionary");
            Assert.AreEqual(2, ten, "expected two tiles with chit value 10 in dictionary");
            Assert.AreEqual(2, eleven, "expected two tiles with chit value 11 in dictionary");
            Assert.AreEqual(1, twelve, "expected one tiles with chit value 12 in dictionary");

        }
        [TestMethod]
        public void TestDesertRobber()
        {
            Board testBoard = new Board(0);
            //testBoard.RandomizeBoard(); 
            Dictionary<int, Tile> tiles = testBoard.Tiles; 
            int des_chit = 0;
            bool robber_test = false; 
            for(int i = 0; i < tiles.Count; i++)
            {
                string res_temp = tiles[i].Resource;
                if (res_temp == "desert")
                {
                    des_chit = tiles[i].Chit;
                    robber_test = tiles[i].Robber; 
                }
            }
            Assert.AreEqual(7, des_chit, "expected desert tile to be value 7 in dictionary");
            Assert.AreEqual(true, robber_test, "expected desert tile to hold the robber in dictionary");
        }
        /***********************************************************************/
        // Testing: PlaceSettlement
        /***********************************************************************/
        // Tests include: testing a point that is not on the board, placing a 
        // settlement that is not at least two roads from another settlement
        /***********************************************************************/
        [TestMethod]
        public void TestPlaceSettlementNotOnBoard()
        {
            Board testBoard = new Board(0);
            //testBoard.RandomizeBoard(); 
            
            // testing the upper left quadrant
            Point pt1 = new Point(0, 0);

            // testing the upper right quadrant
            Point pt2 = new Point(10,1);

            // testing the lower left quadrant
            Point pt3 = new Point(1, 5);

            // testing the lower right quadrant
            Point pt4 = new Point(10, 4);

            Player p1 = new Player("Jon");
            Player p2 = new Player("Arya");
            Player p3 = new Player("Davos");
            Player p4 = new Player("Braun of the Black Water");

            bool placeSettlement1 = testBoard.PlaceSettlement(pt1, p1);
            Assert.AreEqual(false, placeSettlement1, "expected false since 0,0 does not exist on the board");

            bool placeSettlement2 = testBoard.PlaceSettlement(pt2, p2);
            Assert.AreEqual(false, placeSettlement2, "expected false since 10,1 does not exist on the board");

            bool placeSettlement3 = testBoard.PlaceSettlement(pt3, p3);
            Assert.AreEqual(false, placeSettlement3, "expected false since 1,5 does not exist on the board");

            bool placeSettlement4 = testBoard.PlaceSettlement(pt4, p4);
            Assert.AreEqual(false, placeSettlement4, "expected false since 10,4 does not exist on the board");
        }
        
        [TestMethod]
        public void TestPlaceSettlementTooClose()
        {
            Board testBoard = new Board(0); 
            //testBoard.RandomizeBoard();
            Player p1 = new Player ("Jon");
            Player p2 = new Player("Arya");
            Player p3 = new Player("Davos");
            Player p4 = new Player("Braun of the Black Water");

            testBoard.AddPlayer(p1);
            testBoard.AddPlayer(p2);
            testBoard.AddPlayer(p3);
            testBoard.AddPlayer(p4);

            // reference point that has a y-coordinate that is odd
            Point pt1 = new Point(3, 1);

            // points that should not work
            Point pt2 = new Point(3, 2);
            Point pt3 = new Point(2, 1);
            Point pt4 = new Point(4, 1);

            // point that should work
            Point pt5 = new Point (5,1);

            bool place1 = testBoard.PlaceSettlement(pt1, p1);
            Assert.AreEqual(true, place1, "expected true since it the reference point");

            bool place2 = testBoard.PlaceSettlement(pt2, p2);
            Assert.AreEqual(false, place2, "expected false because it not two roads away from the reference point");

            bool place3 = testBoard.PlaceSettlement(pt3, p3);
            Assert.AreEqual(false, place3, "expected false because it's not two roads away from the reference point");

            bool place4 = testBoard.PlaceSettlement(pt4, p4);
            Assert.AreEqual(false, place4, "expected false because it's not two roads away from the reference point");

            bool place5 = testBoard.PlaceSettlement(pt5, p2);
            Assert.AreEqual(true, place5, "expected true because it is two roads away from the reference point");

            // reference point that has a y-coordinate that is even
            Point point1 = new Point(6,2);

            // points that should not work 
            Point point2 = new Point(5,2);
            Point point3 = new Point(7,2);
            Point point4 = new Point(6,3);

            // point that should work 
            Point point5 = new Point(8,2);

            bool placeSettlement1 = testBoard.PlaceSettlement(point1, p1);
            Assert.AreEqual(true, placeSettlement1, "expected  true since it is the reference point");

            bool placeSettlement2 = testBoard.PlaceSettlement(point2, p2);
            Assert.AreEqual(false, placeSettlement2, "expected false because zdf it is not two roads away from the reference point");

            bool placeSettlement3 = testBoard.PlaceSettlement(point3, p3);
            Assert.AreEqual(false, placeSettlement3, "expected false because dd it is not two roads away from the reference point");

            bool placeSettlement4 = testBoard.PlaceSettlement(point4, p4);
            Assert.AreEqual(false, placeSettlement4, "expected false because it is not two roads away from the reference point");

            bool placeSettlement5 = testBoard.PlaceSettlement(point5, p2);
            Assert.AreEqual(true, placeSettlement5, "expected true because it is two roads away from the reference point");
        }

        /***********************************************************************/
        // Testing: PlaceRoad
        /***********************************************************************/
        // Tests include: placing a road that does not exist on the board testing 
        // placing a road that is not connected to a settlement
        /***********************************************************************/
        [TestMethod]
        public void TestPlaceRoadNotOnBoard()
        {
            Board testBoard = new Board(0); 
            //testBoard.RandomizeBoard();
            Player p1 = new Player("Jon");

            // testing in the upper left quadrant
            Line line1 = new Line();
            line1.X1 = 0;
            line1.Y1 = 0;
            line1.X2 = 1;
            line1.Y2 = 0; 

            // testing in the upper right quadrant
            Line line2 = new Line();
            line2.X1 = 10;
            line2.Y1 = 0;
            line2.X2 = 10;
            line2.Y2 = 1;

            // testing in the lower left quadrant
            Line line3 = new Line();
            line3.X1 = 0;
            line3.Y1 = 4;
            line3.X2 = 0;
            line3.Y2 = 5;

            // testing the lower right quadrant
            Line line4 = new Line();
            line4.X1 = 10;
            line4.Y1 = 4;
            line4.X2 = 10;
            line4.Y2 = 5;

            bool road1 = testBoard.PlaceRoad(line1, p1);
            Assert.AreEqual(false, road1, "expected false because this road does not exist on the board");

            bool road2 = testBoard.PlaceRoad(line2, p1);
            Assert.AreEqual(false, road2, "expected false  because this road does not exist on the board");

            bool road3 = testBoard.PlaceRoad(line3, p1);
            Assert.AreEqual(false, road3, "expected false because this road does not exist on the board");

            bool road4 = testBoard.PlaceRoad(line4, p1);
            Assert.AreEqual(false, road4, "expected false because this road does not exist on the board");
        }

        [TestMethod]
        public void TestBuggyRoad()
        {
            Board testBoard = new Board(0);
            Player p1 = new Player("Sansa");
            Player p2 = new Player("Hound");
            Player p3 = new Player("Gendry");
            Point pt1 = new Point(4, 3);

            testBoard.AddPlayer(p1);
            testBoard.AddPlayer(p2);
            testBoard.AddPlayer(p3);

            bool place = testBoard.PlaceSettlement(pt1, p1);

            Assert.AreEqual(true, place, "Expected settlement to be able to be placed");

            Line road = new Line(new Point(5,3), new Point(4,3));

            bool road_placed = testBoard.PlaceRoad(road, p1);
            Console.WriteLine("Was the road placed? {0}", road_placed);
            Assert.AreEqual(true, road_placed, "Expected road to be placed");
        }

        [TestMethod]
        public void TestBuggyRoad2()
        {
            Board testBoard = new Board(0);
            Player p1 = new Player("Sansa");
            Player p2 = new Player("Hound");
            Player p3 = new Player("Gendry");
            Point pt1 = new Point(2, 1);

            testBoard.AddPlayer(p1);
            testBoard.AddPlayer(p2);
            testBoard.AddPlayer(p3);

            bool place = testBoard.PlaceSettlement(pt1, p1);

            Assert.AreEqual(true, place, "Expected settlement to be able to be placed");

            Line road = new Line(new Point(2,1), new Point(1,1));

            bool road_placed = testBoard.PlaceRoad(road, p1);
            Console.WriteLine("Was the road placed? {0}", road_placed);
            Assert.AreEqual(true, road_placed, "Expected road to be placed");
        }

        [TestMethod]
        public void TestBuggyRoad22()
        {
            Board testBoard = new Board(0);
            Player p1 = new Player("Sansa");
            Player p2 = new Player("Hound");
            Player p3 = new Player("Gendry");
            Point pt1 = new Point(2, 1);

            testBoard.AddPlayer(p1);
            testBoard.AddPlayer(p2);
            testBoard.AddPlayer(p3);

            bool place = testBoard.PlaceSettlement(pt1, p1);

            Assert.AreEqual(true, place, "Expected settlement to be able to be placed");

            Line road = new Line(new Point(1,1), new Point(2,1));

            bool road_placed = testBoard.PlaceRoad(road, p1);
            Console.WriteLine("Was the road placed? {0}", road_placed);
            Assert.AreEqual(true, road_placed, "Expected road to be placed");
        }

        [TestMethod]
        public void TestBuggyRoad3()
        {
            Board testBoard = new Board(0);
            Player p1 = new Player("Sansa");
            Player p2 = new Player("Hound");
            Player p3 = new Player("Gendry");
            Point pt1 = new Point(2, 2);

            testBoard.AddPlayer(p1);
            testBoard.AddPlayer(p2);
            testBoard.AddPlayer(p3);

            bool place = testBoard.PlaceSettlement(pt1, p1);

            Assert.AreEqual(true, place, "Expected settlement to be able to be placed");

            Line road = new Line(new Point(2,2), new Point(2,3));

            bool road_placed = testBoard.PlaceRoad(road, p1);
            Console.WriteLine("Was the road placed? {0}", road_placed);
            Assert.AreEqual(true, road_placed, "Expected road to be placed");
        }

        [TestMethod]
        public void TestTileLines()
        {
            Board testBoard = new Board(0);
            Player p1 = new Player("Sansa");
            Player p2 = new Player("Hound");
            Player p3 = new Player("Gendry");

            testBoard.AddPlayer(p1);
            testBoard.AddPlayer(p2);
            testBoard.AddPlayer(p3);

            Dictionary<int, Tile> tile_dict = testBoard.Tiles;

            for (int i = 0; i < 19; i++)
            {
                foreach (Line line in tile_dict[i].Roads)
                {
                    Console.WriteLine("Line for tile {0}: {1}", i, line.ToString());
                }
                Console.WriteLine("---------------------------------------------");
            }
        }

        [TestMethod]
        public void TestPlaceRoadNotConnectedToSettlement()
        {
            Board testBoard = new Board(0);
            Player p1 = new Player("Sansa");
            Point pt1 = new Point(3, 1);

            testBoard.AddPlayer(p1);

            bool place = testBoard.PlaceSettlement(pt1, p1);
            Console.WriteLine(place);
            
            // (X1, Y1) keeps getting added to settlements and idk why HELP
            // road that does not touch the reference settlement
            Line line1 = new Line();
            line1.X1 = 5;
            line1.Y1 = 4;
            line1.X2 = 6;
            line1.Y2 = 3; 

            // road that does touch the reference settlement
            Line line2 = new Line();
            line2.X1 = 3;
            line2.Y1 = 1;
            line2.X2 = 3;
            line2.Y2 = 2; 
            
            bool badRoad = testBoard.PlaceRoad(line1, p1);
            Assert.AreEqual(false, badRoad, "expected false because this road does not touch the reference settlement");

            bool goodRoad = testBoard.PlaceRoad(line2, p1);
            Assert.AreEqual(true, goodRoad, "expected true because this road touches the reference settlement");
        }
    
        /***********************************************************************/
        // Testing: PlaceCity
        /***********************************************************************/
        // Tests include: placing a city where there isn't already a settlement,
        // placing a city on a settlement that exists but is the wrong player's
        /***********************************************************************/
        [TestMethod]
        public void TestPlaceCityNoSettlement()
        {
            Board testBoard = new Board(0); 
            //testBoard.RandomizeBoard(); 
            Player p1 = new Player("Jon");
            Player p2 = new Player ("Arya");

            testBoard.AddPlayer(p1);
            testBoard.AddPlayer(p2);

            // reference settlement point
            Point pt1 = new Point(3,1);

            bool placeSettlement = testBoard.PlaceSettlement(pt1, p1);
            Assert.AreEqual(true, placeSettlement, "expected true because this settlement is on the board and not taken");

            // creating a point where you theoretically can't place a settlement
            Point pt2 = new Point(5, 3);
            bool placeCity1 = testBoard.PlaceCity(pt2, p1);
            Assert.AreEqual(false, placeCity1, "expected false, because there is no settlement at this point");

            bool placeCity2 = testBoard.PlaceCity(pt1, p1);
            Assert.AreEqual(true, placeCity2, "expected true, because there is already a settlement here with  the same player");

            bool placeCity3 = testBoard.PlaceCity( pt1, p2);
            Assert.AreEqual(false, placeCity3, "expected false, because this settlement was not claimed by the correct player");
        }
    }
}