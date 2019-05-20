using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using System.Drawing;

namespace CatanApp
{
    [TestClass]
    public class PlayerTest 
    {
        /***********************************************************************/
        // Testing: AddResource
        /***********************************************************************/
        // Tests include: tests if string is valid and not valid
        /***********************************************************************/
        [TestMethod]
        public void TestAddResource()
        {
            Player p1 = new Player("Jon");
            p1.AddResource("ore");
            Assert.AreEqual(1, p1.Resources.Count, "expected 1 resource added to the player's hand");

            Player p2 = new Player("Arya");
            p2.AddResource("yo, what's up?!");
            Assert.AreEqual(1, p1.Resources.Count, "expected 0 resources added to the player's hand");
        }
       
        /***********************************************************************/
        // Testing: RemoveResource
        /***********************************************************************/
        // Tests include: tests if string is valid and not valid, tests if the
        // you try to remove a resource that the player does not have
        /***********************************************************************/
        [TestMethod]
        public void TestRemoveResource()
        {
            Player p1 = new Player("Jon");
            p1.AddResource("wheat");
            p1.AddResource("brick");
            p1.AddResource("wood");
            p1.AddResource("sheep");
            Assert.AreEqual(4, p1.Resources.Count, "expected 4 resource in the player's hand");

            p1.RemoveResource("ore");
            Assert.AreEqual(4, p1.Resources.Count, "expected 4 resources since RemoveResource should have failed");
            p1.RemoveResource("hey there!");
            Assert.AreEqual(4, p1.Resources.Count, "expected 4 resrouces on invalid input string");
        }
        /***********************************************************************/
        // Testing: ResourceCount
        /***********************************************************************/
        // Tests include: tests if a correct number of the resource is returned
        /***********************************************************************/
        [TestMethod]
        public void TestResourceCount()
        {
            Player p1 = new Player("Jon");
            p1.AddResource("wheat");
            p1.AddResource("brick");
            p1.AddResource("wood");
            p1.AddResource("sheep");
            p1.AddResource("ore");
            int numWheat = p1.ResourceCount("wheat");
            int numBrick = p1.ResourceCount("brick");
            int numWood = p1.ResourceCount("wood");
            int numSheep = p1.ResourceCount("sheep");
            int numOre = p1.ResourceCount("ore");

            Assert.AreEqual(1, numWheat, "expected 1 resource");
            Assert.AreEqual(1, numBrick, "expected 1 resource");
            Assert.AreEqual(1, numWood, "expected 1 resource");
            Assert.AreEqual(1, numSheep, "expected 1 resource");
            Assert.AreEqual(1, numOre, "expected 1 resource");
            Assert.AreEqual(5, p1.Resources.Count, "expected 5 resources in the player's hand");   
        }
    
        /***********************************************************************/
        // Testing: CanBuildSettlement
        /***********************************************************************/
        // Tests include: tests if the player has enough of the correct resources
        // to build a settlement, tests if the player does not have the correct
        // number of resources
        /***********************************************************************/
        [TestMethod]
        public void TestCanBuildSettlementYes()
        {
            Player p1 = new Player("Jon");
            p1.AddResource("wood");
            p1.AddResource("sheep");
            p1.AddResource("brick");
            p1.AddResource("wheat");
            bool yes = p1.CanBuildSettlement();

            Assert.AreEqual(true, yes, "expected true: the player can build a settlement");
        }
        [TestMethod]
        public void TestCanBuildSettlementNo()
        {
            Player p1 = new Player("Jon");
            p1.AddResource("wood");
            p1.AddResource("ore");
            p1.AddResource("brick");
            bool no = p1.CanBuildSettlement();

            Assert.AreEqual(false, no, "expected false: the player cannot build a settlement");   
        }

        /***********************************************************************/
        // Testing: CanBuildRoad
        /***********************************************************************/
        // Tests include: tests if the player has enough of the correct resources
        // to build a road, tests if the player does not have the correct
        // number of resources
        /***********************************************************************/
        [TestMethod]
        public void TestCanBuildRoadYes()
        {
            Player p1 = new Player("Jon");
            p1.AddResource("wood");
            p1.AddResource("brick");
            bool yes = p1.CanBuildRoad();

            Assert.AreEqual(true, yes, "expected true: the player can build a road");
        }
        [TestMethod]
        public void TestCanBuildRoadNo()
        {
            Player p1 = new Player("Jon");
            p1.AddResource("ore");
            bool no = p1.CanBuildRoad();

            Assert.AreEqual(false, no, "expected false: the player cannot build a road");
        }

        /***********************************************************************/
        // Testing: CanBuildCity
        /***********************************************************************/
        // Tests include: tests if the player has enough of the correct resources
        // to build a City, tests if the player does not have the correct
        // number of resources
        /***********************************************************************/
        [TestMethod]
        public void TestCanBuildCityYes()
        {
            Player p1 = new Player("Jon");
            p1.AddResource("ore");
            p1.AddResource("ore");
            p1.AddResource("ore");
            p1.AddResource("wheat");
            p1.AddResource("wheat");
            bool yes = p1.CanBuildCity();

            Assert.AreEqual(true, yes, "expected true: the player can build a city");
        }
        [TestMethod]
        public void TestCanBuildCityNo()
        {
            Player p1 = new Player("Jon");
            p1.AddResource("wheat");
            p1.AddResource("wheat");
            p1.AddResource("wheat");
            p1.AddResource("wheat");
            p1.AddResource("brick");
            bool no = p1.CanBuildCity();

            Assert.AreEqual(false, no, "expected false: the player cannot build a city");
        }
        /***********************************************************************/
        // Testing: CanBuyDevelopmentCard
        /***********************************************************************/
        // Tests include: tests if the player has enough of the correct resources
        // to buy a development card, tests if the player does not have the correct
        // number of resources
        /***********************************************************************/
        [TestMethod]
        public void TestCanBuyDevelopmentCardYes()
        {
            Player p1 = new Player("Jon");
            p1.AddResource("ore");
            p1.AddResource("sheep");
            p1.AddResource("wheat");
            bool yes = p1.CanBuyDevelopmentCard();

            Assert.AreEqual(true, yes, "expected true: the player can buy a development card");
        }
        [TestMethod]
        public void TestCanBuyDevelopmentCardNo()
        {
            Player p1 = new Player("Jon");
            p1.AddResource("ore");
            p1.AddResource("brick");
            p1.AddResource("brick");
            bool no = p1.CanBuyDevelopmentCard();

            Assert.AreEqual(false, no, "expected false: the player cannot buy a development card");
        }
        /***********************************************************************/
        // Testing: AddVictoryPoints
        /***********************************************************************/
        // Tests include: tests if victory points is accurately incremented
        /***********************************************************************/
        [TestMethod]
        public void TestAddVictoryPoint()
        {
            Player p1 = new Player("Jon");
            Assert.AreEqual(0, p1.VictoryPoints, "expected 0 victory points");

            p1.AddVictoryPoint(); 
            Assert.AreEqual(1, p1.VictoryPoints, "expected 1 victory points");
        }
    }
}