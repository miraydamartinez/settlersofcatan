using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using System.Drawing;

namespace CatanApp
{
    [TestClass]
    public class CatanTest 
    {
        /***********************************************************************/
        // Testing: AddPlayer
        /***********************************************************************/
        // Tests include: whether player was correctly added to game's list of
        // players
        /***********************************************************************/

        [TestMethod]
        public void TestAddPlayer()
        {
            Catan catan_test = new Catan();
            catan_test.AddPlayer("Jorah");
            catan_test.AddPlayer("Selmy");

            Assert.IsTrue(catan_test.Players.Contains(new Player("Jorah")));
            Assert.IsTrue(catan_test.Players.Contains(new Player("Selmy")));
            Assert.IsFalse(catan_test.Players.Contains(new Player("Ulfric")));

            Assert.IsTrue(catan_test.Players[0].Name == "Jorah");
            Assert.IsTrue(catan_test.Players[1].Name == "Selmy");
        }

        /***********************************************************************/
        // Testing: CollectResource
        /***********************************************************************/
        // Tests include: player collecting resources based on placing initial
        // settlements and rolling die, players placing settlements, cities, and
        // cities with and without cost
        /***********************************************************************/

        [TestMethod]
        public void TestCollectResource()
        {
            Catan catan_test = new Catan(0);

            Player jon = new Player("Jon");

            catan_test.AddPlayer(jon);

            // give jon resources to be able to build
            for (int i = 0; i < 7; i ++)
            {
                jon.AddResource("wood");
                jon.AddResource("brick");
                jon.AddResource("sheep");
                jon.AddResource("wheat");
                jon.AddResource("ore");
            }

            catan_test.SettlementClicked(new Point(3,1), jon);
            catan_test.SettlementClicked(new Point(5,3), jon);

            catan_test.CollectResources(1, 2);
            catan_test.CollectResources(5, 3);
            catan_test.CollectResources(5, 6);

            Dictionary<string, int> jon_res = PlayerResourceCounts(jon);

            // Check resource counts
            Assert.AreEqual(8, jon_res["ore"], "Expected 8 ore");
            Assert.AreEqual(8, jon_res["brick"], "Expected 8 brick");
            Assert.AreEqual(10, jon_res["wood"], "Expected 10 wood");
            Assert.AreEqual(8, jon_res["sheep"], "Expected 8 sheep");
            Assert.AreEqual(9, jon_res["wheat"], "Expected 9 wheat");
        }

        /* NO LONGER WORKS WITH NEW TURN SYSTEM */
        // [TestMethod]
        // public void TestBuildWithCost()
        // {
        //     Catan catan_test = new Catan(0);

        //     Player jon = new Player("Jon");
        //     Player arya = new Player("Arya");
        //     Player davos = new Player("Davos");
        //     Player braun = new Player("Braun of the Black Water");

        //     catan_test.AddPlayer(jon);
        //     catan_test.AddPlayer(arya);
        //     catan_test.AddPlayer(davos);
        //     catan_test.AddPlayer(braun);


        //     // give jon resources to be able to build
        //     for (int i = 0; i < 7; i ++)
        //     {
        //         jon.AddResource("wood");
        //         jon.AddResource("brick");
        //         jon.AddResource("sheep");
        //         jon.AddResource("wheat");
        //         jon.AddResource("ore");
        //     }

        //     // players place settlements
        //     bool placed = catan_test.SettlementClicked(new Point(3,1), jon);

        //     Console.WriteLine("Settlement placed: {0}", placed);
        //     placed = catan_test.SettlementClicked(new Point(6,2), arya);
        //     Console.WriteLine("Settlement placed: {0}", placed);
        //     placed = catan_test.SettlementClicked(new Point(2,4), davos);
        //     Console.WriteLine("Settlement placed: {0}", placed);
        //     placed = catan_test.SettlementClicked(new Point(4,0), braun);
        //     Console.WriteLine("Settlement placed: {0}", placed);
        //     placed = catan_test.SettlementClicked(new Point(7,0), braun);
        //     Console.WriteLine("Settlement placed: {0}", placed);
        //     placed = catan_test.SettlementClicked(new Point(6,4), davos);
        //     Console.WriteLine("Settlement placed: {0}", placed);
        //     placed = catan_test.SettlementClicked(new Point(2,3), arya);
        //     Console.WriteLine("Settlement placed: {0}", placed);
        //     placed = catan_test.SettlementClicked(new Point(5,3), jon);
        //     Console.WriteLine("Settlement placed: {0}", placed);

        //     catan_test.CollectResources(1, 2);
        //     catan_test.CollectResources(5, 3);
        //     catan_test.CollectResources(5, 6);

        //     Dictionary<string, int> jon_res = PlayerResourceCounts(jon);

        //     // Check resource counts
        //     Assert.AreEqual(8, jon_res["ore"], "Expected 8 ore");
        //     Assert.AreEqual(8, jon_res["brick"], "Expected 8 brick");
        //     Assert.AreEqual(10, jon_res["wood"], "Expected 10 wood");
        //     Assert.AreEqual(8, jon_res["sheep"], "Expected 8 sheep");
        //     Assert.AreEqual(9, jon_res["wheat"], "Expected 9 wheat");

        //     catan_test.EndTurn();

        //     /* Turn 2 */

        //     // Buys city
        //     catan_test.CityClicked(new Point(5,3), jon);

        //     jon_res = PlayerResourceCounts(jon);

        //     // Check resource counts
        //     Assert.AreEqual(10, jon_res["wood"], "Expected 10 wood");
        //     Assert.AreEqual(5, jon_res["ore"], "Expected 5 ore");
        //     Assert.AreEqual(7, jon_res["wheat"], "Expected 7 wheat");


        //     // Buys road
        //     Line line = new Line();
        //     line.X1 = 5;
        //     line.Y1 = 3;
        //     line.X2 = 6;
        //     line.Y2 = 3;

        //     catan_test.RoadClicked(line, jon);
        //     jon_res = PlayerResourceCounts(jon);

        //     // Check resource counts
        //     Assert.AreEqual(5, jon_res["ore"], "Expected 5 ore");
        //     Assert.AreEqual(7, jon_res["brick"], "Expected 7 brick");
        //     Assert.AreEqual(9, jon_res["wood"], "Expected 9 wood");
        //     Assert.AreEqual(8, jon_res["sheep"], "Expected 8 sheep");
        //     Assert.AreEqual(7, jon_res["wheat"], "Expected 7 wheat");
        // }

        [TestMethod]
        public void TestBuggyRoad()
        {
            Catan catan_test = new Catan(0);

            Player jon = new Player("Jon");
            Player arya = new Player("Arya");

            catan_test.AddPlayer(jon);
            catan_test.AddPlayer(arya);

            bool settl_placed = catan_test.SettlementClicked(new Point(4,3), jon);

            bool road_placed = catan_test.RoadClicked(new Line(new Point(4,3), new Point(5,3)), jon);

            Assert.IsTrue(settl_placed, "Expected settlement to have been placed");
            Assert.IsTrue(road_placed, "Expected setup road 1 jon to have been placed");

            settl_placed = catan_test.SettlementClicked(new Point(3,4), arya);

            road_placed = catan_test.RoadClicked(new Line(new Point(3,4), new Point(4,4)), arya);

            Assert.IsTrue(settl_placed, "Expected settlement to have been placed");
            Assert.IsTrue(road_placed, "Expected setup road 1 arya to have been placed");


            settl_placed = catan_test.SettlementClicked(new Point(5,2), arya);

            road_placed = catan_test.RoadClicked(new Line(new Point(5,2), new Point(5,1)), arya);

            Assert.IsTrue(settl_placed, "Expected settlement to have been placed");
            Assert.IsTrue(road_placed, "Expected setup road 2 arya to have been placed");


            settl_placed = catan_test.SettlementClicked(new Point(7,4), jon);

            road_placed = catan_test.RoadClicked(new Line(new Point(7,4), new Point(6,4)), jon);

            Assert.IsTrue(settl_placed, "Expected settlement to have been placed");
            Assert.IsTrue(road_placed, "Expected setup road 2 jon to have been placed");

            for (int i = 0; i < 4; i++)
            {
                jon.AddResource("brick");
                jon.AddResource("wood");
                jon.AddResource("wheat");
                jon.AddResource("sheep");
            }

            catan_test.RoadClicked(new Line(new Point(6,4), new Point(5,4)), jon);
            Assert.IsTrue(road_placed, "Expected road to have been placed");

            Console.WriteLine("Placing final settlement");
            
            settl_placed = catan_test.SettlementClicked(new Point(5,4), jon);
            Assert.IsTrue(settl_placed, "Expected test settlement jon to have been placed");

        }

        public Dictionary<string, int> PlayerResourceCounts(Player player)
        {
            List<string> resources = player.Resources;
            int wood_ct = 0;
            int wheat_ct = 0;
            int brick_ct = 0;
            int ore_ct = 0;
            int sheep_ct = 0;

            foreach (string res in resources)
            {
                if (res == "wood")
                {
                    wood_ct++;
                }
                if (res == "ore")
                {
                    ore_ct++;
                }
                if (res == "brick")
                {
                    brick_ct++;
                }
                if (res == "wheat")
                {
                    wheat_ct++;
                }
                if (res == "sheep")
                {
                    sheep_ct++;
                }
            }

            Dictionary<string, int> r_dict = new Dictionary<string, int>();
            r_dict.Add("wood", wood_ct);
            r_dict.Add("brick", brick_ct);
            r_dict.Add("wheat", wheat_ct);
            r_dict.Add("sheep", sheep_ct);
            r_dict.Add("ore", ore_ct);
            
            return r_dict;
        }
    }
}