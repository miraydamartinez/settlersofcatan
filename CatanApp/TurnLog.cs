using System;
using System.Collections.Generic;
using System.Drawing;

namespace CatanApp
{
    public class TurnLog
    {

        string _message = "";

        string _setupmessage = "";

        // effects: prints a string that is the initial welcome message 
        public void Welcome()
        {
            _message = "Hello, settlers! Welcome to Catan. Please play with integrity and be respectful. Enjoy! \n";
        }

        // requires: nothing
        // effects: sets message string to say which player's turn it is
        public void PlayerTurnMessage(Player player)
        {
            _message = "It is now " + player.Name + "'s turn. Roll the dice to begin. -------------------------------- \n";
        }


        public void ResourcesCollectedMessage(int die1, int die2)
        {
            int roll = die1 + die2;
            string roll_str = roll.ToString();
            _message = roll_str + " was rolled, resources collected. \n";
        }

        // requires: valid player in the Catan game as well as bool that indicates
        // whether the player has enough resources to claim the road
        // effects: returns a string that will be used to print in the Turn Losg
        public void SettlementMessage(Player player, bool placed)
        {
            if (placed)
            {
                _message = "Player " + player.Name + " claimed a settlement. \n";
            }
            else
            {
                _message = "Player " + player.Name + " cannot claim this settlement. \n";    
            }
        }

        // requires: valid player in the Catan game as well as bool that indicates
        // whether the player has enough resources to claim the road
        // effects: returns a string that will be used to print in the Turn Log
        public void RoadMessage(Player player, bool placed)
        {
            if (placed)
            {
                _message = "Player " + player.Name + " claimed a road. \n";
            }
            else
            {
                _message = "Player " + player.Name + " cannot claim this road. \n";    
            }
        }

        // requires: valid player in the Catan game as well as bool that indicates
        // whether the player has enough resources to claim the road
        // effects: returns a string that will be used to print in the Turn Log
        public void CityMessage(Player player, bool placed)
        {
            if (placed)
            {
                _message = "Player " + player.Name + " upgraded a settlement to a city. \n";
            }
            else
            {
                _message = "Player " + player.Name + " cannot upgrade that settlement to a city. \n";    
            }
        }

        // requires: player rolled a 7
        // effects: changes turnlog message to instruct the user to move the robber
        public void RobberMessage()
        {
            _message = "Select which tile you would like to move the robber to. \n";
        }

        // requires: nothing
        // effects: updates message saying player moved the robber
        public void RobberMovedMessage(Player player)
        {
            _message = player.Name + " moved the robber. \n";
        }

        public void EndSetupMessage(Player player)
        {
            _setupmessage = "It is now " + player.Name + "'s turn. Roll the dice to begin. -------------------------------- \n";
        }

        public void NewSetupTurn(Player player)
        {
            _setupmessage = "It is now " + player.Name + "'s turn. Place a settlement and a road. --------------------------- \n \n";
        }

        public void EmptySetupMessage()
        {
            _setupmessage = "";
        }
        
        public string Message
        {
            get { return _message; }
        }

        public string SetupMessage
        {
            get { return _setupmessage; }
        }
    }
}